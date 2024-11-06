using UnityEngine;

namespace Postive.BehaviourTrees.Runtime.Nodes.Actions.BlackBoards
{
    public class MessagePassNode : ActionNode
    {
        public override string NodeCategory => "BlackBoard";
        [SerializeField] private string _key = "Key";
        [SerializeField] private string _message = "Message";
        protected override BTState OnRun(BlackBoard blackBoard)
        {
            //Debug.Log($"MessagePassNode : {_key} : {_message}");
            blackBoard?.Set(_key, _message);
            return BTState.SUCCESS;
        }

        public override BTNode Clone()
        {
            var clone = (MessagePassNode) base.Clone();
            clone._key = _key;
            clone._message = _message;
            return clone;
        }
    }
}