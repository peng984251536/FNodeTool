using UnityEngine;
using UnityEngine.Serialization;

namespace _02TreeBehaviour
{
    public abstract class BaseNode:ScriptableObject
    {
        public enum NodeState
        {
            Running,
            Failure,
            Success
        }

        public NodeState nodeState = NodeState.Running;
        public bool started = false;

        public NodeState UpdateState()
        {
            if (!started)
            {
                OnStart();
                started = true;
            }

            nodeState = OnUpdate();

            if (nodeState == NodeState.Failure || nodeState == NodeState.Success)
            {
                OnStop();
                started = false;
            }

            return nodeState;
        }

        protected abstract void OnStop();

        protected abstract NodeState OnUpdate();

        protected abstract void OnStart();
    }
}