using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vevidi.FindDiff.GameModel;

namespace Vevidi.FindDiff.UI
{
    public class UI_LevelButton : MonoBehaviour
    {
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

            Debug.LogWarning("Model: " + model + " l img: " + model.LevelImage);

            Rect rect = new Rect(0, 0, model.LevelImage.width, model.LevelImage.height);
            buttonImage.overrideSprite = Sprite.Create(model.LevelImage,rect, Vector2.one * 0.5f);
        }

    }
}
