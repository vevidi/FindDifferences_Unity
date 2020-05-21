using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vevidi.FindDiff.GameMediator;
using Vevidi.FindDiff.GameMediator.Commands;
using Vevidi.FindDiff.GameUtils;
using Vevidi.FindDiff.NetworkModel;

namespace Vevidi.FindDiff.GameLogic
{
    public class TouchableArea : MonoBehaviour, /*ICanvasRaycastFilter,*/ IPointerClickHandler
    {
#pragma warning disable 0649
        [SerializeField]
        private RectTransform debugPoint;
#pragma warning restore 0649

        private DifferenceInfoModel model;
        private RectTransform thisTransform;
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

        public void SetClickableArea(ClickableBackground area)
        {
            parentClickArea = area;
            Utils.DebugLog(parentClickArea.gameObject.name, eLogType.Warning);
        }

        public int GetId()
        {
            return model.Id;
        }

        private void Awake()
        {
            thisTransform = GetComponent<RectTransform>();
            wavesParticles = GetComponentInChildren<ParticleSystem>();
        }

#if UNITY_EDITOR
        // just for editor debug
        private void Start()
        {
            GetComponent<Image>().color = new Color32(255, 255, 255, 128);
            CreateDebugPoints();
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

        // ------------ DEBUG functionality -----------
        private void CreateDebugPoints()
        {
            float delta = 2 * Mathf.PI / 50;
            for (int i = 0; i < 50; ++i)
            {
                float x = radius * Mathf.Cos(delta * i);
                float y = radius * Mathf.Sin(delta * i);
                RectTransform rTrans = Instantiate(debugPoint, thisTransform);
                rTrans.localPosition = new Vector3(x, y, 0);
            }
        }

        private void CreateDebugClickPoint(Vector2 clickPosition)
        {
            var spT = Instantiate(debugPoint, thisTransform);
            spT.localPosition = clickPosition;
            spT.GetComponent<Image>().color = Color.green;
            spT.sizeDelta = new Vector2(5, 5);
        }
        // --------------------------------------------
    }
}