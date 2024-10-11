using UnityEngine;

namespace _02TreeBehaviour
{
    public class CompositeNode:BaseNode
    {
        public string message;
        
        protected override void OnStop()
        {
            Debug.Log($"OnStop{message}");
        }

        protected override NodeState OnUpdate()
        {
            Debug.Log($"OnUpdate{message}");
            return NodeState.Success;
        }

        protected override void OnStart()
        {
            Debug.Log($"OnStart{message}");
        }
    }
}