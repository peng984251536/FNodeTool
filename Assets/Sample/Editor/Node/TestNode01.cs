using FNode.Editor;
using FNode.Runtime;
using UnityEngine.UIElements;



[GraphViewMenuItem("TestNode01","Test/TestNode01",0,2)]
public class TestNode01 : GenericNodeBase<TestNodeData>
{
    TextField txt;
    public TestNode01() : base("测试节点2")
    {
        CreateInputPort("测试输入","TestInput", typeof(int), false);
        CreateOutputPort("测试输出","TestOutput", typeof(int), false);
        CreateOutputPort("测试输出2", "TestOutput2", typeof(int), false);
        txt = new TextField("Txt");
        AddContent(txt);
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
