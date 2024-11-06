using System;
using System.Text;
using Postive.BehaviourTrees.Runtime;
using Postive.BehaviourTrees.Runtime.Components;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;
namespace Postive.BehaviourTrees.Editor
{
    public class BehaviourTreeEditor : EditorWindow
    {
        private BehaviourTreeView _behaviourTreeView;
        private BTInspectorView _inspectorView;
        [MenuItem("Window//Behaviour Tree Editor")]
        public static void OpenWindow()
        {
            BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
            wnd.titleContent = new GUIContent("Behaviour Tree Editor");
        }
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line) {
            if (Selection.activeObject is BehaviourTree) {
                OpenWindow();
                return true;
            }
            return false;
        }
        //add select from hierarchy event
        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Instantiate UXML
            var visualTree = Resources.Load<VisualTreeAsset>("BT_Editor_Layout");
            visualTree.CloneTree(root);
        
            _behaviourTreeView = root.Q<BehaviourTreeView>();
            _inspectorView = root.Q<BTInspectorView>();
            
            root.RegisterCallback<KeyDownEvent>(evt => {
                if (evt.keyCode == KeyCode.S && evt.ctrlKey) {
                    Save();
                }
            });
        
            _behaviourTreeView.OnNodeSelectionChanged += OnNodeSelectionChange;
            OnSelectionChange();
        }
        private void Save()
        {
            StringBuilder log = new StringBuilder();
            log.AppendLine("Save Behaviour Tree : " + _behaviourTreeView.Tree.name);
            log.AppendLine("Path : " + AssetDatabase.GetAssetPath(_behaviourTreeView.Tree));
            Debug.Log(log.ToString());
            //save project
            AssetDatabase.SaveAssets();
        }

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged  -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged  += OnPlayModeStateChanged;
        }
        private void OnDisable() {
            EditorApplication.playModeStateChanged  -= OnPlayModeStateChanged;
        }
        private void OnPlayModeStateChanged(PlayModeStateChange modeState)
        {
            switch (modeState) {
                case PlayModeStateChange.EnteredEditMode:
                    OnSelectionChange();
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    OnSelectionChange();
                    break;
                default:
                    break;
            }
        }
        private BehaviourTree _lastTree;
        private void OnSelectionChange()
        {
            if (!Application.isPlaying && _lastTree != null) {
                _lastTree.SetEditorOwner(null);
            }
            _lastTree = Selection.activeObject as BehaviourTree;
            if (!_lastTree) {
                if (Selection.activeObject) {
                    BTRunnerComponent runner;
                    try {
                        runner = Selection.activeGameObject.GetComponent<BTRunnerComponent>();
                    }
                    catch (Exception) {
                        runner = null;
                    }
                    if (runner) {
                        _lastTree = runner.Tree;
                        if (!Application.isPlaying) {
                            _lastTree.SetEditorOwner(runner.gameObject);
                        }
                        Debug.Log($"Selected BT Runner : {Selection.activeGameObject.name}");
                    }
                }
            }

            if (Application.isPlaying) {
                if (_lastTree) {
                    Debug.Log($"Open Tree : " + _lastTree.name);
                    _behaviourTreeView.PopulateView(_lastTree);
                }
            }
            else {
                if (_lastTree && AssetDatabase.CanOpenAssetInEditor(_lastTree.GetInstanceID())) {
                    Debug.Log($"Open Tree : " + _lastTree.name);
                    _behaviourTreeView.PopulateView(_lastTree);
                }
            }
        }
        private void OnDestroy() {
            _behaviourTreeView.OnNodeSelectionChanged -= OnNodeSelectionChange;
            _behaviourTreeView = null;
            _inspectorView = null;
            if (Application.isPlaying) {
                _lastTree = null;
                return;
            }
            _lastTree?.SetEditorOwner(null);
            _lastTree = null;
        }
        private void OnNodeSelectionChange(BTNodeView nodeView) {
            _inspectorView.UpdateSelection(nodeView);
        }

        private void OnInspectorUpdate() {
            _behaviourTreeView?.UpdateNodeStates();
        }
    }
}
