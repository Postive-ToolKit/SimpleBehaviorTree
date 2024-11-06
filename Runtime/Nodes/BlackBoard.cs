using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Postive.BehaviourTrees.Runtime.Nodes
{
    [Serializable]
    public class BlackBoard
    {
        [HideInInspector]public BlackBoard Parent;

        public Dictionary<string, object> Data {
            get {
                var result = new Dictionary<string, object>();
                foreach (var key in _data.Keys) {
                    result.Add(key, _data[key]);
                }
                //add parent data
                if (Parent == null) return result;
                //add parent divider
                foreach (var key in Parent.Data.Keys.Where(key => !result.ContainsKey(key))) {
                    result.Add(key, Parent.Data[key]);
                }
                return result;
            }
        }
        private readonly Dictionary<string,object> _data = new Dictionary<string, object>();
        public BlackBoard() { }
        public BlackBoard(BlackBoard parent) {
            Parent = parent;
        }
        public object Get(string key) {
            if (_data.ContainsKey(key)) {
                return _data[key];
            }
            //Find from parent when not found
            if (Parent != null) {
                return Parent.Get(key);
            }
            return null;
        }
        public T Get<T>(string key) {
            object o = Get(key);
            if (o == null) return default;
            T data = (T)Get(key);
            if (data != null) {
                return data;
            }
            return default;
        }
        public bool Contains(string key) {
            if (_data.ContainsKey(key)) {
                return true;
            }
            return Parent != null && Parent.Contains(key);
        }
        public void Set(string key, object value) {
            //Debug.Log("Set : " + key + " : " + value);
            if (_data.ContainsKey(key)) {
                _data[key] = value;
                return;
            }
            _data.Add(key, value);
        }

        public void Remove(string key) {
            if(!_data.ContainsKey(key)) return;
            _data.Remove(key);
        }

        public void Clear() {
            _data.Clear();
        }
    }
}