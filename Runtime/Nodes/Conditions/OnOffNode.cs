using Postive.BehaviourTrees.Runtime.Data;
using Postive.BehaviourTrees.Runtime.Nodes;
using UnityEngine;

namespace Postive.BehaviourTrees.Runtime.Conditions
{
    public class OnOffNode : ConditionNode {
        [SerializeField] private bool _on = true;
        protected override BTState GetConditionState(BlackBoard blackBoard) {
            return _on ? BTState.SUCCESS : BTState.FAILURE;
        }
        public override BTNode Clone() {
            var clone = (OnOffNode)base.Clone();
            clone._on = _on;
            return clone;
        }
    }
}