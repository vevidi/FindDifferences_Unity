using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vevidi.FindDiff.GameMediator;
using Vevidi.FindDiff.GameMediator.Commands;
using Vevidi.FindDiff.GameUtils;
using Vevidi.FindDiff.NetworkModel;

namespace Vevidi.FindDiff.GameLogic
{
    public class TouchableArea : DebugableArea, IPointerClickHandler
    {
        private DifferenceInfoModel model;
        private ClickableBackground parentClickArea;
        private ParticleSystem wavesParticles;
        private float radius = 50f;

        public void Init(DifferenceInfoModel model, int offsetX = 0, int offsetY = 0)
        {
            this.model = model;
            radius = model.Radius;
            thisTransform.sizeDelta = Vector2.one * radius * 2;
            thisTransform.localPosition = new Vector2(model.X + offsetX, model.Y + offsetY);
            thisTransform.localScale = Vector3.one;
        }

        public void SetClickableArea(ClickableBackground area) => parentClickArea = area;

        public int GetId() => model.Id;

        protected override void Awake()
        {
            base.Awake();
            wavesParticles = GetComponentInChildren<ParticleSystem>();
        }

#if UNITY_EDITOR
        // just for editor debug
        private void Start()
        {
            GetComponent<Image>().color = new Color32(255, 255, 255, 128);
            CreateDebugPoints(model.Radius);
        }
#endif

        public void PlayHintAnimation()
        {
            if (wavesParticles.isPlaying)
                wavesParticles.Stop();
            wavesParticles.Play();
        }

        private void OnClick()
        {
            GameManager.Instance.gameEventSystem.Publish(new DiffFoundCommand(model, this));
            gameObject.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                GetComponent<RectTransform>(),
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localCursor))
            {
                return;
            }
#if UNITY_EDITOR
            CreateDebugClickPoint(localCursor);
#endif
            if (localCursor.magnitude <= radius)
                OnClick();
            else
                parentClickArea.OnPointerDown(eventData);
        }
    }
}