using Postive.BehaviourTrees.Runtime.Data;

namespace Postive.BehaviourTrees.Runtime.Nodes.Actions
{
    public abstract class ActionNode : BTNode {
        public override bool CanReEvaluate => false;
        public override BTState Evaluate(BlackBoard blackBoard) => BTState.SUCCESS;
    }
}