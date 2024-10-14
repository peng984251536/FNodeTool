using System;
using UnityEngine.Serialization;

namespace MyEditorView.Runtime
{
    //单项输入输出
    /// <summary>
    /// 节点的连线数据
    /// </summary>
    [Serializable]
    public class NodeLinkData
    {
        //public string OutPortName;
        //public string InPortName;
        
        /// <summary>
        /// output-输出
        /// </summary>
        public string OutputNodeGuid;
        public int OutputNodeIndex;
        
        /// <summary>
        /// intput-输入
        /// </summary>
        public string InputNodeGuid;
        public int InputNodeIndex;
    }
}