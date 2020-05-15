using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vevidi.Experimental
{
    public class ScrollView3D : MonoBehaviour
    {
        private readonly Vector3 centerPos = new Vector3(0, -1.3f, 2.8f);
        private readonly Vector3 rightStartPos = new Vector3(14, -1.3f, 11.45f);
        private readonly Vector3 leftStartPos = new Vector3(-14, -1.3f, 11.45f);
        private readonly Quaternion centerRotation = Quaternion.Euler(new Vector3(-270, -90, 90));
        private readonly Quaternion rightRotation = Quaternion.Euler(new Vector3(-270, -30, 90));
        private readonly Quaternion leftRotation = Quaternion.Euler(new Vector3(-270, 30, -90));
        private readonly Vector3 posOffsetRight = new Vector3(-1.15f, 0, -0.65f);
        private readonly Vector3 posOffsetLeft = new Vector3(-1.15f, 0, 0.65f);

        public Transform itemPrefab;

        private Transform thisTransform;
        private List<Transform> items;
        private bool isBlocked = false;

        private int currSelectedItem = 0;

        private void Awake()
        {
            thisTransform = transform;
            items = new List<Transform>();

            // TEST
            Initialize(5);
        }

        public void Initialize(int itemsCount)
        {
            for (int i = 0; i < itemsCount; ++i)
            {
                items.Add(Instantiate(itemPrefab, thisTransform));
            }
            ArrangeItems();
        }

        public void Initialize(List<Transform> items)
        {
            foreach (var item in items)
                item.SetParent(thisTransform);
            this.items.AddRange(items);
            ArrangeItems();
        }

        private void ArrangeItems(int selectedItemId = 4)
        {
            if (items.Count > 0)
            {
                //TODO: refactor this
                currSelectedItem = selectedItemId;
                int idCenter = currSelectedItem;

                items[idCenter].localPosition = centerPos;
                for (int i = idCenter - 1; i >= 0; --i)
                {
                    items[i].localPosition = leftStartPos + posOffsetLeft * (idCenter - 1 - i);
                    items[i].localRotation = leftRotation;
                }
                for (int i = idCenter + 1; i < items.Count; ++i)
                {
                    items[i].localPosition = rightStartPos + posOffsetRight * (idCenter + 1 - i);
                    items[i].localRotation = rightRotation;
                }
            }
        }

        private IEnumerator MoveTo(Transform objTrans, Vector3 position, Action callback = null, Action preCallback = null, float speed = 30f)
        {
            float step = speed * Time.deltaTime;
            while(Vector3.Distance(objTrans.localPosition,position)>0.001f)
            {
                if (Vector3.Distance(objTrans.localPosition, position) < 1f && preAnimationNeeded)
                    preCallback?.Invoke();

                objTrans.localPosition = Vector3.MoveTowards(objTrans.localPosition, position, step);
                yield return null;
            }
            callback?.Invoke();
        }

        private IEnumerator RotateTo(Transform objTrans, Quaternion rotation, float speed = 135f)
        {
            float step = speed * Time.deltaTime;
            while (Quaternion.Angle(objTrans.localRotation, rotation) > 0.1f)
            {
                objTrans.localRotation = Quaternion.RotateTowards(objTrans.localRotation, rotation, step);
                yield return null;
            }
        }

        private void AnimationEnded()
        {
            isBlocked = false;
        }

        bool preAnimationNeeded = false;
        private void PreAnimationEnded()
        {
            Debug.LogWarning("Pre ended");
            preAnimationNeeded = false;
            ArrangeItems(currSelectedItem);
        }

        public IEnumerator SwipeRight()
        {
            if (currSelectedItem > 0 && !isBlocked)
            {
                isBlocked = true;
                preAnimationNeeded = true;
                StartCoroutine(MoveTo(items[currSelectedItem - 1], centerPos));
                StartCoroutine(RotateTo(items[currSelectedItem - 1], centerRotation));
                StartCoroutine(MoveTo(items[currSelectedItem], rightStartPos, AnimationEnded, PreAnimationEnded));
                StartCoroutine(RotateTo(items[currSelectedItem], rightRotation));
                --currSelectedItem;
            }
            yield return null;
        }

        public IEnumerator SwipeLeft()
        {
            if (currSelectedItem < items.Count-1 && !isBlocked)
            {
                isBlocked = true;
                preAnimationNeeded = true;
                StartCoroutine(MoveTo(items[currSelectedItem + 1], centerPos));
                StartCoroutine(RotateTo(items[currSelectedItem + 1], centerRotation));
                StartCoroutine(MoveTo(items[currSelectedItem], leftStartPos, AnimationEnded, PreAnimationEnded));
                StartCoroutine(RotateTo(items[currSelectedItem], leftRotation));
                ++currSelectedItem;
            }
            yield return null;
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(250, 100, 100, 100), "Right"))
            {
                StartCoroutine(SwipeRight());
            }
            if (GUI.Button(new Rect(100, 100, 100, 100), "Left"))
            {
                StartCoroutine(SwipeLeft());
            }
        }
    }
}