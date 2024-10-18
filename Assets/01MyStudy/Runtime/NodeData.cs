using System;
using System.Collections.Generic;
using FNode.Editor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace MyEditorView.Runtime
{
    [Serializable]
    public class NodeData
    {
        public MyEditorView.Runtime.BaseNode userData;
        public string NodeType;
        public string Guid;
        public string DialogueText;
        public Vector2 Position;

        
        /// <summary>
        /// output-输出
        /// </summary>
        public string[] OutPortName;

        /// <summary>
        /// intput-输入
        /// </summary>
        public string[] InPortName;
    }
}