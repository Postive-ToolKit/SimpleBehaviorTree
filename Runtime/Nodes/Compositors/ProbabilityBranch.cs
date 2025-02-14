﻿using System.Collections.Generic;
using Postive.BehaviourTrees.Runtime.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Postive.BehaviourTrees.Runtime.Nodes.Compositors
{
    public class ProbabilityBranch : CompositeNode
    {
        [SerializeField] private List<float> _probabilities = new List<float>();
        private int _currentBranch = 0;
        protected override BTState OnActiveEvaluate(BlackBoard blackBoard)
        {
            if (!Children[_currentBranch].CanReEvaluate) return BTState.SUCCESS;
            return Children[_currentBranch].Evaluate(blackBoard);
        }
        protected override void OnStart(BlackBoard blackBoard)
        {
            for (int i = 0; i < Children.Count; i++) {
                Children[i].IsStarted = false;
            }
            
            var totalProbability = 0f;
            for (int i = 0; i < _probabilities.Count; i++) {
                totalProbability += _probabilities[i];
            }
            var random = Random.Range(0f, totalProbability);
            var currentProbability = 0f;
            for (int i = 0; i < _probabilities.Count; i++) {
                currentProbability += _probabilities[i];
                if (random <= currentProbability) {
                    _currentBranch = i;
                    break;
                }
            }
        }
        protected override BTState RunChildren(BlackBoard blackBoard) {
            return Children[_currentBranch].Run(blackBoard);
        }
        public override BTNode Clone()
        {
            var clone = (ProbabilityBranch)base.Clone();
            clone._probabilities = new List<float>(_probabilities);
            return clone;
        }

        protected override void CheckIntegrity()
        {
            base.CheckIntegrity();
            for (int i = _probabilities.Count; i < Children.Count; i++) {
                _probabilities.Add(1f);
            }
            //remove over children
            if (_probabilities.Count > Children.Count) {
                _probabilities.RemoveRange(Children.Count, _probabilities.Count - Children.Count);
            }
        }
    }
}