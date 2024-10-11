using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace _02TreeBehaviour
{
    public class BehaviourTreeView:GraphView
    {
        public new class UxmlFactory:UxmlFactory<BehaviourTreeView,GraphView.UxmlTraits>
        {
            
        }
        
        
        public BehaviourTreeView()
        {
            Insert(0,new GridBackground());

            this.AddManipulator(new ContentZoomer());
            
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>
                ("Assets/02TreeBehaviour/EditorView/TreeBehaviourEditor.uss");
            styleSheets.Add(styleSheet);
        }
    }
}