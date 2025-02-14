using Postive.BehaviourTrees.Runtime.Data;
using Postive.BehaviourTrees.Runtime.Nodes;

namespace Postive.BehaviourTrees.Runtime.Conditions
{
    public abstract class ConditionNode : BTNode {
        public override bool CanReEvaluate => true;
        public override BTState Evaluate(BlackBoard blackBoard) => GetConditionState(blackBoard);

        protected override BTState OnRun(BlackBoard blackBoard) => GetConditionState(blackBoard);
        protected abstract BTState GetConditionState(BlackBoard blackBoard);
    }
}