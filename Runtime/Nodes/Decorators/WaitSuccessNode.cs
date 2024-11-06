﻿using UnityEngine;

namespace Postive.BehaviourTrees.Runtime.Nodes.Decorators
{
    public class WaitSuccessNode : DecoratorNode
    {
        public bool UseRandomBetweenTwoConstants => _useRandomBetweenTwoConstants;
        [SerializeField] private bool _useRandomBetweenTwoConstants = false;
        [SerializeField] private Vector2 _waitTimeRange = new Vector2(1f, 2f);
        [SerializeField] private float _waitTime = 1f;
        private float _timer = 0f;

        protected override void OnStart(BlackBoard blackBoard)
        {
            _timer = 0f;
            if (_useRandomBetweenTwoConstants) {
                _waitTime = Random.Range(_waitTimeRange.x, _waitTimeRange.y);
            }
        }
        protected override BTState OnRun(BlackBoard blackBoard)
        {
            _timer += Time.deltaTime;
            if (_timer >= _waitTime) {
                return BTState.FAILURE;
            }
            var result = Child.Run(blackBoard);
            //Debug.Log("WaitSuccessNode: " + result);
            return result == BTState.SUCCESS ? BTState.SUCCESS : BTState.RUNNING;
        }

        public override BTNode Clone()
        {
            var clone = (WaitSuccessNode) base.Clone();
            clone._waitTime = _waitTime;
            clone._useRandomBetweenTwoConstants = _useRandomBetweenTwoConstants;
            clone._waitTimeRange = _waitTimeRange;
            return clone;
        }
    }
}