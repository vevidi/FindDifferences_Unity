using UnityEngine;
using UnityEngine.SceneManagement;
using Vevidi.FindDiff.GameLogic;
using Vevidi.FindDiff.GameModel;

namespace Vevidi.FindDiff.UI
{
    public class ScrollView3DItem : MonoBehaviour
    {
#pragma warning disable 0649
        //[SerializeField]
        //private GameObject blurredImage;
        [SerializeField]
        private GameObject levelPassedCheckmark;
        [SerializeField]
        private GameObject levelPassedGradient;
        [SerializeField]
        private Renderer imageWithGradient;
        [SerializeField]
        private Shader unlitGrayscaleShader;
        [SerializeField]
        private Shader gradientGrayscaleShader;
#pragma warning restore 0649

        public int Id { get { return levelDescription.Id; } }

        private LevelDescriptionModel levelDescription;
        private Renderer thisRenderer;
        private Material thisMaterial;
        private Material thisGradientMaterial;
        private Material materialAreaCopy;
        private bool isBlocked = false;

        private void Awake()
        {
            thisRenderer = GetComponent<Renderer>();
            thisMaterial = new Material(thisRenderer.sharedMaterial);
            thisGradientMaterial = new Material(imageWithGradient.sharedMaterial);
            thisRenderer.sharedMaterial = thisMaterial;
            imageWithGradient.sharedMaterial = thisGradientMaterial;
        }

        public void Init(LevelDescriptionModel model)
        {
            levelDescription = model;
            int width = model.LevelImage.width;
            int height = model.LevelImage.height;

            RenderTexture rt = new RenderTexture(width, height, 16, RenderTextureFormat.ARGB32);
            rt.Create();
            Graphics.Blit(model.LevelImage, rt);
            RenderTexture.active = rt;
            Texture2D buttonImageTexture = new Texture2D(width / 2, height);
            buttonImageTexture.ReadPixels(new Rect(0, 0, width / 2, height), 0, 0);
            buttonImageTexture.Apply(false);

            //thisRenderer.sharedMaterial = new Material(thisRenderer.sharedMaterial)
            //{
            //    mainTexture = buttonImageTexture
            //};
            //imageWithGradient.sharedMaterial = new Material(imageWithGradient.sharedMaterial)
            //{
            //    mainTexture = buttonImageTexture
            //};
            thisMaterial.mainTexture = buttonImageTexture;
            thisGradientMaterial.mainTexture = buttonImageTexture;

            if (levelDescription.IsEnded)
            {
                levelPassedCheckmark.SetActive(true);
                levelPassedGradient.SetActive(true);
            }
            else if (!levelDescription.IsOpened)
            {
                MakeGrayscale();
            }
        }

        public void MakeGrayscale()
        {
            thisMaterial.shader = unlitGrayscaleShader;
            thisGradientMaterial.shader = gradientGrayscaleShader;
            thisRenderer.sharedMaterial = thisMaterial;
            imageWithGradient.sharedMaterial = thisGradientMaterial;
        }

        public void OnClick()
        {
            if (!isBlocked && levelDescription.IsOpened)
            {
                SoundsManager.Instance.PlaySound(SoundsManager.eSoundType.Click);
                GameManager.Instance.LevelsManager.SelectLevel(levelDescription.Id);
                SceneManager.LoadScene(GameVariables.LevelScene);
            }
        }

        public void BlockItem(bool isBlocked)
        {
            this.isBlocked = isBlocked;
        }
    }
}
