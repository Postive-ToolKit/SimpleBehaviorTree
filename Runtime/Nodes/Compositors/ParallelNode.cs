namespace Postive.BehaviourTrees.Runtime.Nodes.Compositors
{
    public class ParallelNode : CompositeNode
    {
        private int _currentEndCount = 0;
        protected override BTState OnActiveEvaluate(BlackBoard blackBoard)
        {
            for (int i = 0; i < Children.Count; i++) {
                if (!Children[i].CanReEvaluate) continue;
                var result = Children[i].Evaluate(blackBoard);
                if (result == BTState.FAILURE) return BTState.FAILURE;
            }
            return BTState.SUCCESS;
        }
        protected override BTState RunChildren(BlackBoard blackBoard)
        {
            for (int i = 0; i < Children.Count; i++) {
                var state = Children[i].Run(blackBoard);
                switch (state) {
                    case BTState.FAILURE:case BTState.RUNNING:
                        return state;
                    case BTState.SUCCESS:
                        _currentEndCount++;
                        break;
                }
            }
            return _currentEndCount == Children.Count ? BTState.SUCCESS : BTState.RUNNING;
        }
    }
}