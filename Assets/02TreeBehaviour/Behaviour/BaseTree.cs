using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _02TreeBehaviour
{
    public class BaseTree:ScriptableObject
    {
        public BaseNode rootNode;
        [FormerlySerializedAs("treeState")] public BaseNode.NodeState treeNodeState;
        public Action onUpdateEvent;
        public Action onStoppedEvent;
        
        bool running;
        
        public virtual BaseNode.NodeState UpdateState()
        {
            if (running == false)
                OnStarted();
            if (treeNodeState == BaseNode.NodeState.Running)
            {
                treeNodeState = rootNode.UpdateState();
                onUpdateEvent?.Invoke();
            }
            if (treeNodeState == BaseNode.NodeState.Success || treeNodeState == BaseNode.NodeState.Failure)
                OnStopped();
            return treeNodeState;
        }
        
        public virtual void OnStarted()
        {
            //ResetState();
            treeNodeState = BaseNode.NodeState.Running;
            running = true;
        }
        public virtual void OnStopped()
        {
            running = false;
            onStoppedEvent?.Invoke();
            onStoppedEvent = null;
        }
    }
}