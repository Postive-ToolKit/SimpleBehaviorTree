using System.Collections.Generic;
using UnityEngine;
namespace Postive.BehaviourTrees.Runtime.Nodes.Compositors
{
    public abstract class CompositeNode : BTNode
    {
        public override bool CanReEvaluate => Children.Exists(child => child.CanReEvaluate);

        public override bool EnableHijack {
            get {
                switch (AbortType) {
                    case ConditionalAbortType.NONE: case ConditionalAbortType.SELF:default:
                        return false;
                    case ConditionalAbortType.LOWER_PRIORITY: case ConditionalAbortType.BOTH:
                        return true;
                }
            }
        }
        [HideInInspector] public List<BTNode> Children =new List<BTNode>();
        public ConditionalAbortType AbortType = ConditionalAbortType.NONE;
        private BTState[] _childEvaluationStates;
        public override BTNode Clone() {
            var clone = (CompositeNode)base.Clone();
            clone.Children = Application.isPlaying ? Children.ConvertAll(child => child.Clone()) : new List<BTNode>();
            return clone;
        }
        protected override BTState OnRun(BlackBoard blackBoard)
        {
            //self abort
            if (AbortType is ConditionalAbortType.BOTH or ConditionalAbortType.SELF) {
                var evaluate = Evaluate(blackBoard);
                if (evaluate == BTState.FAILURE) return BTState.FAILURE;
            }
            if (OnLowerPriorityEvaluate(blackBoard) == BTState.FAILURE) {
                return BTState.FAILURE;
            }
            return RunChildren(blackBoard);
        }
        public override BTState Evaluate(BlackBoard blackBoard) {
            return IsStarted ? OnActiveEvaluate(blackBoard) : OnDeActiveEvaluate(blackBoard);
        }
        /// <summary>
        /// Evaluate the node when it is not active
        /// </summary>
        /// <param name="blackBoard"> BlackBoard </param>
        /// <returns> BTState </returns>
        protected virtual BTState OnDeActiveEvaluate(BlackBoard blackBoard)
        {
            for (int i = 0; i < Children.Count; i++) {
                if (!Children[i].CanReEvaluate) continue;
                if (Children[i].Evaluate(blackBoard) == BTState.FAILURE) {
                    return BTState.FAILURE;
                }
            }
            return BTState.SUCCESS;
        }
        protected abstract BTState OnActiveEvaluate(BlackBoard blackBoard);

        /// <summary>
        /// Handle the low priority abort
        /// </summary>
        /// <param name="blackBoard"> BlackBoard </param>
        protected virtual BTState OnLowerPriorityEvaluate(BlackBoard blackBoard) => BTState.SUCCESS;

        /// <summary>
        /// Handle the children nodes
        /// </summary>
        /// <param name="blackBoard"> BlackBoard </param>
        /// <returns> BTState </returns>
        protected abstract BTState RunChildren(BlackBoard blackBoard);


        protected override void OnStop(BlackBoard blackBoard) 
        {
            for (int i = 0; i < Children.Count; i++) {
                Children[i].BTStop(blackBoard);
            }
        }
        
        protected override void CheckIntegrity()
        {
            //remove null children
            Children.RemoveAll(child => child == null);
            Children.Sort((a, b) => {
                var aPos = a.Position;
                var bPos = b.Position;
                return aPos.x.CompareTo(bPos.x);
            });
        }
    }
}