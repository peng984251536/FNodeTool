

using System.Collections.Generic;
using MyEditorView;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyEditorView
{
    public class NodeSearchWindow:ScriptableObject,ISearchWindowProvider
    {
        private DialogueView m_DialogueView;
        private EditorWindow m_EditorWindow;
        private Texture2D m_indentationIcon;

        public void Init(EditorWindow window, DialogueView graphView)
        {
            m_DialogueView = graphView;
            m_EditorWindow = window;
            
            //
            m_indentationIcon = new Texture2D(1, 1);
            m_indentationIcon.SetPixel(0,0,new Color(0,0,0,1));
            m_indentationIcon.Apply();
        }
        
        /// <summary>
        /// 创建菜单页
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent("Cteate Elements"),0),
                
                new SearchTreeGroupEntry(new GUIContent("Dialogue"),1),
                new SearchTreeEntry(new GUIContent("Dialogue Node",m_indentationIcon))
                {
                    userData = nameof(DialogueNode),level = 2
                }
                
            };
            return tree;
        }
    
        /// <summary>
        /// 选中某个菜单页签时
        /// </summary>
        /// <param name="SearchTreeEntry"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            var worldMousePos = m_EditorWindow.rootVisualElement.ChangeCoordinatesTo
            (
                m_EditorWindow.rootVisualElement.parent,
                context.screenMousePosition - m_EditorWindow.position.position
            );
            //获取编辑器的局部位置
            var localMousePosition = m_DialogueView.contentViewContainer.WorldToLocal(worldMousePos);
            
            switch (SearchTreeEntry.userData)
            {
                case nameof(DialogueNode):
                    Debug.Log(nameof(SearchTreeEntry.userData));
                    var node = m_DialogueView.CreateDialogueNode("Dialogue Node");
                    var rect = node.GetPosition();
                    node.SetPosition(new Rect(localMousePosition,rect.size));
                    return true;
                default:
                    return false;
            }
        }
    }
}

