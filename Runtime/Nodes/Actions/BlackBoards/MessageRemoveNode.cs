using Postive.BehaviourTrees.Runtime.Attributes;
using Postive.BehaviourTrees.Runtime.Data;
using UnityEngine;

namespace Postive.BehaviourTrees.Runtime.Nodes.Actions.BlackBoards
{
    [BTInfo("BlackBoard", "Removes a key-value pair from the blackboard.")]
    public class MessageRemoveNode : ActionNode
    {
        [SerializeField] private string _key;
        protected override BTState OnRun(BlackBoard blackBoard)
        {
            if (blackBoard == null) return BTState.FAILURE;
            blackBoard.Remove(_key);
            return BTState.SUCCESS;
        }
        public override BTNode Clone()
        {
            var clone = (MessageRemoveNode) base.Clone();
            clone._key = _key;
            return clone;
        }
    }
}