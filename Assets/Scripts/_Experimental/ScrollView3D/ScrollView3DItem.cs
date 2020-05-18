using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vevidi.FindDiff.GameLogic;
using Vevidi.FindDiff.GameModel;
using Vevidi.FindDiff.GameUtils;

namespace Vevidi.Experimental
{
    public class ScrollView3DItem : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField]
        private GameObject blurredImage;
        [SerializeField]
        private GameObject levelPassedCheckmark;
        [SerializeField]
        private GameObject levelPassedGradient;
        [SerializeField]
        private Renderer imageWithGradient;
#pragma warning restore 0649

        private LevelDescriptionModel levelDescription;
        private Renderer thisRenderer;
        private Material materialAreaCopy;

        private void Awake()
        {
            thisRenderer = GetComponent<Renderer>();
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
            buttonImageTexture.ReadPixels(new Rect(0,0,width/2,height),0,0);
            buttonImageTexture.Apply(false);

            thisRenderer.sharedMaterial = new Material(thisRenderer.sharedMaterial);
            thisRenderer.sharedMaterial.mainTexture = buttonImageTexture;
            imageWithGradient.sharedMaterial.mainTexture = buttonImageTexture;

            if (levelDescription.IsEnded)
            {
                levelPassedCheckmark.SetActive(true);
                levelPassedGradient.SetActive(true);
            }
            else if (!levelDescription.IsOpened)
            {
                blurredImage.SetActive(true);
                //levelButton.enabled = false;
            }
        }

        //private void OnDestroy()
        //{
        //    levelButton.onClick.RemoveListener(OnClick);
        //}

        private void OnClick()
        {
            SoundsManager.Instance.PlaySound(SoundsManager.eSoundType.Click);
            GameManager.Instance.LevelsManager.SelectLevel(levelDescription.Id);
            SceneManager.LoadScene(GameVariables.LevelScene);
        }
    }
}
