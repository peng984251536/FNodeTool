using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MyEditorView.Runtime
{
    
    /// <summary>
    /// 
    /// </summary>
    [NodeName("RandomSelectorNode")]
    [NodePath("Base/RandomSelectorNode")]
    public class RandomSelectorNode : CompositeNode
    {
        protected override void OnReset()
        {
            
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (children.Count > 0)
            {
                int index = Random.Range(0, children.Count - 1);
                var child = children[index];
                if (child.Enable)
                    return child.UpdateState();
            }
            return State.Success;
        }
        
    }
}