
using Unity.Collections;
using UnityEngine;

namespace MyEditorView.Runtime
{
    public class RandomNode: DecoratorNode
    {
        public float probability;
        [ReadOnly]
        public float result;
        
        protected override void DoAction()
        {
            result = Random.Range(0f, 100f);
        }
        
        protected override State OnUpdate()
        {
            if(child!=null || !child.Enable)
                return State.Success;
            else if (result <= probability)
                return child.UpdateState();
            else
                return State.Failure;
        }
    }
}