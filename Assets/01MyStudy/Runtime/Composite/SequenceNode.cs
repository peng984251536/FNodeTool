namespace MyEditorView.Runtime
{
    /// <summary>
    /// 按顺序执行所有子节点
    /// </summary>
    [NodeName("SequenceNode")]
    [NodePath("Base/SequenceNode")]
    public class SequenceNode : CompositeNode
    {
        public int ExposedInt = 0;

        protected override void OnReset()
        {
            ExposedInt = 0;
        }

        protected override void OnStart()
        {
            ExposedInt = 0;
        }

        protected override State OnUpdate()
        {
            //当所有的children都运算完才结束
            while (ExposedInt < children.Count)
            {
                var child = children[ExposedInt%(children.Count)];
                if (!child.Enable)
                {
                    return State.Success;
                }

                switch (child.UpdateState())
                {
                    case State.Failure:
                        ExposedInt++;
                        return State.Running;
                    case State.Success:
                        ExposedInt++;
                        return State.Running;
                    case State.Running:
                        ExposedInt++;
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