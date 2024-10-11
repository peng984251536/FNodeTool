using UnityEngine;

namespace _02TreeBehaviour
{
    public class RepeatNode:DecoratorNode
    {
        protected override void OnStart()
        {
            
        }
        
        protected override void OnStop()
        {
            base.OnStop();
        }

        protected override NodeState OnUpdate()
        {
            //刷新下一节点的状态
            NodeState nodeState = childs[ChildIndex].UpdateState();
            if(nodeState == NodeState.Success)
                ChildIndex++;
            return NodeState.Running;
        }

        protected override void DoAction()
        {
        }


    }
}