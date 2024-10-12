namespace MyEditorView.Runtime
{
    /// <summary>
    /// 执行所有子节点
    /// </summary>
    public class SequenceNode : CompositeNode
    {
        
        public int ExposedInt = 0;

        protected override void OnReset()
        {
        }

        protected override void OnStart()
        {
        }

        protected override State OnUpdate()
        {
            while (ExposedInt < children.Count)
            {
                var child = children[ExposedInt];
                if (!child.Enable)
                {
                    ExposedInt = (ExposedInt + 1) % children.Count;
                    continue;
                }

                switch (child.UpdateState())
                {
                    case State.Failure:
                        return State.Failure;
                    case State.Success:
                        ExposedInt = (ExposedInt + 1) % children.Count;
                        break;
                    case State.Running:
                        return State.Running;
                }
            }

            return State.Success;
        }
        
        protected override void OnStop()
        {
            ExposedInt = 0;
        }
    }
}