using System;
using Postive.BehaviourTrees.Runtime.Data;
using Postive.BehaviourTrees.Runtime.Nodes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Postive.BehaviourTrees.Runtime.Components
{
    public class BTRunnerComponent : MonoBehaviour
    {
        // private const float BEHAVIOUR_TREE_UPDATE_INTERVAL = 0.1f;
        public event Action OnTreeStopped;
        [Serializable]
        private class BlackBoardPassObject {
            public string Key;
            public Object Value;
        }
        public BehaviourTree Tree => _behaviourTree;
        public BlackBoard BlackBoard => _blackBoard;
        public bool IsPaused => _isPaused;
        public bool IsStopped => _isStopped;
        [SerializeField] protected bool _startOnAwake = true;
        [SerializeField] protected BehaviourTree _behaviourTree;
        [SerializeField] private BlackBoardPassObject[] _initialObjects;
        [HideInInspector][SerializeField] BlackBoard _blackBoard = new BlackBoard();
        private bool _isPaused = false;
        private bool _isStopped = false;
        private BehaviourTree _nextTree;
        
        //private float _time = 0;
        
        private void Awake() {
            if (_behaviourTree == null) {
                Debug.LogWarning("Behaviour tree is null.");
                return;
            }
            if (!_startOnAwake) return;
            foreach (var data in _initialObjects) {
                _blackBoard.Set(data.Key, data.Value);
            }
            SetTree(_behaviourTree);
        }
        private void Update() {
            if (_behaviourTree == null) return;
            if (!_behaviourTree.IsCloned) return;
            if (_isPaused) return;
            var lastState = _behaviourTree.BTUpdate(_blackBoard);
            if (lastState == BTState.FAILURE || lastState == BTState.SUCCESS) {
                Stop();
            }
        }
        private void LateUpdate() {
            if (_nextTree == null) return;
            _behaviourTree?.Stop();
            _behaviourTree = _nextTree;
            _nextTree = null;
        }
        public void BTSendMessage(string message) {
            _blackBoard.Set("Message", message);
            //_behaviourTree.BlackBoard.Set("Message", message);
        }
        public void BTSetData(string key, object value) {
            _blackBoard.Set(key, value);
            //_behaviourTree.BlackBoard.Set(key, value);
        }
        public void BTRemoveData(string key) {
            //_behaviourTree.BlackBoard.Remove(key);
            _blackBoard.Remove(key);
        }
        public void SetTree(BehaviourTree behaviourTree) {
            if (behaviourTree == null) return;
            _nextTree = behaviourTree.IsCloned ? behaviourTree : behaviourTree.Clone(gameObject);
            // _nextTree.BlackBoard.Clear();
            // foreach (var data in _initialObjects) {
            //     _nextTree.BlackBoard.Set(data.Key, data.Value);
            // }
            _nextTree.OnAwake();
        }
        public void Stop() {
            _behaviourTree.Stop();
            _isPaused = true;
            _isStopped = true;
            OnTreeStopped?.Invoke();
        }
        public void Play() {
            _behaviourTree.Stop();
            _isPaused = false;
            _isStopped = false;
        }
        public void Pause() {
            _isPaused = true;
        }
        public void Resume() {
            _isPaused = false;
        }
        protected virtual void OnValidate()
        {
            foreach (var objectSet in _initialObjects) {
                if (objectSet.Value == null) {
                    Debug.LogWarning($"Object value is null for key: {objectSet.Key}");
                    continue;
                }
                objectSet.Key = objectSet.Value != null && string.IsNullOrEmpty(objectSet.Key) ? objectSet.Value.name : objectSet.Key;
            }
        }

        private void OnDrawGizmos() {
            if (_behaviourTree == null) return;
            if (_behaviourTree.Owner == null) return;
            _behaviourTree.OnDrawGizmos();
        }
    }
}