using Postive.BehaviourTrees.Runtime.Data;

namespace Postive.BehaviourTrees.Runtime.Nodes.Compositors
{
    public class SequencerNode : CompositeNode
    {
        private int _current = 0;
        protected override BTState OnActiveEvaluate(BlackBoard blackBoard)
        {
            for (int i = 0; i < _current; i++) {
                if (!Children[i].CanReEvaluate) continue;
                if (Children[i].Evaluate(blackBoard) == BTState.FAILURE) return BTState.FAILURE;
            }
            return BTState.SUCCESS;
        }
        protected override BTState OnLowerPriorityEvaluate(BlackBoard blackBoard)
        {
            for (int i = 0; i < _current + 1; i++) {
                if (!Children[i].CanReEvaluate) continue;
                if (!Children[i].EnableHijack) continue;
                // if (Children[i] is not CompositeNode compositeChild) continue;
                // if (compositeChild.AbortType != ConditionalAbortType.LOWER_PRIORITY && compositeChild.AbortType != ConditionalAbortType.BOTH) continue;
                if (Children[i].Evaluate(blackBoard) == BTState.FAILURE) return BTState.FAILURE;
            }
            return BTState.SUCCESS;
        }
        protected override void OnStart(BlackBoard blackBoard) {
            _current = 0;
        }
        protected override BTState RunChildren(BlackBoard blackBoard)
        {
            var child = Children[_current];
            switch (child.Run(blackBoard)) {
                case BTState.FAILURE:
                    _current++;
                    return BTState.FAILURE;
                case BTState.RUNNING:
                    return BTState.RUNNING;
                case BTState.SUCCESS:
                    _current++;
                    break;
            }
            return _current >= Children.Count ? BTState.SUCCESS : BTState.RUNNING;
        }
    }
}