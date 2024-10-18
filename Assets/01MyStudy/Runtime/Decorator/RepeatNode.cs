namespace MyEditorView.Runtime
{
    [NodeName("Repeat")]
    [NodePath("Base/RepeatNode")]
    public class RepeatNode : DecoratorNode
    {
        public int count;

        int currentIndex=0;

        protected override void DoAction()
        {
            currentIndex = 0;
        }

        protected override State OnUpdate()
        {
            switch (child.UpdateState())
            {
                case State.Running:
                    return State.Running;
                case State.Failure:
                    currentIndex++;
                    break;
                case State.Success:
                    currentIndex++;
                    break;
            }
            return State.Running;
        }

        protected override void OnStop()
        {
            base.OnStop();
            
        }
    }
}