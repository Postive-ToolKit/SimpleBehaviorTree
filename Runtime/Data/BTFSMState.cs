using System;
using Postive.BehaviourTrees.Runtime;
using UnityEngine;

namespace Postive.Runtime.BehaviourTrees.Data
{
    [Serializable]
    public class BTFSMState
    {
        public string ID => _id;
        [SerializeField] private string _id;
        [SerializeField] private BehaviourTree _tree;
        private BehaviourTree _behaviourTree = null;
        public BehaviourTree GetTree(GameObject owner) {
            if (_behaviourTree == null) {
                _behaviourTree = _tree.Clone(owner);
            }
            return _behaviourTree;
        }
    }
}