using System;
using MyEditorView.Runtime;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(UITestFormLogic))]
public class ObjectPathEditor : Editor
{
    SerializedProperty propertyList;
    ReorderableList list;
    
    private UITestFormLogic mTarget;
    
    private void OnEnable()
    {
        mTarget = (UITestFormLogic)(target);
        
        propertyList = serializedObject.FindProperty("mWidgetPrefabPathList");
        
        list = new ReorderableList(serializedObject, propertyList, true, true, true, true);
        list.elementHeight = 46;
        list.drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, "节点模板列表"); };
        list.drawElementCallback = (rect, index, active, focused) =>
        {
            SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
            EditorGUI.LabelField(new Rect(rect.x, rect.y + 2, 60, 20), $"Element {index.ToString()}");
            var path = element.stringValue;
            var tFileObj = Resources.Load(path);
            var newObj =
                CreateObjectField<DialogueContainer>(new Rect(rect.x + 70, rect.y + 2, rect.width - 80, 20), tFileObj);
            if (tFileObj != newObj)
            {
                var newPath = (AssetDatabase.GetAssetPath(newObj));
                if (path != newPath)
                {
                    element.stringValue = newPath;
                }
            }

            EditorGUI.LabelField(new Rect(rect.x, rect.y + 24, rect.width, 20),
                $"path:{(newObj != null ? path : "无")}");
        };
        list.onAddCallback = reorderableList => { ReorderableList.defaultBehaviours.DoAddButton(reorderableList); };
        list.onRemoveCallback = reorderableList =>
        {
            //if (EditorUtility.DisplayDialog("警告", "确定删除？", "确定", "取消"))
            //{
            ReorderableList.defaultBehaviours.DoRemoveButton(reorderableList);
            //}
        };
    }
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
    
    private Object CreateObjectField<T>(Rect rect, Object obj)
    {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginHorizontal();
        //EditorGUILayout.LabelField(title);
        var ret = EditorGUI.ObjectField(rect, obj, typeof(T), true);
        if (ret != obj)
        {
            RecordUndo(typeof(T).Name);
        }

        if (EditorGUI.EndChangeCheck())
        {
            SetDirty();
        }

        EditorGUILayout.EndHorizontal();
        return ret;
    }
    
    /// <summary>
    /// 记录undo
    /// </summary>
    /// <param name="key"></param>
    /// <param name="isSeparately"></param>
    protected void RecordUndo(string key, bool isSeparately = false)
    {
        Undo.RegisterCompleteObjectUndo(base.target, key);
        if (isSeparately)
        {
            Undo.IncrementCurrentGroup();
        }
    }
    
    /// <summary>
    /// 设置已改变
    /// </summary>
    private new void SetDirty()
    {
        EditorUtility.SetDirty(base.target);
    }
    
}