using Postive.BehaviourTrees.Runtime.Nodes.Decorators;
using UnityEditor;
using UnityEngine;

namespace Postive.BehaviourTrees.Editor.CustomEditors.NodeEditors
{
    [CustomEditor(typeof(BlackBoardNode))]
    public class BlackBoardNodeEditor : UnityEditor.Editor {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var node = (BlackBoardNode) target;
            GUILayout.Box("", new GUIStyle() {fixedHeight = 1, stretchWidth = true, normal = {background = Texture2D.whiteTexture}});
            EditorGUI.BeginDisabledGroup(true);
            foreach (var key in node.BlackBoard.Data.Keys) {
                var value = node.BlackBoard.Get<object>(key);
                EditorGUILayout.TextField(key, value.ToString());
            }
            EditorGUI.EndDisabledGroup();
        }
    }
}