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
#pragma warning restore 0649

        private LevelDescriptionModel levelInfo;
        private LevelsManager levelManager;
        private LevelObjectsFactory levelObjFactory;
        private Mediator gameEvents;

        private List<TouchableArea> touchableAreas;
        private List<RectTransform> foundedCheckmarks;
        private ClickableBackground backgroundClickArea;
        private RectTransform gameFieldImageTransform;

        private int gameFieldWidth;
        private int gameFieldHeight;

        private int liveCount = 5;

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
            // cache some vars
            levelManager = GameManager.Instance.LevelsManager;
            levelInfo = levelManager.GetLevelByID(levelManager.GetSelectedLevel());
            gameEvents = GameManager.Instance.gameEventSystem;
            levelObjFactory = GameManager.Instance.LevelObjFactory;
            //subscribe listeners
            gameEvents.Subscribe<DiffFoundCommand>(OnDiffFound);
            gameEvents.Subscribe<NextLevelCommand>(GoToNextLevel);
            gameEvents.Subscribe<RestartLevelCommand>(RestartLevel);

            touchableAreas = new List<TouchableArea>();
            foundedCheckmarks = new List<RectTransform>();
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
            --liveCount;
            gameEvents.Publish(new UpdateLivesCountCommand(liveCount));
            if (liveCount == 0)
                LoseGame();
        }

        private void OnDiffFound(DiffFoundCommand command)
        {
            SoundsManager.Instance.PlaySound(SoundsManager.eSoundType.Correct);

            touchableAreas.ForEach((ta) =>
            {
                if (ta.GetId() == command.FoundedDifference.Id)
                {
                    RectTransform checkmark = levelObjFactory.CreateCheckmark(gameFieldRoot);
                    checkmark.localPosition = ta.transform.localPosition;
                    foundedCheckmarks.Add(checkmark);
                    Destroy(ta.gameObject);
                }
            });
            touchableAreas.RemoveAll((ta) => { return ta.GetId() == command.FoundedDifference.Id; });

            int diffCount = levelInfo.Differences.Count;
            int newDiffValue = levelInfo.Differences.Count - touchableAreas.Count / 2;
            gameEvents.Publish(new UpdateDiffCountCommand(newDiffValue, diffCount));
            if (touchableAreas.Count == 0)
                WinGame();
        }

        private void WinGame()
        {
            levelManager.EndLevel(levelInfo.Id);
            SoundsManager.Instance.PlaySound(SoundsManager.eSoundType.Win);
            UI_WindowsManager.Instance.ShowWindow(new UI_WindowConfig(UI_WindowsManager.eWindowType.Win));
        }

        private void LoseGame()
        {
            SoundsManager.Instance.PlaySound(SoundsManager.eSoundType.Lose);
            UI_WindowsManager.Instance.ShowWindow(new UI_WindowConfig(UI_WindowsManager.eWindowType.Lose));

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
            liveCount = 5;
            gameEvents.Publish(new UpdateLivesCountCommand(liveCount));
            gameEvents.Publish(new UpdateDiffCountCommand(diffs.Count, diffs.Count));
        }

        private void ClearLevel()
        {
            touchableAreas.ForEach((ta) => Destroy(ta.gameObject));
            touchableAreas.Clear();
            foundedCheckmarks.ForEach((fc) => Destroy(fc.gameObject));
            foundedCheckmarks.Clear();
        }

        private void GoToNextLevel(NextLevelCommand command)
        {
            int selectedLevelID = levelManager.GetSelectedLevel();
            levelManager.SelectLevel(selectedLevelID + 1);
            levelInfo = levelManager.GetLevelByID(selectedLevelID + 1);
            ClearLevel();
            InitLevel();
        }

        private void RestartLevel(RestartLevelCommand command)
        {
            ClearLevel();
            InitLevel();
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
            // unsubscribe all
            backgroundClickArea.OnBackgroundClick -= OnMissTap;
            gameEvents.DeleteSubscriber<DiffFoundCommand>(OnDiffFound);
            gameEvents.DeleteSubscriber<NextLevelCommand>(GoToNextLevel);
            gameEvents.DeleteSubscriber<RestartLevelCommand>(RestartLevel);
        }
    }
}