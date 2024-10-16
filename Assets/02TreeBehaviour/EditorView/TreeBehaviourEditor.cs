using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace _02TreeBehaviour
{
    public class TreeBehaviourEditor : EditorWindow
    {
        [MenuItem("Window/UI Toolkit/TreeBehaviourEditor")]
        public static void ShowExample()
        {
            TreeBehaviourEditor wnd = GetWindow<TreeBehaviourEditor>();
            wnd.titleContent = new GUIContent("TreeBehaviourEditor");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // VisualElements objects can contain other VisualElement following a tree hierarchy.
            VisualElement label = new Label("Hello World! From C#");
            root.Add(label);

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/02TreeBehaviour/EditorView/TreeBehaviourEditor.uxml");
            VisualElement labelFromUXML = visualTree.Instantiate();
            root.Add(labelFromUXML);

            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/02TreeBehaviour/EditorView/TreeBehaviourEditor.uss");
            VisualElement labelWithStyle = new Label("Hello World! With Style");
            labelWithStyle.styleSheets.Add(styleSheet);
            root.Add(labelWithStyle);
        }
    }
}

