using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Vevidi.FindDiff.NetworkModel;

namespace Vevidi.FindDiff.GameEditor
{
    [ExecuteInEditMode]
    public class DifferenceArea : DebugableArea
    {
        private int id;
        private bool selected = false;
        private DifferenceArea secondArea;
        private GameEditorManager manager;
        private DifferenceInfoModel model;
        private int offsetX = 0;
        private int offsetY = 0;

        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                gameObject.name = id.ToString();
                secondArea.id = value;
                secondArea.gameObject.name = id.ToString();
            }
        }
        public RectTransform ThisTransform { get { return thisTransform; } }
        public bool IsSelectable { get; set; } = true;

        public void Init(DifferenceInfoModel model, GameEditorManager manager, int offsetX, int offsetY)
        {
            id = model.Id;
            this.manager = manager;
            this.model = model;
            gameObject.name = id.ToString();
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            thisTransform.localPosition = new Vector3(model.X + offsetX, model.Y + offsetY);
            thisTransform.localScale = Vector3.one;
            thisTransform.sizeDelta = Vector2.one * model.Radius * 2f;
            CreateDebugPoints(model.Radius);
        }

        public void SetPair(DifferenceArea second)
        {
            secondArea = second;
        }

        public void SetSelected(bool selected = true)
        {
            this.selected = selected;
            Update();
        }
        // there is no another solution
        private void Update()
        {
            if (selected && transform.hasChanged && secondArea != null)
            {
                transform.hasChanged = false;
                secondArea.ThisTransform.localPosition = new Vector3(thisTransform.localPosition.x - offsetX, thisTransform.localPosition.y - offsetY);
            }
        }

        private void OnRectTransformDimensionsChange()
        {
            if (thisTransform != null)
            {
                Vector2 currSizeDelta = thisTransform.sizeDelta;
                thisTransform.sizeDelta = new Vector2(currSizeDelta.x, currSizeDelta.x);
                model.Radius = currSizeDelta.x * 0.5f;
                UpdateDebugPointsRadius(model.Radius);
                if (secondArea != null)
                    secondArea.ThisTransform.sizeDelta = thisTransform.sizeDelta;
            }
        }

        public (int id, float x, float y, float r) GetInfo()
        {
            Vector3 pos = thisTransform.localPosition;
            return (model.Id, pos.x - offsetX, pos.y - offsetY, model.Radius);
        }

    }
}