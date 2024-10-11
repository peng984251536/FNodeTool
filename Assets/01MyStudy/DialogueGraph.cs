using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyEditorView
{
    /// <summary>
    /// 用于初始化编辑器窗口
    /// </summary>
    public class DialogueGraph:EditorWindow
    {
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
            //（保存/加载）
            var fileNameTextField = new TextField("File Name");
            fileNameTextField.SetValueWithoutNotify(m_fileName);
            fileNameTextField.MarkDirtyRepaint();
            fileNameTextField.RegisterValueChangedCallback((evt =>
            {
                m_fileName = evt.newValue;
            }));
            toolbar.Add(fileNameTextField);
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
            blackboard.Add(new BlackboardSection(){title = "Exposed Properties"});
            //给这个属性板-添加属性
            blackboard.addItemRequested = blackboard =>
            {
                m_DialogueView.AddPropertyToBlackBoard(new ExposedProperty());
            };
            //编辑这个text的属性
            blackboard.editTextRequested = (blackboard1, element, newValue) =>
            {
                var oldPropertyName = ((BlackboardField)element).text;
                if (m_DialogueView.ExposedProperties.Any(x => x.PropertyName == newValue))
                {
                    EditorUtility.DisplayDialog("Error", "已经有这个属性了", "OK");
                    return;
                }

                var propertyIndex = m_DialogueView.ExposedProperties.FindIndex
                (
                    x => x.PropertyName == oldPropertyName
                );
                m_DialogueView.ExposedProperties[propertyIndex].PropertyName = newValue;
                ((BlackboardField)element).text = newValue;
            };
            blackboard.SetPosition(new Rect(10,30,200,300));
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

            var saveUtility = GraphSaveUtility.GetInstance(m_DialogueView);
            if (save)
            {
                saveUtility.SaveGraph(m_fileName);
            }
            else
            {
                saveUtility.LoadGraph(m_fileName);
            }

        }


        
    }
}