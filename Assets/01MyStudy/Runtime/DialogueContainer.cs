using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyEditorView.Runtime
{
    [Serializable]
    public class DialogueContainer:ScriptableObject
    {
        //连线数据
        public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();
        //节点数据
        public List<DialogueNodeData> DialogueNodeDatas = new List<DialogueNodeData>();
        public List<BaseNode> BaseNodes = new List<BaseNode>();
        
        
                
        /// <summary>
        /// 在 view 加载保存的节点
        /// </summary>
        public BaseTree CreateBaseTree()
        {
            if(DialogueNodeDatas==null)
                return null;

            BaseTree baseTree = ScriptableObject.CreateInstance<BaseTree>();
            baseTree.treeState = BaseNode.State.Default;

            // for (int i = 0; i < DialogueNodeDatas.Count; i++)
            // {
            //     baseTree.Nodes.Add(DialogueNodeDatas[i].userData);
            //     if (DialogueNodeDatas[i].NodeType.Contains(nameof(EntryPointEditorNode)))
            //     {
            //         baseTree.rootNode = DialogueNodeDatas[i].userData;
            //     }
            // }
            for (int i = 0; i < BaseNodes.Count; i++)
            {
                baseTree.Nodes.Add(BaseNodes[i]);
                if (DialogueNodeDatas[i].NodeType.Contains(nameof(EntryPointEditorNode)))
                {
                    baseTree.rootNode = BaseNodes[i];
                }
            }

            return baseTree;
        }
    }
}