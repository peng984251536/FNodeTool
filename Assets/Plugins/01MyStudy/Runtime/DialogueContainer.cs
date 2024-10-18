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
        
        public Dictionary<string,BaseNode> BaseNodes = new Dictionary<string,BaseNode>();
    }
}