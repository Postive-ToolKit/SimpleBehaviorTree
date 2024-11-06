using Postive.BehaviourTrees.Runtime.Nodes;
using UnityEngine;

namespace Postive.BehaviourTrees.Runtime.Conditions.BlackBoards
{
    public class MessageReceiveNode : ConditionNode
    {
        public override string NodeCategory => "BlackBoard";
        [SerializeField] private string _key = "Key";
        [SerializeField] private string _message = "Message";
        protected override BTState GetConditionState(BlackBoard blackBoard) {
            var message = blackBoard.Get<string>(_key);
            if (message == null) return BTState.FAILURE;
            return message.Equals(_message) ? BTState.SUCCESS : BTState.FAILURE;
        }

        public override BTNode Clone()
        {
            var clone = (MessageReceiveNode) base.Clone();
            clone._key = _key;
            clone._message = _message;
            return clone;
        }
    }
}