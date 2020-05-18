using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vevidi.FindDiff.GameUtils;

namespace Vevidi.Experimental
{
    public class ScrollView3D : MonoBehaviour, ISwipable
    {
        [SerializeField]
        private Vector3 rightStartPos = new Vector3(12.7f, -1.3f, 10.7f);
        [SerializeField]
        private Vector3 leftStartPos = new Vector3(-12.7f, -1.3f, 10.7f);
        [SerializeField]
        private float moveSpeed = 60f;
        [SerializeField]
        private float rotationSpeed = 270f;
        [SerializeField]
        private List<Transform> items;

        private readonly Vector3 centerPos = new Vector3(0, -1.3f, 2.8f);
        private readonly Quaternion centerRotation = Quaternion.Euler(new Vector3(-270, -90, 90));
        private readonly Quaternion rightRotation = Quaternion.Euler(new Vector3(-270, -30, 90));
        private readonly Quaternion leftRotation = Quaternion.Euler(new Vector3(-270, 30, -90));
        private readonly Vector3 posOffsetRight = new Vector3(-1.15f, 0, -0.65f);
        private readonly Vector3 posOffsetLeft = new Vector3(-1.15f, 0, 0.65f);
        private readonly Vector3 maxPackSizeRight = new Vector3(-1.15f, 0, -0.65f) * 4;
        private readonly Vector3 maxPackSizeLeft = new Vector3(-1.15f, 0, 0.65f) * 4;

        public Transform itemPrefab;

        private Transform thisTransform;
        private bool isBlocked = false;
        private bool preAnimationNeeded = false;
        private List<ScrollView3DItem> itemsScripts;

        private int rightItemsCount = 0;
        private int leftItemsCount = 0;

        public int CurrentItem { get; set; }

        private void Awake()
        {
            thisTransform = transform;
            items = new List<Transform>();
            itemsScripts = new List<ScrollView3DItem>();

            // TEST
            //Initialize(20);
        }

        public void Initialize(int itemsCount)
        {
            for (int i = 0; i < itemsCount; ++i)
            {
                var currItem = Instantiate(itemPrefab, thisTransform);
                items.Add(currItem);
                itemsScripts.Add(currItem.GetComponent<ScrollView3DItem>());
            }
            ArrangeItems(0, true);
        }

        public void Initialize(List<Transform> items)
        {
            foreach (var item in items)
            {
                item.SetParent(thisTransform);
                itemsScripts.Add(item.GetComponent<ScrollView3DItem>());
            }
            this.items.AddRange(items);

            ArrangeItems(0, true);
        }

        private void BlockItems(bool isBlocked)
        {
            foreach (var item in itemsScripts)
                item.BlockItem(isBlocked);
        }

        private void BlockItemsExceptCurrent()
        {
            BlockItems(true);
            itemsScripts[CurrentItem].BlockItem(false);
        }

        public void ArrangeItems(int selectedItemId = 0, bool withCenterUpdate = false, bool ignoreBlocked = false)
        {
            if (items!=null && items.Count > 0 && (!isBlocked||ignoreBlocked))
            {
                CurrentItem = selectedItemId;

#if UNITY_EDITOR
                // prevent Unity editor errors
                if (items[CurrentItem] == null)
                    return;
#endif

                if (withCenterUpdate)
                {
                    items[CurrentItem].localPosition = centerPos;
                    items[CurrentItem].localRotation = centerRotation;
                }

                leftItemsCount = CurrentItem - 1;
                if (leftItemsCount > 0)
                {
                    Vector3 currOffset = maxPackSizeLeft / leftItemsCount;
                    if (currOffset.IsMore(posOffsetLeft))
                        currOffset = posOffsetLeft;
                    for (int i = leftItemsCount; i >= 0; --i)
                    {
                        items[i].localPosition = leftStartPos + currOffset * (leftItemsCount - i);
                        items[i].localRotation = leftRotation;
                    }
                }

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
                BlockItemsExceptCurrent();
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
            isBlocked = false;
            //BlockItems(isBlocked);
            BlockItemsExceptCurrent();
        }

        private void PreAnimationEnded()
        {
            preAnimationNeeded = false;
            ArrangeItems(CurrentItem, false, true);
        }

        public void SwipeRight()
        {
            if (CurrentItem > 0 && !isBlocked)
            {
                isBlocked = true;
                BlockItems(isBlocked);
                preAnimationNeeded = true;
                StartCoroutine(MoveTo(items[CurrentItem - 1], centerPos));
                StartCoroutine(RotateTo(items[CurrentItem - 1], centerRotation));
                StartCoroutine(MoveTo(items[CurrentItem], rightStartPos, AnimationEnded, PreAnimationEnded));
                StartCoroutine(RotateTo(items[CurrentItem], rightRotation));
                --CurrentItem;
            }
        }

        public void SwipeLeft()
        {
            if (CurrentItem < items.Count - 1 && !isBlocked)
            {
                isBlocked = true;
                BlockItems(isBlocked);
                preAnimationNeeded = true;
                StartCoroutine(MoveTo(items[CurrentItem + 1], centerPos));
                StartCoroutine(RotateTo(items[CurrentItem + 1], centerRotation));
                StartCoroutine(MoveTo(items[CurrentItem], leftStartPos, AnimationEnded, PreAnimationEnded));
                StartCoroutine(RotateTo(items[CurrentItem], leftRotation));
                ++CurrentItem;
            }
        }
    }
}