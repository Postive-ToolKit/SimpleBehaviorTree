using System;
using System.Collections.Generic;
using Postive.BehaviourTrees.Runtime.Data;
using Postive.BehaviourTrees.Runtime.Nodes;
using Postive.BehaviourTrees.Runtime.Nodes.Compositors;
using Postive.BehaviourTrees.Runtime.Nodes.Decorators;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace Postive.BehaviourTrees.Runtime
{
    [CreateAssetMenu(fileName = "BehaviourTree", menuName = "AI/BehaviourTree")]
    public class BehaviourTree : ScriptableObject
    {
        public bool IsCloned => _isCloned;
        public GameObject Owner { get; private set; } = null;
        public BTNode Root;
        public BTState TreeState = BTState.RUNNING;
        public List<BTNode> Nodes = new List<BTNode>();
        private BlackBoard _lastBlackBoard = new BlackBoard();
        private bool _isCloned = false;
        public BTState BTUpdate(BlackBoard blackBoard) {
            _lastBlackBoard = blackBoard;
            if (Root.State == BTState.RUNNING || Root.State == BTState.NOT_ENTERED) {
                TreeState = Root.Run(_lastBlackBoard);
            }
            return TreeState;
        }
#if UNITY_EDITOR
        public void SetEditorOwner(GameObject owner) {
            Owner = owner;
        }
        public BTNode CreateNode(System.Type type)
        {
            var node = CreateInstance(type) as BTNode;
            if (node == null) return null;
            node.Tree = this;
            node.name = type.Name;
            node.GUID = System.Guid.NewGuid().ToString();
            Undo.RecordObject(this, "Behaviour Tree(Create Node)");
            Nodes.Add(node);
            if (!Application.isPlaying) {
                AssetDatabase.AddObjectToAsset(node, this);
                Undo.RegisterCreatedObjectUndo(node, "Behaviour Tree(Create Node)");
            }

            return node;
        }
        public void AddNode(BTNode node)
        {
            Undo.RecordObject(this, "Behaviour Tree(Copy Node)");
            Nodes.Add(node);
            if (!Application.isPlaying) {
                AssetDatabase.AddObjectToAsset(node, this);
                Undo.RegisterCreatedObjectUndo(node, "Behaviour Tree(Copy Node)");
            }
        }
        public void DeleteNode(BTNode node)
        {
            Undo.RecordObject(this, "Behaviour Tree(Delete Node)");
            Nodes.Remove(node);
            foreach (var n in Nodes) {
                n.OnValidate();
            }
            //AssetDatabase.RemoveObjectFromAsset(node);
            Undo.DestroyObjectImmediate(node);
        }
        public void AddChild(BTNode parent, BTNode child)
        {
            BTRootNode root = parent as BTRootNode;
            if (root != null) {
                Undo.RecordObject(root, "Behaviour Tree(Add Child)");
                root.Child = child;
                root.OnValidate();
                EditorUtility.SetDirty(root);
            }
            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator != null) {
                Undo.RecordObject(decorator, "Behaviour Tree(Add Child)");
                decorator.Child = child;
                decorator.OnValidate();
                EditorUtility.SetDirty(decorator);
            }
            CompositeNode composite = parent as CompositeNode;
            if (composite != null) {
                Undo.RecordObject(composite, "Behaviour Tree(Add Child)");
                composite.Children.Add(child);
                composite.OnValidate();
                EditorUtility.SetDirty(composite);
            }
            parent.OnValidate();
        }
        public void RemoveChild(BTNode parent, BTNode child)
        {
            BTRootNode root = parent as BTRootNode;
            if (root != null) {
                Undo.RecordObject(root, "Behaviour Tree(Remove Child)");
                root.Child = null;
                root.OnValidate();
                EditorUtility.SetDirty(root);
            }
            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator != null) {
                Undo.RecordObject(decorator, "Behaviour Tree(Remove Child)");
                decorator.Child = null;
                decorator.OnValidate();
                EditorUtility.SetDirty(decorator);
            }
            CompositeNode composite = parent as CompositeNode;
            if (composite != null) {
                Undo.RecordObject(composite, "Behaviour Tree(Remove Child)");
                composite.Children.Remove(child);
                composite.OnValidate();
                EditorUtility.SetDirty(composite);
            }
            parent.OnValidate();
        }
        public void NotifyTreeChanged() {
            Undo.RecordObject(this, "Tree Changed");
            OnValidate();
            EditorUtility.SetDirty(this);
        }
#endif
        /// <summary>
        /// Get all children of a node
        /// </summary>
        /// <param name="parent"> The parent node. </param>
        /// <returns> The children of the parent node. </returns>
        public List<BTNode> GetChildren(BTNode parent)
        {
            List<BTNode> children = new List<BTNode>();
            BTRootNode root = parent as BTRootNode;
            if (root != null && root.Child != null) {
                children.Add(root.Child);
            }
            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator != null && decorator.Child != null) {
                children.Add(decorator.Child);
            }
            CompositeNode composite = parent as CompositeNode;
            if (composite != null) {
                children.AddRange(composite.Children);
                return children;
            }
            return children;
        }
        /// <summary>
        /// Clone tree with new owner
        /// </summary>
        /// <param name="owner"> The owner of the new tree. </param>
        /// <returns> The cloned tree. </returns>
        public BehaviourTree Clone(GameObject owner)
        {
            var tree = Instantiate(this);
            tree.Owner = owner;
            tree.Root = tree.Root.Clone();
            tree.Nodes.Clear();
            tree._isCloned = true;
            Traverse(tree.Root, node => {
                node.Tree = tree;
                tree.Nodes.Add(node);
            });
            return tree;
        }
        public void OnValidate() {
            Nodes.RemoveAll(child => child == null);
            foreach (var node in Nodes) {
                node.OnValidate();
            }
        }
        /// <summary>
        /// Traverse the tree
        /// </summary>
        /// <param name="node"> The node to start from. </param>
        /// <param name="visitor"> The visitor function. </param>
        private void Traverse(BTNode node, Action<BTNode> visitor)
        {
            if (!node) return;
            visitor.Invoke(node);
            var children = GetChildren(node);
            foreach (var child in children) {
                Traverse(child, visitor);
            }
        }
        /// <summary>
        /// Awake the tree
        /// </summary>
        public void OnAwake() {
            foreach (var node in Nodes) {
                node.OnAwake();
            }
        }

        public void Stop() {
            Root.BTStop(_lastBlackBoard);
            for (int i = 0; i < Nodes.Count; i++) {
                Nodes[i].State = BTState.NOT_ENTERED;
            }
        }
        public void OnDrawGizmos() {
            for(int i = 0; i < Nodes.Count; i++) {
                Nodes[i].OnDrawGizmos();
            }
        }
    }
}