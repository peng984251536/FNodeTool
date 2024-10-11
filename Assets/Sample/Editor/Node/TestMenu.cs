using FNode.Editor;
using FNode.Runtime;
using UnityEngine.UIElements;



[GraphViewMenuItem("Test","Test",0,2)]
public class TestMenu : GenericNodeBase<TestNodeData>
{
    TextField txt;
    public TestMenu() : base("测试节点2")
    {
    }

    protected override void SyncFromData()
    {
        txt.value = FieldsInfo.textInfo;
    }

    protected override void SyncToData()
    {
        FieldsInfo.textInfo = txt.value;
    }
}
