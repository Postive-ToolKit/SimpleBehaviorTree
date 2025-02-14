using Postive.BehaviourTrees.Runtime.Data;
using UnityEngine;

namespace Postive.BehaviourTrees.Runtime.Nodes.Actions.BlackBoards
{
    public class MessageRemoveNode : ActionNode
    {
        public override string NodeCategory => "BlackBoard";
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