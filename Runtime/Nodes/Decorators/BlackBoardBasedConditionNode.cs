using UnityEngine;

namespace Postive.BehaviourTrees.Runtime.Nodes.Decorators
{
    public class BlackBoardBasedConditionNode : DecoratorNode {
        public override string NodeCategory => "BlackBoard";
        [SerializeField] private string _key;
        protected override BTState OnRun(BlackBoard blackBoard)
        {
            if (!blackBoard.Contains(_key)) {
                //Debug.LogError("Key not found in blackboard with key");
                return BTState.FAILURE;
            }
            object result = blackBoard.Get(_key);
            if (result == null) {
                return BTState.FAILURE;
            }
            return Child.Run(blackBoard);
        }
        public override BTNode Clone()
        {
            var clone = (BlackBoardBasedConditionNode) base.Clone();
            clone._key = _key;
            return clone;
        }
    }
}