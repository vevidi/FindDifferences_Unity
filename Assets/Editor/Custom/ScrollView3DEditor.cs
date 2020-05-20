using UnityEditor;
using UnityEngine;
using Vevidi.FindDiff.UI;

namespace Vevidi.CustomGameEditor
{
    [CustomEditor(typeof(ScrollView3D))]
    [CanEditMultipleObjects]
    public class ScrollView3DEditor : Editor
    {
        private ScrollView3D scrollView;

        void OnEnable()
        {
            scrollView = target as ScrollView3D;
            scrollView.ArrangeItems(scrollView.CurrentItem, true);
        }

        public override void OnInspectorGUI()
        {
            scrollView = target as ScrollView3D;
            scrollView.CurrentItem = EditorGUILayout.IntField("Selected item", scrollView.CurrentItem);
            DrawDefaultInspector();

            if (Application.isEditor)
            {
                scrollView.ArrangeItems(scrollView.CurrentItem, true);
            }
            if (GUI.changed)
            {
                EditorUtility.SetDirty(this);
            }
        }
    }
}