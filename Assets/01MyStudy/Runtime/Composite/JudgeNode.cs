
namespace MyEditorView.Runtime
{
    public class JudgeNode : DecoratorNode
    {
        protected bool isRight;
        public bool alwaysRight;

        protected sealed override State OnUpdate()
        {
            if (isRight || alwaysRight)
                return (child!=null && child.Enable) ? child.UpdateState() : State.Success;
            else
                return State.Failure;
        }
        protected sealed override void DoAction()
        {
            //Judge();
        }
    
        //protected virtual void Judge();

        public JudgeNode(BaseTree baseTree) : base(baseTree)
        {
        }
    }
}