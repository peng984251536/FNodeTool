using System;
using System.Collections.Generic;
using System.Reflection;
using _02TreeBehaviour;
using Unity.VisualScripting;
using UnityEngine;

namespace MyEditorView.Runtime
{
    
    /// <summary>
    /// 单输出接口(组合)
    /// </summary>
    public abstract class CompositeNode:BaseNode,ICompositeNode
    {
        [SerializeField, HideInInspector]
        protected List<BaseNode> children = new List<BaseNode>();
        
        protected override void OnStart()
        {
        }
        
        public override void AddChild(BaseNode baseNode)
        {
            if (!children.Contains(baseNode))
                children.Add(baseNode);
        }

        public override void RemoveChild(BaseNode baseNode)
        {
            if (children.Contains(baseNode))
                children.Remove(baseNode);
        }
        
        public sealed override List<BaseNode> GetChildren()
        {
            return children;
        }
    }
}