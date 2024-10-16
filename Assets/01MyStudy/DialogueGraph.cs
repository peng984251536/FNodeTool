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
    public class FileState
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

        private TextField m_TextField = null;
        public string FileName
        {
            get
            {
                m_fileName = PlayerPrefs.GetString(FileState.BaseTree, "File Name");
                return m_fileName;
            }
            set
            {
                PlayerPrefs.SetString(FileState.BaseTree,value);
                m_fileName = value;
            }
        }

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
            m_TextField = new TextField("File Name");
            m_TextField.SetValueWithoutNotify(FileName);
            m_TextField.MarkDirtyRepaint();
            m_TextField.RegisterValueChangedCallback((evt =>
            {
                FileName = evt.newValue;
            }));
            //数据实体
            ObjectField treeField = new ObjectField();
            treeField.RegisterValueChangedCallback((e) =>
            {
                FileName = e.newValue.name;
                RequestDataOperation(false);
            });
            treeField.objectType = typeof(DialogueContainer);
            
            
            toolbar.Add(m_TextField);
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
            if (string.IsNullOrEmpty(FileName))
            {
                //警告弹窗
                EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid file name.", "ok");
                return;
            }


            m_GraphSaveUtility = GraphSaveUtility.GetInstance(m_DialogueView);
            if (save)
            {
                m_GraphSaveUtility.SaveGraph(FileName);
            }
            else
            {
                m_GraphSaveUtility.LoadGraph(FileName);
            }

        }

        private void UpdateFileName(string fileName)
        {
            
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