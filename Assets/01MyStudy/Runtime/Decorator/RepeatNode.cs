namespace MyEditorView.Runtime
{
    [NodeName("Repeat")]
    [NodePath("Base/RepeatNode")]
    public class RepeatNode : DecoratorNode
    {

        protected override void DoAction()
        {
        }

        protected override State OnUpdate()
        {
            switch (child.UpdateState())
            {
                case State.Running:
                    return State.Running;
                case State.Failure:
                    return State.Running;
                case State.Success:
                    return State.Running;
            }
            return State.Running;
        }

        protected override void OnStop()
        {
            base.OnStop();
            
        }
    }
}