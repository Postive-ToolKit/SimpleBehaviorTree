using Postive.BehaviourTrees.Runtime.Nodes.Decorators;
using UnityEditor;

namespace Postive.BehaviourTrees.Editor.CustomEditors.NodeEditors
{
    [CustomEditor(typeof(WaitSuccessNode))]
    public class WaitSuccessNodeEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI() {
            var waitSuccessNode = (WaitSuccessNode) target;
            //show script field
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_useRandomBetweenTwoConstants"));
            if (waitSuccessNode.UseRandomBetweenTwoConstants) {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_waitTimeRange"));
                serializedObject.ApplyModifiedProperties();
                return;
            }
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_waitTime"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}