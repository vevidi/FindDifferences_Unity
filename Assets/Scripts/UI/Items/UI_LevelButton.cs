using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vevidi.FindDiff.GameModel;

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
            levelButton = GetComponent<Button>();
            buttonImage = GetComponent<Image>();
        }

        public void Init(LevelDescriptionModel model)
        {
            levelDescription = model;
            Rect buttonRect = buttonImage.rectTransform.rect;
            Texture2D buttonImageTexture = new Texture2D(model.LevelImage.width, model.LevelImage.height);

            buttonImageTexture.SetPixels(model.LevelImage.GetPixels());
            buttonImageTexture.Apply();

            Rect newRect = new Rect(0, 0, buttonImageTexture.width, buttonImageTexture.height);
            buttonImage.overrideSprite = Sprite.Create(buttonImageTexture, newRect, Vector2.one * 0.5f);

            if (levelDescription.IsEnded)
                levelPassedCheckmark.SetActive(true);
            else if (!levelDescription.IsOpened)
            {
                blurredImage.SetActive(true);
                levelButton.enabled = false;
            }
        }

    }
}
