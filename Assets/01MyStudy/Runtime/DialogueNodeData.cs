using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace MyEditorView.Runtime
{
    [Serializable]
    public class DialogueNodeData
    {
        public string NodeType;
        public string Guid;
        public string DialogueText;
        public Vector2 Position;
        
        /// <summary>
        /// output-输出
        /// </summary>
        public int  OutputCount;
        
        /// <summary>
        /// intput-输入
        /// </summary>
        public int  InputCount;
    }
}