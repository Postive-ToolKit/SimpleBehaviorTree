using System;
using Postive.Runtime.BehaviourTrees.Data;
using UnityEngine;

namespace Postive.BehaviourTrees.Runtime.Components
{
    [RequireComponent(typeof(BTRunnerComponent))]
    public class BTStateMachineComponent : BTRunnerComponent {
        [SerializeField] private string _initialState;
        [SerializeField] private BTFSMState[] _states;
        private void Start() {
            SetState(_initialState);
        }
        public void SetState(string id) {
            if (string.IsNullOrEmpty(id)) return;
            var state = Array.Find(_states, s => s.ID == id);
            SetTree(state.GetTree(gameObject));
        }
    }
}