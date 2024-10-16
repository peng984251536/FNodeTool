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

        public EntryPointEditorNode GenerateEntryPointNode(string guid = "")
        {
            if (guid == "")
                guid = Guid.NewGuid().ToString();
            var node = new EntryPointEditorNode(this, guid)
            {
                title = "START",
                DialogueText = "ENTRYPOINT",
                EntryPoint = true
            };

            return node;
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

        private FieldInfo[] m_nodeFields = null;
        private BaseNode m_curBaseNode = null;
        
        /// <summary>
        /// 刷新点击的节点
        /// </summary>
        /// <param name="nodeBase"></param>
        public virtual void OnSelectNode(EditorNodeBase nodeBase)
        {
            m_curBaseNode = nodeBase.BaseNode;
            InspectClassProperties(nodeBase.BaseNode);
        }

        public virtual void InspectClassProperties(BaseNode baseNode)
        {
            Type type = baseNode.GetType();

            m_nodeFields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            
            // PropertyInfo[] properties = (type)
            //     .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var property in m_nodeFields)
            {
                UnityEngine.Debug.Log(
                    $"InspectClassProperties:type:{property.Name}--value:{property.FieldType}");
                var attributes = property.GetCustomAttributes(true);
                for (int i = 0; i < attributes.Length; i++)
                {
                    Debug.Log($"---->> CustomAttributes:{attributes[i]}");
                }
            }
        }

        public virtual void UpdateBlackboardProperty()
        {
            DialogueGraph.InitBlackBoard(this,Blackboard);

            for (int i = 0; i < m_nodeFields.Length; i++)
            {
                FieldInfo fieldInfo = m_nodeFields[i];
                ExposedProperty property = new ExposedProperty();
                property.PropertyName = fieldInfo.Name;

                #region 判断类型
                object data = fieldInfo.GetValue(m_curBaseNode);
                bool isInt = 

                #endregion
                
                
                property.PropertyValue = fieldInfo.GetValue(m_curBaseNode)

            }
            
        }

        public void GetTypeValue(FieldInfo fieldInfo,ref ExposedProperty property)
        {
            switch (fieldInfo.FieldType)
            {
                case typeof(int):
                    break;
            }
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
            ExposedProperties.Add(property);

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
            propertyValueTextFeild.RegisterValueChangedCallback((evt) =>
            {
                var changingPropertyIndex = ExposedProperties.FindIndex
                (
                    x => x.PropertyName == property.PropertyName
                );
                ExposedProperties[changingPropertyIndex].PropertyValue = evt.newValue;
            });
            var blackBoardValueRow = new BlackboardRow(blackboardField, propertyValueTextFeild);
            container.Add(blackBoardValueRow);

            Blackboard.Add(container);
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