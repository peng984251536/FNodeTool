

using System;
using System.Collections.Generic;
using System.Linq;
using MyEditorView;
using MyEditorView.Runtime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyEditorView
{
    public class NodeSearchWindow:ScriptableObject,ISearchWindowProvider
    {
        protected List<string> pathes;
        
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
            // var tree = new List<SearchTreeEntry>()
            // {
            //     new SearchTreeGroupEntry(new GUIContent("Cteate Elements"),0),
            //     
            //     new SearchTreeGroupEntry(new GUIContent("Dialogue"),1),
            //     new SearchTreeEntry(new GUIContent("Dialogue Node",m_indentationIcon))
            //     {
            //         userData = nameof(DialogueEditorNode),level = 2
            //     }
            //     
            // };
            // return tree;
            pathes = new List<string>();
            var tree = new List<SearchTreeEntry>();
            tree.Add(new SearchTreeGroupEntry(new GUIContent("Create Nodes")));//标题
            
            List<string> NodePathStart = new List<string> { "Base" };
            foreach (var item in NodePathStart)
            {
                tree.Add(new SearchTreeGroupEntry(new GUIContent(item), 1));
                var baseTypes = FindTypes(item);
                baseTypes = baseTypes.OrderBy(s => GetNodePath(s)).ToList();
                foreach (var type in baseTypes)
                {
                    CreateEntry(type, item, ref tree);
                }
            }
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

            //Debug.Log($"OnSelectEntry::{nameof(SearchTreeEntry.userData)}");
            Type type = SearchTreeEntry.userData as Type;
            BaseNode node = CreateInstance(type) as BaseNode;
            if (node == null)
                return false;
            var nodeView = m_DialogueView.CreateDefaultNode(node);
            var rect = nodeView.GetPosition();
            nodeView.SetPosition(new Rect(localMousePosition,rect.size));
            // switch (SearchTreeEntry.userData)
            // {
            //     case nameof(DialogueEditorNode):
            //         Debug.Log(nameof(SearchTreeEntry.userData));
            //         var node = m_DialogueView.CreateDialogueNode("Dialogue Node");
            //         var rect = node.GetPosition();
            //         node.SetPosition(new Rect(localMousePosition,rect.size));
            //         return true;
            //     default:
            //         return false;
            // }
            return true;
        }


        #region 菜单页签2.0
        
        /// <summary>
        /// 找到利用这个路径为节点路径的所有派生类
        /// </summary>
        /// <param name="startWith"></param>
        /// <returns></returns>
        protected List<Type> FindTypes(string startWith)
        {
            List<Type> types = new List<Type>();
            //获取基于BaseNode派生的所有集合
            var childTypes = TypeCache.GetTypesDerivedFrom<BaseNode>();
            foreach (var type in childTypes)
            {
                //是否是抽象类
                if (type.IsAbstract)
                    continue;
                //获取这个类的标签
                var nodePathes = type.GetCustomAttributes(typeof(NodePath), false);
                if (nodePathes.Length == 0)
                    continue;
                var nodePath = nodePathes[0] as NodePath;
                if (!nodePath.Path.StartsWith(startWith))
                    continue;
                var pathStrs = nodePath.Path.Split(new char[] { '/' });
                if (pathStrs.Length == 1)
                    continue;
                types.Add(type);
            }
            // foreach (var item in editorWindow.NodeTypeFilter)
            // {
            //     if (types.Contains(item))
            //         types.Remove(item);
            // }
            // validTypes.AddRange(types);
            return types;
        }
        
        /// <summary>
        /// 加载菜单路径（根据路径标签）
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected string GetNodePath(Type type)
        {

            var nodePathes = type.GetCustomAttributes(typeof(NodePath), false);
            if (nodePathes.Length == 0)
                return string.Empty;
            var nodePath = nodePathes[0] as NodePath;
            
            // ///临时用
            // if (TreeDesignerSet.Instance.showChinese && TreeDesignerSet.Instance.NodeName(type) != string.Empty)
            // {
            //     var pathStrs = nodePath.Path.Split(new char[] { '/' });
            //     pathStrs[pathStrs.Length - 1] = TreeDesignerSet.Instance.NodeName(type);
            //     var nodePathStr = string.Empty;
            //     for (int i = 0; i < pathStrs.Length; i++)
            //     {
            //         nodePathStr += pathStrs[i];
            //         if (i < pathStrs.Length - 1)
            //             nodePathStr += "/";
            //     }
            //     return nodePathStr;
            // }
            
            return nodePath.Path;
        }
        
        /// <summary>
        /// 注册实体、菜单
        /// </summary>
        /// <param name="type"></param>
        /// <param name="startWith"></param>
        /// <param name="tree"></param>
        protected void CreateEntry(Type type, string startWith, ref List<SearchTreeEntry> tree)
        {
            var pathStrs = GetNodePath(type).Split(new char[] { '/' });
            if (pathStrs.Length == 1)
                return;
            string path = $"{startWith}/";
            for (int i = 1; i < pathStrs.Length; i++)
            {
                int level = i + 1;
                string pathStr = pathStrs[i];
                if (i == pathStrs.Length - 1)
                {
                    //菜单实体
                    tree.Add(new SearchTreeEntry(new GUIContent(pathStrs[i]))
                    {
                        userData = type,
                        level = level
                    });
                }
                else
                {
                    //菜单
                    path += pathStr + "/";
                    if (!pathes.Contains(path))
                    {
                        tree.Add(new SearchTreeGroupEntry(new GUIContent(pathStr,m_indentationIcon), level));
                        pathes.Add(path);
                    }
                }
            }
        }
        
        #endregion
        
    }
}

