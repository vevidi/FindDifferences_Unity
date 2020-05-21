using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vevidi.FindDiff.GameLogic;
using Vevidi.FindDiff.GameMediator.Commands;
using Vevidi.FindDiff.GameUtils;
using eDragDirection = Vevidi.FindDiff.UI.SV3DSwipeControls.eDragDirection;

namespace Vevidi.FindDiff.UI
{
    public class ScrollView3D : MonoBehaviour, ISwipable
    {
        public enum eScrollSide
        {
            Right,
            Left
        }

#pragma warning disable 0649
        [SerializeField]
        private Vector3 rightStartPos = new Vector3(12.7f, -1.3f, 10.7f);
        [SerializeField]
        private Vector3 leftStartPos = new Vector3(-12.7f, -1.3f, 10.7f);
        [SerializeField]
        private float moveSpeed = 60f;
        [SerializeField]
        private float rotationSpeed = 270f;
        //[SerializeField]
        //private float distToCenterTreshold = 0.3f;
        [SerializeField]
        private List<Transform> items;
#pragma warning restore 0649

        private readonly Vector3 centerPos = new Vector3(0, -1.3f, 2.8f);
        private readonly Quaternion centerRotation = Quaternion.Euler(new Vector3(-270, -90, 90));
        private readonly Quaternion rightRotation = Quaternion.Euler(new Vector3(-270, -30, 90));
        private readonly Quaternion leftRotation = Quaternion.Euler(new Vector3(-270, 30, -90));
        private readonly Vector3 posOffsetRight = new Vector3(-1.15f, 0, -0.65f);
        private readonly Vector3 posOffsetLeft = new Vector3(-1.15f, 0, 0.65f);
        private readonly Vector3 maxPackSizeRight = new Vector3(-1.15f, 0, -0.65f) * 4;
        private readonly Vector3 maxPackSizeLeft = new Vector3(-1.15f, 0, 0.65f) * 4;
        private float distanceToCenter;

        public Transform itemPrefab;

        private Transform thisTransform;
        private bool isBlocked = false;
        private bool preAnimationNeeded = false;
        private List<ScrollView3DItem> itemsScripts;
        private Camera mainCamera;

        private int rightItemsCount = 0;
        private int leftItemsCount = 0;
        private int currItem;
        private int lastItem;
        private int nextItem;
        private int toCenterItem;
        private float currMoveValue = 0.5f;

        public int CurrentItem { get { return currItem; } set { currItem = value; } }

        private void Awake()
        {
            thisTransform = transform;
            items = new List<Transform>();
            itemsScripts = new List<ScrollView3DItem>();
            mainCamera = Camera.main;
            distanceToCenter = (centerPos - rightStartPos).magnitude;
        }

        public void Initialize(int itemsCount, int selectedLevel = 0)
        {
            for (int i = 0; i < itemsCount; ++i)
            {
                var currItem = Instantiate(itemPrefab, thisTransform);
                items.Add(currItem);
                itemsScripts.Add(currItem.GetComponent<ScrollView3DItem>());
            }
            lastItem = -1;
            ArrangeItems(selectedLevel, true);
        }

        public void Initialize(List<Transform> items, int selectedLevel = 0)
        {
            // just for debug
            int counter = 0;
            foreach (var item in items)
            {
                ++counter;
                item.SetParent(thisTransform);
                item.gameObject.name = "Item " + counter;
                itemsScripts.Add(item.GetComponent<ScrollView3DItem>());
            }
            this.items.AddRange(items);
            lastItem = -1;
            ArrangeItems(selectedLevel, true);
        }

        private void BlockItems(bool isBlocked)
        {
            foreach (var item in itemsScripts)
                item.BlockItem(isBlocked);
        }

        private void BlockItemsExceptCurrent()
        {
            BlockItems(true);
            itemsScripts[currItem].BlockItem(false);
        }

        private void ArrangeItemsRange(eScrollSide side)
        {
            switch (side)
            {
                case eScrollSide.Left:
                {
                    leftItemsCount = currItem;
                    if (leftItemsCount > 0)
                    {
                        Vector3 currOffset = maxPackSizeLeft / leftItemsCount;
                        if (currOffset.IsMore(posOffsetLeft))
                            currOffset = posOffsetLeft;
                        for (int i = leftItemsCount - 1; i >= 0; --i)
                        {
                            items[i].localPosition = leftStartPos + currOffset * (leftItemsCount - i);
                            items[i].localRotation = leftRotation;
                        }
                    }
                    break;
                }
                case eScrollSide.Right:
                {
                    rightItemsCount = items.Count - CurrentItem;
                    if (rightItemsCount > 0)
                    {
                        Vector3 currOffset = maxPackSizeRight / rightItemsCount;
                        if (currOffset.IsMore(posOffsetRight))
                            currOffset = posOffsetRight;
                        for (int i = CurrentItem + 1; i < items.Count; ++i)
                        {
                            items[i].localPosition = rightStartPos + currOffset * (CurrentItem + 1 - i);
                            items[i].localRotation = rightRotation;
                        }
                    }
                    break;
                }
            }
        }

        public void ArrangeItemsMoveFinger()
        {
            Utils.DebugLog("AIMF " + currItem + " " + nextItem);
            if (toCenterItem != -1)
            {
                currItem = toCenterItem;
                if (items[currItem].localPosition.x > 0)
                {
                    int secondItemId = currItem - 1;
                    secondItemId = CheckItemIdInBounds(secondItemId);
                    ArrangeItemsAnimated(currItem, secondItemId);
                }
                else if (items[currItem].localPosition.x < 0)
                {
                    int secondItemId = currItem + 1;
                    secondItemId = CheckItemIdInBounds(secondItemId);
                    ArrangeItemsAnimated(currItem, secondItemId);
                }
            }
            else
                ArrangeItemsAnimated(currItem, nextItem);
        }

        private void ArrangeItemsAnimated(int currItemID, int nextItemId)
        {
            Utils.DebugLog("AIA " + nextItem + " " + currItem);
            if (nextItemId < currItemID)
                SwipeLeftInternal(items[currItemID], items[nextItemId]);
            else if (nextItemId > currItemID)
                SwipeRightInternal(items[currItemID], items[nextItemId]);
            else
                BlockItemsExceptCurrent();
        }

        public void ArrangeItems(int selectedItemId = 0, bool withCenterUpdate = false, bool ignoreBlocked = false, bool force = false)
        {
            Utils.DebugLog("AI " + selectedItemId);
            if (items != null && selectedItemId >= 0 && items.Count > 0 && (!isBlocked || ignoreBlocked))
            {
                Utils.DebugLog("AII " + selectedItemId);
                if (lastItem != selectedItemId || force)
                {
                    Utils.DebugLog("Arrange items!", eLogType.Warning);

                    lastItem = selectedItemId;
                    currItem = selectedItemId;

#if UNITY_EDITOR
                    // prevent Unity editor errors
                    if (items[currItem] == null)
                        return;
#endif

                    if (withCenterUpdate)
                    {
                        items[currItem].localPosition = centerPos;
                        items[currItem].localRotation = centerRotation;
                    }
                    ArrangeItemsRange(eScrollSide.Left);
                    ArrangeItemsRange(eScrollSide.Right);

                    BlockItemsExceptCurrent();
                }
            }
        }

        private IEnumerator MoveTo(Transform objTrans, Vector3 position, Action callback = null, Action preCallback = null)
        {
            float step = moveSpeed * Time.fixedDeltaTime;
            while (Vector3.Distance(objTrans.localPosition, position) > 0.001f)
            {
                if (Vector3.Distance(objTrans.localPosition, position) < 1f && preAnimationNeeded)
                    preCallback?.Invoke();

                objTrans.localPosition = Vector3.MoveTowards(objTrans.localPosition, position, step);
                yield return new WaitForEndOfFrame();
            }
            callback?.Invoke();
        }

        private IEnumerator RotateTo(Transform objTrans, Quaternion rotation)
        {
            float step = rotationSpeed * Time.fixedDeltaTime;
            while (Quaternion.Angle(objTrans.localRotation, rotation) > 0.1f)
            {
                objTrans.localRotation = Quaternion.RotateTowards(objTrans.localRotation, rotation, step);
                yield return new WaitForEndOfFrame();
            }
        }

        private void AnimationEnded()
        {
            Utils.DebugLog("Anim ended", eLogType.Warning);

            isBlocked = false;
            GameManager.Instance.gameEventSystem.Publish(new NextLevelCommand(currItem));
            BlockItemsExceptCurrent();
        }

        private void PreAnimationEnded()
        {
            preAnimationNeeded = false;
            ArrangeItems(currItem, false, true);
        }

        public void SwipeRight()
        {
            if (currItem > 0 && !isBlocked)
            {
                SwipeRightInternal(items[currItem - 1], items[currItem]);
                --currItem;
            }
        }

        private void SwipeRightInternal(Transform firstObj, Transform secondObj)
        {
            Utils.DebugLog("Swipe right internal!" + (currItem > 0) + " " + !isBlocked);
            isBlocked = true;
            BlockItems(isBlocked);
            preAnimationNeeded = true;
            StartCoroutine(MoveTo(firstObj, centerPos));
            StartCoroutine(RotateTo(firstObj, centerRotation));
            StartCoroutine(MoveTo(secondObj, rightStartPos, AnimationEnded, PreAnimationEnded));
            StartCoroutine(RotateTo(secondObj, rightRotation));
        }

        public void SwipeLeft()
        {
            if (currItem < items.Count - 1 && !isBlocked)
            {
                SwipeLeftInternal(items[currItem + 1], items[currItem]);
                ++currItem;
            }
        }

        public void SwipeLeftInternal(Transform firstObj, Transform secondObj)
        {
            Utils.DebugLog("Swipe left internal! " + (currItem < items.Count - 1) + " " + !isBlocked);
            isBlocked = true;
            BlockItems(isBlocked);
            preAnimationNeeded = true;
            StartCoroutine(MoveTo(firstObj, centerPos));
            StartCoroutine(RotateTo(firstObj, centerRotation));
            StartCoroutine(MoveTo(secondObj, leftStartPos, AnimationEnded, PreAnimationEnded));
            StartCoroutine(RotateTo(secondObj, leftRotation));
        }

        public void ResetCurrentMoveValue() => currMoveValue = 0.5f;

        private void LerpPositionRotation(Transform objTrans, Vector3 startPos, Vector3 endPos, Quaternion startRot, Quaternion endRot, float value, float t = -1)
        {
            if (currMoveValue > 0)
            {
                objTrans.localPosition = Vector3.Lerp(startPos, endPos, value * 2f);
                objTrans.localRotation = Quaternion.Lerp(startRot, endRot, value * 2f);
            }
        }

        private int CheckItemIdInBounds(int itemId)
        {
            if (itemId > items.Count - 1)
                itemId = items.Count - 1;
            else if (itemId < 0)
                itemId = 0;
            return itemId;
        }

        private void CheckCurrMoveValCorrect()
        {
            if (currMoveValue > 1)
                currMoveValue = 1;
            else if (currMoveValue < 0)
                currMoveValue = 0;
        }

        public void DragStarted(eDragDirection direction)
        {
            if (direction == eDragDirection.Right)
                nextItem = currItem - 1;
            else if (direction == eDragDirection.Left)
                nextItem = currItem + 1;
            nextItem = CheckItemIdInBounds(nextItem);
        }

        private void CenterReached()
        {
            int rightItem = currItem + 1;
            int leftItem = currItem - 1;
            if (rightItem <= items.Count - 1)
            {
                items[rightItem].transform.localPosition = rightStartPos;
                items[rightItem].transform.localRotation = rightRotation;
            }
            if (leftItem >= 0)
            {
                items[leftItem].transform.localPosition = leftStartPos;
                items[leftItem].transform.localRotation = leftRotation;
            }
            ArrangeItems(currItem);
        }

        private (int curr, int toCenter) GetCenterItemID()
        {
            int resultCurr = -1;
            int resultToCenter = -1;
            float minDistCurr = 20f; //TODO: make constants
            float minDistCenter = 20f; //TODO: make constants
            for (int i = currItem - 2; i <= currItem + 2; ++i)
            {
                if (i < 0 || i > items.Count - 1)
                    continue;
                var item = itemsScripts[i];
                float dist = Mathf.Abs(item.transform.localPosition.x); // center x position == 0
                if (dist < 1f)
                {
                    if (dist <= minDistCurr)
                    {
                        minDistCurr = dist;
                        resultCurr = item.Id;
                    }
                }
                if (dist <= 5f)
                {
                    if (dist <= minDistCenter && resultCurr != item.Id)
                    {
                        minDistCenter = dist;
                        resultToCenter = item.Id;
                    }
                }
            }
            return (resultCurr, resultToCenter);
        }

        public void MoveLeftPercent(float value) // value in [0,1] range
        {
            if (currItem < items.Count - 1)
            {
                currMoveValue += value;
                CheckCurrMoveValCorrect();

                (int currCenterItemId, int toCenterItemId) = GetCenterItemID();
                toCenterItem = toCenterItemId != -1 ? toCenterItemId : toCenterItem;

                if (currMoveValue > 0.5f)
                    LerpPositionRotation(items[currItem], centerPos, rightStartPos, centerRotation, rightRotation, currMoveValue - 0.5f);
                else if (currMoveValue < 0.5f)
                    LerpPositionRotation(items[currItem], leftStartPos, centerPos, leftRotation, centerRotation, currMoveValue);
                if (currMoveValue < 0.5f)
                    LerpPositionRotation(items[nextItem], centerPos, rightStartPos, centerRotation, rightRotation, currMoveValue, currMoveValue);
                else if (currMoveValue - 0.5f > 0)
                    LerpPositionRotation(items[nextItem], leftStartPos, centerPos, leftRotation, centerRotation, currMoveValue - 0.5f, currMoveValue);

                if (currCenterItemId != -1
                    && (nextItem == currCenterItemId || currItem == currCenterItemId))
                {
                    if (nextItem == currCenterItemId)
                    {
                        ResetCurrentMoveValue();
                        currItem = currCenterItemId;
                        nextItem = currItem + 1;
                        nextItem = CheckItemIdInBounds(nextItem);
                        Utils.DebugLog("!!! Left: Current item changed 1 " + currItem + " " + nextItem);
                    }
                    else if (currItem == currCenterItemId)
                    {
                        if (items[currCenterItemId].localPosition.x > 0f && nextItem != currItem - 1)
                        {
                            nextItem = currItem - 1;
                            nextItem = CheckItemIdInBounds(nextItem);
                            CenterReached();
                        }
                        else if (items[currCenterItemId].localPosition.x < 0f && nextItem != currItem + 1)
                        {
                            nextItem = currItem + 1;
                            nextItem = CheckItemIdInBounds(nextItem);
                            CenterReached();
                        }
                        else
                            ArrangeItems(currItem);

                        Utils.DebugLog("!!! Left: Current item changed 2 " + currItem + " " + nextItem);
                    }
                }
            }
        }

        public void MoveRightPercent(float value) // value in [0,1] range
        {
            if (currItem > 0)
            {
                currMoveValue += value;
                CheckCurrMoveValCorrect();

                (int currCenterItemId, int toCenterItemId) = GetCenterItemID();
                toCenterItem = toCenterItemId != -1 ? toCenterItemId : toCenterItem;

                if (currMoveValue > 0.5f)
                    LerpPositionRotation(items[currItem], centerPos, rightStartPos, centerRotation, rightRotation, currMoveValue - 0.5f);
                else if (currMoveValue < 0.5f)
                    LerpPositionRotation(items[currItem], leftStartPos, centerPos, leftRotation, centerRotation, currMoveValue);
                if (currMoveValue < 0.5f)
                    LerpPositionRotation(items[nextItem], centerPos, rightStartPos, centerRotation, rightRotation, currMoveValue, currMoveValue);
                else if (currMoveValue - 0.5f > 0)
                    LerpPositionRotation(items[nextItem], leftStartPos, centerPos, leftRotation, centerRotation, currMoveValue - 0.5f, currMoveValue);

                if (currCenterItemId != -1 && (nextItem == currCenterItemId || currItem == currCenterItemId))
                {
                    if (nextItem == currCenterItemId)
                    {
                        currItem = currCenterItemId;
                        nextItem = currItem - 1;
                        ResetCurrentMoveValue();
                        nextItem = CheckItemIdInBounds(nextItem);
                        Utils.DebugLog("!!! Right: Current item changed 1 " + currItem + " " + nextItem);

                    }
                    else if (currItem == currCenterItemId)
                    {
                        if (items[currCenterItemId].localPosition.x > 0f && nextItem != currItem - 1)
                        {
                            nextItem = currItem - 1;
                            nextItem = CheckItemIdInBounds(nextItem);
                            CenterReached();
                        }
                        else if (items[currCenterItemId].localPosition.x < 0f && nextItem != currItem + 1)
                        {
                            nextItem = currItem + 1;
                            nextItem = CheckItemIdInBounds(nextItem);
                            CenterReached();
                        }
                        else
                            ArrangeItems(currItem);

                        Utils.DebugLog("!!! Right: Current item changed 2 " + currItem + " " + nextItem);
                    }
                }
            }
        }

        public void DoClick(Vector2 clickPoint)
        {
            if (Physics.Raycast(mainCamera.ScreenPointToRay(clickPoint), out RaycastHit hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("3D_UI_Item")))
            {
                ScrollView3DItem levelButton = hit.collider.gameObject.GetComponent<ScrollView3DItem>();
                levelButton.OnClick();
            }
        }
    }
}