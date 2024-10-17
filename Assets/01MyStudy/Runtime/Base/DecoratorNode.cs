using System;
using System.Collections.Generic;
using System.Reflection;
using _02TreeBehaviour;
using Unity.VisualScripting;
using UnityEngine;

namespace MyEditorView.Runtime
{
    /// <summary>
    /// 修饰器
    /// </summary>
    public abstract class DecoratorNode : BaseNode, IDecoratorNode
    {
        [SerializeField, HideInInspector] protected BaseNode child;

        protected sealed override void GetValue()
        {
            base.GetValue();
        }

        protected sealed override void OnReset()
        {
        }

        protected sealed override void OnStart()
        {
            GetValue();
            DoAction();
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            return (child!=null && child.Enable) ? child.UpdateState() : State.Success;
        }

        /// <summary>
        /// 开始时调用
        /// </summary>
        protected abstract void DoAction();


        public override void AddChild(BaseNode baseNode)
        {
            child = baseNode;
        }

        public override void RemoveChild(BaseNode baseNode)
        {
            child = null;
        }
        public override void RemoveAllChild()
        {
            child = null;
        }

        public override List<BaseNode> GetChildren()
        {
            if (child != null)
                return new List<BaseNode> { child };
            else
                return new List<BaseNode>();
        }
        
    }
}