using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugableArea : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    protected RectTransform debugPoint;
#pragma warning restore 0649
    protected RectTransform thisTransform;
    private List<RectTransform> debugPoints;

    protected virtual void Awake()
    {
        thisTransform = GetComponent<RectTransform>();
        debugPoints = new List<RectTransform>();
    }

    // ------------ DEBUG functionality -----------
    protected void CreateDebugPoints(float radius)
    {
        for (int i = 0; i < 50; ++i)
        {
            RectTransform rTrans = Instantiate(debugPoint, thisTransform);
            debugPoints.Add(rTrans);
        }
        UpdateDebugPointsRadius(radius);
    }

    public void UpdateDebugPointsRadius(float radius)
    {
        if (debugPoints != null && debugPoints.Count >= 50)
        {
            float delta = 2 * Mathf.PI / 50;
            for (int i = 0; i < 50; ++i)
            {
                float x = radius * Mathf.Cos(delta * i);
                float y = radius * Mathf.Sin(delta * i);
                debugPoints[i].localPosition = new Vector3(x, y, 0);
            }
        }
    }

    protected void CreateDebugClickPoint(Vector2 clickPosition)
    {
        var spT = Instantiate(debugPoint, thisTransform);
        spT.localPosition = clickPosition;
        spT.GetComponent<Image>().color = Color.green;
        spT.sizeDelta = new Vector2(5, 5);
    }
    // --------------------------------------------
}