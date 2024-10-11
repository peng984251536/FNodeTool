using System.Collections.Generic;
using MyEditorView.Runtime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyEditorView
{
    public static class NodeToolUtility
    {
        public static int FindPort(this VisualElement node, Port port)
        {
            for (int i = 0; i < node.childCount; i++)
            {
                if (node[i].Q<Port>() == port)
                    return i;
            }

            Debug.LogError("找不到节点-接口");
            return -1;
        }

        public static string FindPortName(this List<NodeLinkData> nodeLinkDatas, string guid, Direction direction,int index)
        {
            int k = 0;
            for (int i = 0; i < nodeLinkDatas.Count; i++)
            {
                if (direction == Direction.Input)
                {
                    if (nodeLinkDatas[i].InputNodeGuid == guid)
                    {
                        if(k==index)
                            return nodeLinkDatas[i].InPortName;
                        else
                            k += 1;
                    }
                }
                else if (direction == Direction.Output)
                {
                    if (nodeLinkDatas[i].OutputNodeGuid == guid)
                    {
                        if(k==index)
                            return nodeLinkDatas[i].OutPortName;
                        else
                            k += 1;
                    }
                }
            }

            return "";
        }
    }
}