using UnityEditor;
using UnityEngine;
using Vevidi.Experimental;

namespace Vevidi.CustomGameEditor
{
    [CustomEditor(typeof(ScrollView3D))]
    [CanEditMultipleObjects]
    public class ScrollView3DEditor : Editor
    {
        private SerializedProperty rightStartPos;
        private SerializedProperty leftStartPos;

        private ScrollView3D scrollView;

        void OnEnable()
        {
            rightStartPos = serializedObject.FindProperty("rightStartPos");
            leftStartPos = serializedObject.FindProperty("leftStartPos");

            scrollView = target as ScrollView3D;
            scrollView.ArrangeItems(scrollView.currSelectedItem, true);
        }

        public override void OnInspectorGUI()
        {
            scrollView = target as ScrollView3D;

            serializedObject.Update();
            EditorGUILayout.PropertyField(rightStartPos);
            EditorGUILayout.PropertyField(leftStartPos);

            scrollView.currSelectedItem = EditorGUILayout.IntField("Selected item", scrollView.currSelectedItem);

            if (Application.isEditor)
            {
                serializedObject.ApplyModifiedProperties();
                scrollView.ArrangeItems(scrollView.currSelectedItem, true);
            }
            if (GUI.changed)
            {
                EditorUtility.SetDirty(this);
            }
        }
    }
}