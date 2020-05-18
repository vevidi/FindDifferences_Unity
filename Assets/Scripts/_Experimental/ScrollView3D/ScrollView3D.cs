﻿using System;
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

        private int rightItemsCount = 0;
        private int leftItemsCount = 0;
        private int currentItem = 0;

        public int CurrentItem { get => currentItem; set => currentItem = value; }

        private void Awake()
        {
            thisTransform = transform;
            items = new List<Transform>();

            // TEST
            Initialize(20);
        }

        public void Initialize(int itemsCount)
        {
            for (int i = 0; i < itemsCount; ++i)
            {
                items.Add(Instantiate(itemPrefab, thisTransform));
            }
            ArrangeItems(0, true);
        }

        public void Initialize(List<Transform> items)
        {
            foreach (var item in items)
                item.SetParent(thisTransform);
            this.items.AddRange(items);
            ArrangeItems(0, true);
        }

        public void ArrangeItems(int selectedItemId = 0, bool withCenterUpdate = false, bool ignoreBlocked = false)
        {
            if (items!=null && items.Count > 0 && (!isBlocked||ignoreBlocked))
            {
                currentItem = selectedItemId;

                if (withCenterUpdate)
                {
                    items[currentItem].localPosition = centerPos;
                    items[currentItem].localRotation = centerRotation;
                }

                leftItemsCount = currentItem - 1;
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

                rightItemsCount = items.Count - currentItem;
                if (rightItemsCount > 0)
                {
                    Vector3 currOffset = maxPackSizeRight / rightItemsCount;
                    if (currOffset.IsMore(posOffsetRight))
                        currOffset = posOffsetRight;
                    for (int i = currentItem + 1; i < items.Count; ++i)
                    {
                        items[i].localPosition = rightStartPos + currOffset * (currentItem + 1 - i);
                        items[i].localRotation = rightRotation;
                    }
                }
            }
        }

        private IEnumerator MoveTo(Transform objTrans, Vector3 position, Action callback = null, Action preCallback = null)
        {
            float step = moveSpeed * Time.fixedDeltaTime;
            while (Vector3.Distance(objTrans.localPosition, position) > 0.001f)
            {
                if (Vector3.Distance(objTrans.localPosition, position) < 0.5f && preAnimationNeeded)
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
        }

        private void PreAnimationEnded()
        {
            preAnimationNeeded = false;
            ArrangeItems(currentItem, false, true);
        }

        public void SwipeRight()
        {
            if (currentItem > 0 && !isBlocked)
            {
                isBlocked = true;
                preAnimationNeeded = true;
                StartCoroutine(MoveTo(items[currentItem - 1], centerPos));
                StartCoroutine(RotateTo(items[currentItem - 1], centerRotation));
                StartCoroutine(MoveTo(items[currentItem], rightStartPos, AnimationEnded, PreAnimationEnded));
                StartCoroutine(RotateTo(items[currentItem], rightRotation));
                --currentItem;
            }
        }

        public void SwipeLeft()
        {
            if (currentItem < items.Count - 1 && !isBlocked)
            {
                isBlocked = true;
                preAnimationNeeded = true;
                StartCoroutine(MoveTo(items[currentItem + 1], centerPos));
                StartCoroutine(RotateTo(items[currentItem + 1], centerRotation));
                StartCoroutine(MoveTo(items[currentItem], leftStartPos, AnimationEnded, PreAnimationEnded));
                StartCoroutine(RotateTo(items[currentItem], leftRotation));
                ++currentItem;
            }
        }
    }
}