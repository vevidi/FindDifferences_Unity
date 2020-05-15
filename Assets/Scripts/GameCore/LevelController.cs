using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vevidi.FindDiff.Factories;
using Vevidi.FindDiff.GameMediator;
using Vevidi.FindDiff.GameMediator.Commands;
using Vevidi.FindDiff.GameModel;
using Vevidi.FindDiff.GameUtils;
using Vevidi.FindDiff.UI;

namespace Vevidi.FindDiff.GameLogic
{
    public class LevelController : SingletonBase<LevelController>
    {
#pragma warning disable 0649
        [SerializeField]
        private Image backgroundImage;
        [SerializeField]
        private RectTransform gameFieldRoot;
        //TODO: think abou deattaching UI script from LevelController (m.b. init through mediator?)
        [SerializeField]
        private UI_LevelManager lvManager;
#pragma warning restore 0649

        private LevelDescriptionModel levelInfo;
        private LevelsManager levelManager;
        private LevelObjectsFactory levelObjFactory;
        private Mediator gameEvents;
        private List<TouchableArea> touchableAreas;
        private ClickableBackground backgroundClickArea;
        private RectTransform gameFieldImageTransform;

        private int gameFieldWidth;
        private int gameFieldHeight;

        private void UpdateSize()
        {
            if(Utils.GetAspectRatio()>1.8f)
            {
                gameFieldImageTransform.localScale = Vector3.one * 0.85f;
                gameFieldRoot.localScale = Vector3.one * 0.85f;
            }
        }

        private void Awake()
        {
            levelManager = GameManager.Instance.LevelsManager;
            levelInfo = levelManager.GetLevelByID(levelManager.GetSelectedLevel());
            gameEvents = GameManager.Instance.gameEventSystem;
            levelObjFactory = GameManager.Instance.LevelObjFactory;
            gameEvents.Subscribe<DiffFoundCommand>(OnDiffFound);
            touchableAreas = new List<TouchableArea>();

            gameFieldWidth = (int)gameFieldRoot.rect.width;
            gameFieldHeight = (int)gameFieldRoot.rect.height;
            gameFieldImageTransform = backgroundImage.rectTransform;

            backgroundClickArea = backgroundImage.GetComponent<ClickableBackground>();
            backgroundClickArea.OnBackgroundClick += OnMissTap;
            UpdateSize();

            Debug.Log("Level controller -> Loaded level: " + levelInfo);
        }

        private void OnMissTap(PointerEventData data)
        {
            SoundsManager.Instance.PlaySound(SoundsManager.eSoundType.Wrong);
            Vector2 localCursor;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(gameFieldImageTransform, data.position, data.pressEventCamera, out localCursor))
                return;
            MissTapCheckmark checkmark = levelObjFactory.CreateMissTapCheckmark(gameFieldRoot);
            checkmark.transform.localPosition = localCursor;
        }

        private void OnDiffFound(DiffFoundCommand command)
        {
            SoundsManager.Instance.PlaySound(SoundsManager.eSoundType.Correct);

            touchableAreas.ForEach((ta) =>
            {
                if (ta.GetId() == command.foundedDifference.Id)
                {
                    RectTransform checkmark = levelObjFactory.CreateCheckmark(gameFieldRoot);
                    checkmark.localPosition = ta.transform.localPosition;
                    Destroy(ta.gameObject);
                }
            });
            touchableAreas.RemoveAll((ta) => { return ta.GetId() == command.foundedDifference.Id; });

            int newDiffValue = levelInfo.LevelInfo.Differences.Count - touchableAreas.Count / 2;
            gameEvents.Publish(new UpdateLevelUiCommand(newDiffValue));
            if (touchableAreas.Count == 0)
            {
                levelManager.EndLevel(levelInfo.Id);
                SoundsManager.Instance.PlaySound(SoundsManager.eSoundType.Win);
                UI_WindowsManager.Instance.ShowWindow(new UI_WindowConfig(UI_WindowsManager.eWindowType.GameEnded));
            }
        }

        private void InitLevel()
        {
            backgroundImage.overrideSprite = Utils.GetSpriteFromTex2D(levelInfo.LevelImage);
            var diffs = levelInfo.Differences;
            foreach (var diff in diffs)
            {
                TouchableArea area = levelObjFactory.CreateTouchableArea(gameFieldRoot, diff, -gameFieldWidth / 2, 0);
                touchableAreas.Add(area);
                area = levelObjFactory.CreateTouchableArea(gameFieldRoot, diff);
                touchableAreas.Add(area);
            }
            lvManager.Init(diffs.Count);
        }

        private void Start()
        {
            InitLevel();
        }

        public LevelDescriptionModel GetLevelInfo()
        {
            return levelInfo;
        }

        protected override void OnDestroy()
        {
            backgroundClickArea.OnBackgroundClick -= OnMissTap;
            gameEvents.DeleteSubscriber<DiffFoundCommand>(OnDiffFound);
        }
    }
}