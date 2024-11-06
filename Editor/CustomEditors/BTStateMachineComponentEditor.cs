using Postive.BehaviourTrees.Runtime.Components;
using UnityEditor;

namespace Postive.BehaviourTrees.Editor.CustomEditors
{
    [CustomEditor(typeof(BTStateMachineComponent))]
    public class BTStateMachineComponentEditor : BTRunnerComponentEditor {
        protected override void SingleBehaviorTreePanel(){
            //base.SingleBlackBoardPanel();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_initialState"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_states"));
        }
    }
}