using Postive.BehaviourTrees.Runtime.Data;

namespace Postive.BehaviourTrees.Runtime.Nodes.Compositors
{
    public class SelectorNode : CompositeNode
    {
        private int _current = 0;
        protected override BTState OnActiveEvaluate(BlackBoard blackBoard)
        {
            for(int i = 0; i < Children.Count; i++) {
                if (!Children[i].CanReEvaluate) {
                    return BTState.SUCCESS;
                }
                if (Children[i].Evaluate(blackBoard) == BTState.SUCCESS) return BTState.SUCCESS;
            }
            return BTState.FAILURE;
        }

        protected override BTState OnDeActiveEvaluate(BlackBoard blackBoard) {
            for(int i = 0; i < Children.Count; i++) {
                if (!Children[i].CanReEvaluate) {
                    return BTState.SUCCESS;
                }
                if (Children[i].Evaluate(blackBoard) == BTState.SUCCESS) return BTState.SUCCESS;
            }
            return BTState.FAILURE;
        }

        protected override BTState OnLowerPriorityEvaluate(BlackBoard blackBoard)
        {
            HandleLowerPriority(blackBoard);
            return BTState.SUCCESS;
        }

        private void HandleLowerPriority(BlackBoard blackBoard) {
            var currentChild = Children[_current];
            for (int i = 0; i < _current; i++) {
                if (!Children[i].CanReEvaluate) continue;
                if (!Children[i].EnableHijack) continue;
                // if (Children[i] is not CompositeNode compositeChild) continue;
                // if (compositeChild.AbortType != ConditionalAbortType.LOWER_PRIORITY && compositeChild.AbortType != ConditionalAbortType.BOTH) continue;
                if (Children[i].Evaluate(blackBoard) == BTState.FAILURE) continue;
                currentChild.BTStop(blackBoard);
                _current = i;
                return;
            }
        }
        protected override void OnStart(BlackBoard blackBoard) {
            _current = 0;
        }
        protected override BTState RunChildren(BlackBoard blackBoard)
        {
            var child = Children[_current];
            switch (child.Run(blackBoard)) {
                case BTState.RUNNING:
                    return BTState.RUNNING;
                case BTState.SUCCESS:
                    _current++;
                    return BTState.SUCCESS;
                case BTState.FAILURE:
                    _current++;
                    break;
            }
            return _current >= Children.Count ? BTState.FAILURE : BTState.RUNNING;
        }
    }
}