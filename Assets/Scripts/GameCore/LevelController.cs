using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vevidi.FindDiff.Factories;
using Vevidi.FindDiff.GameMediator;
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
        private LevelsManager lManager;
        private LevelObjectsFactory loFactory;
        private Mediator gameEvents;
        private List<TouchableArea> touchableAreas;
        private Button backgroundClickArea;

        private int gameFieldWidth;
        private int gameFieldHeight;

        //public static LevelController Instance
        //{
        //    get
        //    {
        //        if (isShuttingDown)
        //        {
        //            Debug.LogWarning("Level controller is shutting down");
        //            return null;
        //        }
        //        return instance;
        //    }
        //}

        private void Awake()
        {
            lManager = GameManager.Instance.LevelsManager;
            levelInfo = lManager.GetLevelByID(lManager.GetSelectedLevel());
            gameEvents = GameManager.Instance.gameEventSystem;
            loFactory = GameManager.Instance.LevelObjFactory;
            gameEvents.Subscribe<DiffFoundCommand>(OnDiffFound);
            touchableAreas = new List<TouchableArea>();

            gameFieldWidth = (int)gameFieldRoot.rect.width;
            gameFieldHeight = (int)gameFieldRoot.rect.height;

            backgroundClickArea = backgroundImage.GetComponent<Button>();
            backgroundClickArea.onClick.AddListener(OnMissTap);
            Debug.Log("Level controller -> Loaded level: " + levelInfo);
        }

        private void OnMissTap()
        {
            SoundsManager.Instance.PlaySound(SoundsManager.eSoundType.Wrong);
        }

        private void OnDiffFound(DiffFoundCommand command)
        {
            RectTransform checkmark = loFactory.CreateCheckmark(gameFieldRoot);
            checkmark.localPosition = command.sender.transform.localPosition;

            SoundsManager.Instance.PlaySound(SoundsManager.eSoundType.Correct);

            touchableAreas.ForEach((ta) =>
            {
                if (ta.GetId() == command.foundedDifference.Id)
                    Destroy(ta.gameObject);
            });
            touchableAreas.RemoveAll((ta) => { return ta.GetId() == command.foundedDifference.Id; });

            int newDiffValue = levelInfo.LevelInfo.Differences.Count - touchableAreas.Count / 2;
            gameEvents.Publish(new UpdateLevelUiCommand(newDiffValue));
            if (touchableAreas.Count == 0)
            {
                lManager.EndLevel(levelInfo.Id);
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
                TouchableArea area = loFactory.CreateTouchableArea(gameFieldRoot, diff, -gameFieldWidth / 2, -gameFieldHeight / 2);
                touchableAreas.Add(area);
                area = loFactory.CreateTouchableArea(gameFieldRoot, diff, 0, -gameFieldHeight / 2);
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
            //base.OnDestroy();
            backgroundClickArea.onClick.RemoveListener(OnMissTap);
            gameEvents.DeleteSubscriber<DiffFoundCommand>(OnDiffFound);
        }
    }
}