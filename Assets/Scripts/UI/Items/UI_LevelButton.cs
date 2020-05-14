using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vevidi.FindDiff.GameLogic;
using Vevidi.FindDiff.GameModel;
using Vevidi.FindDiff.GameUtils;

namespace Vevidi.FindDiff.UI
{
    public class UI_LevelButton : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField]
        private GameObject blurredImage;
        [SerializeField]
        private GameObject levelPassedCheckmark;
#pragma warning restore 0649

        private Button levelButton;
        private Image buttonImage;
        private LevelDescriptionModel levelDescription;

        private void Awake()
        {
            levelButton = GetComponentInChildren<Button>();
            buttonImage = levelButton.image;
        }

        public void Init(LevelDescriptionModel model)
        {
            levelDescription = model;
            //Rect buttonRect = buttonImage.rectTransform.rect;
            int width = model.LevelImage.width;
            int height = model.LevelImage.height;

            Texture2D buttonImageTexture = new Texture2D(width, height);

            buttonImageTexture.SetPixels(0, 0, width, height, model.LevelImage.GetPixels());
            buttonImageTexture.Apply();

            Rect newRect = new Rect(0, 0, buttonImageTexture.width / 2, buttonImageTexture.height);
            buttonImage.overrideSprite = Utils.GetSpriteFromTex2D(buttonImageTexture,newRect);

            //Sprite.Create(buttonImageTexture, newRect, Vector2.one * 0.5f);

            levelButton.onClick.AddListener(OnClick);

            if (levelDescription.IsEnded)
                levelPassedCheckmark.SetActive(true);
            else if (!levelDescription.IsOpened)
            {
                blurredImage.SetActive(true);
                levelButton.enabled = false;
            }
        }

        private void OnDestroy()
        {
            levelButton.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            Debug.Log("Selecting level: " + levelDescription.Id);
            SoundsManager.Instance.PlaySound(SoundsManager.eSoundType.Click);
            GameManager.Instance.LevelsManager.SelectLevel(levelDescription.Id);
            SceneManager.LoadScene(GameVariables.LevelScene);
        }
    }
}
