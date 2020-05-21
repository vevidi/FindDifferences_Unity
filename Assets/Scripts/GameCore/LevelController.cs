using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vevidi.FindDiff.Factories;
using Vevidi.FindDiff.GameMediator;
using Vevidi.FindDiff.GameMediator.Commands;
using Vevidi.FindDiff.GameModel;
using Vevidi.FindDiff.GameUtils;
using Vevidi.FindDiff.NetworkModel;
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
        private List<DifferenceInfoModel> differences;

        private int gameFieldWidth;
        private int gameFieldHeight;

        private const int maxLives = 5;
        private int liveCount = maxLives;

        private void UpdateSize()
        {
            if (Utils.GetAspectRatio() > 1.8f)
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
            gameEvents.Subscribe<ShowHintCommand>(ShowHint);

            touchableAreas = new List<TouchableArea>();
            foundedCheckmarks = new List<RectTransform>();
            gameFieldWidth = (int)gameFieldRoot.rect.width;
            gameFieldHeight = (int)gameFieldRoot.rect.height;
            gameFieldImageTransform = backgroundImage.rectTransform;

            backgroundClickArea = backgroundImage.GetComponent<ClickableBackground>();
            backgroundClickArea.OnBackgroundClick += OnMissTap;
            UpdateSize();
        }

        private void RemoveOneLive()
        {
            --liveCount;
            gameEvents.Publish(new UpdateLivesCountCommand(liveCount, maxLives));
            if (liveCount == 0)
                LoseGame();
        }

        private void OnMissTap(PointerEventData data)
        {
            SoundsManager.Instance.PlaySound(SoundsManager.eSoundType.Wrong);
            Vector2 localCursor;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(gameFieldImageTransform, data.position, data.pressEventCamera, out localCursor))
                return;
            MissTapCheckmark checkmark = levelObjFactory.CreateMissTapCheckmark(gameFieldRoot);
            checkmark.transform.localPosition = localCursor;
            RemoveOneLive();
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
            differences.RemoveAll((diff) => diff.Id == command.FoundedDifference.Id);

            int diffCount = levelInfo.Differences.Count;
            int newDiffValue = diffCount - differences.Count;
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

        private void CreateClickArea(DifferenceInfoModel model, int offsetX = 0, int offsetY = 0)
        {
            TouchableArea area = levelObjFactory.CreateTouchableArea(gameFieldRoot, model, offsetX, offsetY);
            area.SetClickableArea(backgroundClickArea);
            touchableAreas.Add(area);
        }

        private void InitLevel()
        {
            backgroundImage.overrideSprite = Utils.GetSpriteFromTex2D(levelInfo.LevelImage);
            var diffs = levelInfo.Differences;
            differences = diffs.Clone() as List<DifferenceInfoModel>;
            foreach (var diff in differences)
            {
                // right item
                CreateClickArea(diff, -gameFieldWidth / 2);
                // left item
                CreateClickArea(diff);
            }
            liveCount = 5;
            gameEvents.Publish(new UpdateLivesCountCommand(liveCount, maxLives));
            gameEvents.Publish(new UpdateDiffCountCommand(0, differences.Count));
        }

        private void ClearLevel()
        {
            touchableAreas.ForEach((ta) => Destroy(ta.gameObject));
            touchableAreas.Clear();
            foundedCheckmarks.ForEach((fc) => Destroy(fc.gameObject));
            foundedCheckmarks.Clear();
            differences.Clear();
        }

        private void GoToNextLevel(NextLevelCommand command)
        {
            levelManager.SelectLevel(command.LevelID);
            levelInfo = levelManager.GetLevelByID(command.LevelID);// + 1);
            ClearLevel();
            InitLevel();
        }

        private void RestartLevel(RestartLevelCommand command)
        {
            ClearLevel();
            InitLevel();
        }

        private void ShowHint(ShowHintCommand command)
        {
            RemoveOneLive();
            DifferenceInfoModel diffModel = differences[Random.Range(0, differences.Count)];
            int targetDiffId = diffModel.Id;
            List<TouchableArea> targets = touchableAreas.FindAll((ta) => ta.GetId() == targetDiffId);
            foreach (TouchableArea ta in targets)
                ta.PlayHintAnimation();
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
            gameEvents.DeleteSubscriber<ShowHintCommand>(ShowHint);
        }
    }
}