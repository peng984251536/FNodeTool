using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MyEditorView.Runtime;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;


namespace MyEditorView
{
    /// <summary>
    /// 编辑器界面
    /// </summary>
    public class DialogueView : GraphView
    {
        public UnityEngine.Vector2 m_defaultNodeSize = new UnityEngine.Vector2(100, 200);
        public Blackboard Blackboard;
        public List<ExposedProperty> ExposedProperties = new List<ExposedProperty>();
        private FieldInfo[] NodeFields = null;

        private NodeSearchWindow m_searchWindow;
        private EditorWindow m_EditorWindow;


        public UnityEngine.Vector2 DefaultNodeSize
        {
            get { return m_defaultNodeSize; }
        }

        public DialogueView(EditorWindow editorWindow)
        {
            styleSheets.Add(Resources.Load<StyleSheet>("GridBackground"));
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var grid = new GridBackground();
            Insert(0, grid);
            //创建开始节点
            GenerateEntryPointNode();
            //菜单
            AddSearchWindow(editorWindow);
        }

        private int index = 115;

        #region 节点连接

        /// <summary>
        /// 设置接口的输入输出关系
        /// </summary>
        /// <param name="startPort"></param>
        /// <param name="nodeAdapter"></param>
        /// <returns></returns>
        public override List<Port> GetCompatiblePorts(Port startAnchor, NodeAdapter nodeAdapter)
        {
            //端口类型相同、方向不同、节点不同
            var compatiblePorts = new List<Port>();
            foreach (var port in ports.ToList())
            {
                if (startAnchor.node == port.node ||
                    startAnchor.direction == port.direction ||
                    startAnchor.portType != port.portType)
                {
                    continue;
                }

                compatiblePorts.Add(port);
            }

            return compatiblePorts;
        }

        #endregion

        #region 创建节点

        public DefaultEditorNode GenerateEntryPointNode(string guid = "")
        {
            if (guid == "")
                guid = Guid.NewGuid().ToString();
            // var node = new EntryPointEditorNode(this, guid)
            // {
            //     title = "START",
            //     DialogueText = "ENTRYPOINT",
            //     EntryPoint = true
            // };
            var baseNode = ScriptableObject.CreateInstance<EnterNode>();
            var nodeName = (baseNode.GetType().GetCustomAttributes(typeof(NodeName), false)[0] as NodeName).Name;
            string _guid = Guid.NewGuid().ToString();
            var dialogueNode = new DefaultEditorNode(this, baseNode, _guid)
            {
                title = nodeName,
                DialogueText = nodeName,
            };

            return dialogueNode;
        }

        /// <summary>
        /// 在view中创建节点
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public DialogueEditorNode CreateDialogueNode(string nodeName, string _guid = null)
        {
            if (_guid == null)
                _guid = Guid.NewGuid().ToString();
            var dialogueNode = new DialogueEditorNode(this, _guid)
            {
                title = nodeName,
                DialogueText = nodeName,
            };

            return dialogueNode;
        }

        /// <summary>
        /// 在view中创建节点
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public DefaultEditorNode CreateDefaultNode(BaseNode baseNode)
        {
            var nodeName = (baseNode.GetType().GetCustomAttributes(typeof(NodeName), false)[0] as NodeName).Name;
            string _guid = Guid.NewGuid().ToString();
            var dialogueNode = new DefaultEditorNode(this, baseNode, _guid)
            {
                title = nodeName,
                DialogueText = nodeName,
            };

            return dialogueNode;
        }

        /// <summary>
        /// 在view中创建节点
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public EditorNodeBase CreateNodeBase(string nodeName, string guid)
        {
            var dialogueNode = new EditorNodeBase(this, guid)
            {
                title = nodeName,
                DialogueText = nodeName,
            };

            return dialogueNode;
        }

        #endregion

        #region 点击节点
        
        private BaseNode m_curBaseNode = null;
        
        /// <summary>
        /// 刷新点击的节点
        /// </summary>
        /// <param name="nodeBase"></param>
        public virtual void OnSelectNode(EditorNodeBase nodeBase)
        {
            if(nodeBase.LocalBaseNode==null)
                return;
            m_curBaseNode = nodeBase.LocalBaseNode;
            InspectClassProperties(nodeBase.LocalBaseNode);
        }

        public virtual void InspectClassProperties(BaseNode baseNode)
        {
            Type type = baseNode.GetType();

            NodeFields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            
            // PropertyInfo[] properties = (type)
            //     .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var property in NodeFields)
            {
                // UnityEngine.Debug.Log(
                //     $"InspectClassProperties:type:{property.Name}--value:{property.FieldType}");
                var attributes = property.GetCustomAttributes(true);
                for (int i = 0; i < attributes.Length; i++)
                {
                    //Debug.Log($"---->> CustomAttributes:{attributes[i]}");
                }
            }

            UpdateBlackboardProperty();
        }

        public virtual void UpdateBlackboardProperty()
        {
            Blackboard.Clear();
            ExposedProperties.Clear();
            DialogueGraph.InitBlackBoard(this,Blackboard);

            for (int i = 0; i < NodeFields.Length; i++)
            {
                FieldInfo fieldInfo = NodeFields[i];
                ExposedProperty property = new ExposedProperty();
                GetTypeValue(fieldInfo,ref property);
                ExposedProperties.Add(property);
                AddPropertyToBlackBoard(property);
            }
            
        }

        public void GetTypeValue(FieldInfo fieldInfo,ref ExposedProperty property)
        {
            if (fieldInfo.FieldType == typeof(int))
            {
                int index = (int)(fieldInfo.GetValue(m_curBaseNode));
                property.PropertyValue = index.ToString();
            }
            else if (fieldInfo.FieldType == typeof(long))
            {
                long index = (long)(fieldInfo.GetValue(m_curBaseNode));
                property.PropertyValue = index.ToString();
            }
            else if (fieldInfo.FieldType == typeof(string))
            {
                string index = (string)(fieldInfo.GetValue(m_curBaseNode));
                property.PropertyValue = index;
            }
            else if (fieldInfo.FieldType == typeof(bool))
            {
                bool index = (bool)(fieldInfo.GetValue(m_curBaseNode));
                property.PropertyValue = index.ToString();
            }
            property.PropertyName = fieldInfo.Name;
        }
        
        #endregion

        #region 属性

        /// <summary>
        /// 在属性栏中添加属性
        /// </summary>
        /// <param name="exposedProperty"></param>
        public void AddPropertyToBlackBoard(ExposedProperty exposedProperty)
        {
            var property = new ExposedProperty();
            property.PropertyName = exposedProperty.PropertyName;
            property.PropertyValue = exposedProperty.PropertyValue;

            //显示
            var container = new VisualElement();
            var blackboardField = new BlackboardField()
            {
                text = property.PropertyValue,
                typeText = property.PropertyName
            };
            container.Add(blackboardField);

            //折叠属性
            var propertyValueTextFeild = new TextField("Value")
            {
                value = property.PropertyValue
            };
            propertyValueTextFeild.MarkDirtyRepaint();
            propertyValueTextFeild.RegisterValueChangedCallback((evt) =>
            {
                var changingPropertyIndex = ExposedProperties.FindIndex
                (
                    x => x.PropertyName == property.PropertyName
                );
                OnUpdateProperty<object>(changingPropertyIndex, evt.newValue);
            });
            var blackBoardValueRow = new BlackboardRow(blackboardField, propertyValueTextFeild);
            container.Add(blackBoardValueRow);

            Blackboard.Add(container);
        }

        private void OnUpdateProperty<T>(int changingPropertyIndex,T newValue)
        {
            if(m_curBaseNode==null)
                return;
            ExposedProperties[changingPropertyIndex].PropertyValue = newValue.ToString();
            NodeFields[changingPropertyIndex].SetValue(m_curBaseNode,newValue);
            
        }
        #endregion

        #region 菜单

        private void AddSearchWindow(EditorWindow editorWindow)
        {
            m_searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
            m_searchWindow.Init(editorWindow, this);
            //
            nodeCreationRequest = context =>
            {
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), m_searchWindow);
            };
        }

        #endregion
    }
}