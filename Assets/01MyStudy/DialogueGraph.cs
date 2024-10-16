using System;
using System.Linq;
using System.Reflection;
using MyEditorView.Runtime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UIElements;
using UnityGameFramework.Runtime;

namespace MyEditorView
{
    public class FileName
    {
        public const string BaseTree = "BaseTree";
    }
    
    /// <summary>
    /// 用于初始化编辑器窗口
    /// </summary>
    public class DialogueGraph:EditorWindow
    {
        private GraphSaveUtility m_GraphSaveUtility;
        private DialogueContainer m_currentContainer;
        private DialogueView m_DialogueView;
        private string m_fileName = "New Narrative";

        [MenuItem("My Graph/Dialogue Graph")]
        public static void OpenDialogueGraphWindow()
        {
            var window = GetWindow<DialogueGraph>();
            window.titleContent = new UnityEngine.GUIContent("Dialogue Graph");
        }

        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolbar();
            GenerateMiniMap();
            GenerateBlackBoard();
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(m_DialogueView);
        }

        /// <summary>
        /// 
        /// </summary>
        private void ConstructGraphView()
        {
            m_DialogueView = new DialogueView(this) { name = "Dialogue Graph" };

            m_DialogueView.StretchToParentSize();
            rootVisualElement.Add(m_DialogueView);
        }

        /// <summary>
        /// 置顶的工具栏
        /// </summary>
        private void GenerateToolbar()
        {
            var toolbar = new Toolbar();
            //创建节点按钮
            var nodeCreateButton = new Button(() => 
            { 
                m_DialogueView.CreateDialogueNode("Dialogue Node"); 
            });
            nodeCreateButton.text = "Create Node";
            toolbar.Add(nodeCreateButton);
            //数据名字（保存/加载）
            var fileNameTextField = new TextField("File Name");
            PlayerPrefs.GetString(FileName.BaseTree,"File Name");
            fileNameTextField.SetValueWithoutNotify(m_fileName);
            fileNameTextField.MarkDirtyRepaint();
            fileNameTextField.RegisterValueChangedCallback((evt =>
            {
                m_fileName = evt.newValue;
                PlayerPrefs.SetString(FileName.BaseTree,evt.newValue);
            }));
            //数据实体
            ObjectField treeField = new ObjectField();
            treeField.RegisterValueChangedCallback((e) =>
            {
                DialogueContainer container = e.newValue as DialogueContainer;
                if(m_GraphSaveUtility==null)
                    m_GraphSaveUtility = GraphSaveUtility.GetInstance(m_DialogueView);
                m_GraphSaveUtility.LoadGraph(container);

                m_fileName = container.name;
                fileNameTextField.SetValueWithoutNotify(m_fileName);
                PlayerPrefs.SetString(FileName.BaseTree,m_fileName);
            });
            treeField.objectType = typeof(DialogueContainer);
            m_currentContainer = Resources.Load<DialogueContainer>(m_fileName);
            if(m_currentContainer!=null)
                treeField.SetValueWithoutNotify(m_currentContainer);
            
            
            toolbar.Add(fileNameTextField);
            toolbar.Add(treeField);
            toolbar.Add(new Button((() => RequestDataOperation(true))){text = "SaveData"});
            toolbar.Add(new Button((() => RequestDataOperation(false))){text = "LoadData"});
            
            rootVisualElement.Add(toolbar);
        }

        /// <summary>
        /// 小地图
        /// </summary>
        private void GenerateMiniMap()
        {
            var miniMap = new MiniMap{anchored = true};
            //
            var cords = m_DialogueView.contentViewContainer.WorldToLocal(new Vector2(maxSize.x - 10, 30));
            miniMap.SetPosition(new Rect(cords.x,cords.y,200,140));
            m_DialogueView.Add(miniMap);
        }
        
        /// <summary>
        /// 添加定格背景版
        /// </summary>
        private void GenerateBlackBoard()
        {
            var blackboard = new Blackboard(m_DialogueView);
            InitBlackBoard(m_DialogueView,blackboard);
            m_DialogueView.Blackboard = blackboard;
            m_DialogueView.Add(blackboard);
        }
        
        private void RequestDataOperation(bool save)
        {
            if (string.IsNullOrEmpty(m_fileName))
            {
                //警告弹窗
                EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid file name.", "ok");
                return;
            }

            if(m_GraphSaveUtility==null)
                m_GraphSaveUtility = GraphSaveUtility.GetInstance(m_DialogueView);
            if (save)
            {
                m_GraphSaveUtility.SaveGraph(m_fileName);
            }
            else
            {
                m_GraphSaveUtility.LoadGraph(m_fileName);
            }

        }


        #region 点击节点

        public virtual void OnSelectNode(EditorNodeBase nodeBase)
        {
            InspectClassProperties(nodeBase.BaseNode);
        }

        public virtual void InspectClassProperties(BaseNode baseNode)
        {
            PropertyInfo[] properties = baseNode.GetType().GetProperties(BindingFlags.Public);

            foreach (var property in properties)
            {
                var v = property.Attributes;
                Log.Debug($"InspectClassProperties:type:{property.PropertyType}--value:{property.GetType()}");
            }
            
        }

        #endregion

        #region 静态工具

        public static void InitBlackBoard(DialogueView graphView,Blackboard blackboard)
        {
            blackboard.Add(new BlackboardSection(){title = "Exposed Properties"});
            //给这个属性板-添加属性
            blackboard.addItemRequested = blackboard =>
            {
                graphView.AddPropertyToBlackBoard(new ExposedProperty());
            };
            //编辑这个text的属性
            blackboard.editTextRequested = (blackboard1, element, newValue) =>
            {
                var oldPropertyName = ((BlackboardField)element).text;
                if (graphView.ExposedProperties.Any(x => x.PropertyName == newValue))
                {
                    EditorUtility.DisplayDialog("Error", "已经有这个属性了", "OK");
                    return;
                }

                var propertyIndex = graphView.ExposedProperties.FindIndex
                (
                    x => x.PropertyName == oldPropertyName
                );
                graphView.ExposedProperties[propertyIndex].PropertyName = newValue;
                ((BlackboardField)element).text = newValue;
            };
            blackboard.SetPosition(new Rect(10,30,200,300));
        }

        #endregion
    }
}