using Postive.BehaviourTrees.Runtime.Attributes;
using Postive.BehaviourTrees.Runtime.Data;
using Postive.BehaviourTrees.Runtime.Nodes;
using UnityEngine;

namespace Postive.BehaviourTrees.Runtime.Conditions.BlackBoards
{
    [BTInfo("BlackBoard", "Checks if a message received from the blackboard matches a specified value.")]
    public class MessageReceiveNode : ConditionNode
    {
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