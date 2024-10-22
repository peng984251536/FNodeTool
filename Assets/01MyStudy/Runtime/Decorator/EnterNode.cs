namespace MyEditorView.Runtime
{
    /// <summary>
    /// 按顺序执行所有子节点
    /// </summary>
    [NodeName("EnterNode")]
    [NodePath("Base/EnterNode")]
    public class EnterNode : DecoratorNode
    {

        protected override State OnUpdate()
        {
            return child.UpdateState();
        }

        protected override void DoAction()
        {
            
        }

        protected override void OnStop()
        {
        }
        
    }
}