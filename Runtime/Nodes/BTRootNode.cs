using Postive.BehaviourTrees.Runtime.Data;
using UnityEngine;

namespace Postive.BehaviourTrees.Runtime.Nodes
{
    public class BTRootNode : BTNode
    {
        [HideInInspector] public BTNode Child;
        protected override BTState OnRun(BlackBoard blackBoard) {
            return Child.Run(blackBoard);
        }
        protected override void OnStop(BlackBoard blackBoard) {
            Child.BTStop(blackBoard);
        }
        //향후 수정해야 할지도
        public override BTState Evaluate(BlackBoard blackBoard) {
            return Child.Evaluate(blackBoard);
        }
        public override BTNode Clone()
        {
            var clone = (BTRootNode)base.Clone();
            clone.Child = Child.Clone();
            return clone;
        }
    }
}