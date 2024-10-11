using System.Collections.Generic;
using UnityEngine;

namespace _02TreeBehaviour
{
    public abstract class DecoratorNode : BaseNode
    {
        public List<BaseNode>  childs = new List<BaseNode>();
        private int childIndex = 0;
        protected int ChildIndex
        {
            get { return childIndex % (childs.Count); }
            set { childIndex = value; }
        }
        
        protected override void OnStart()
        {
            DoAction();
        }

        protected override void OnStop()
        {
        }

        protected override NodeState OnUpdate()
        {
            return childs[ChildIndex] ? childs[ChildIndex].UpdateState() : NodeState.Success;
        }

        protected abstract void DoAction();
        
    }
}