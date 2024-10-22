using System;
using System.Collections;
using System.Collections.Generic;
using Demo;
using MyEditorView.Runtime;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = Demo.GameEntry;

[Serializable]
public class MonoPath<T> where T: MonoBehaviour
{
    [SerializeField]
    private T GameObject;
    [ReadOnly]
    public string Path="";
}
[Serializable]
public class ObjectPath
{
    [SerializeField]
    private Transform GameObject;
    [ReadOnly]
    public string Path="";
}

public class UITestFormLogic : UIFormLogic
{
    
    [SerializeField] private Button m_BtnClick;
    
    [SerializeField] [Header("���ݽڵ�ģ�常��")] private Transform mWidgetParent;
    [SerializeField] [Header("���ݽڵ�·���б�")] [HideInInspector] private List<string> mWidgetPrefabPathList;

    private ProcedureStart m_ProcedureStart;
    
    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
        RectTransform transform = GetComponent<RectTransform>();
        transform.sizeDelta = Vector2.zero;
        transform.anchoredPosition = Vector2.zero;
        
        m_ProcedureStart = userData as ProcedureStart;
        m_BtnClick.onClick.AddListener(OnClickBtn);
    }

    private void OnClickBtn()
    {
        GFLog.Debug($"UITestFormLogic����ӡ��־��");
        m_ProcedureStart.ChangeProcedure();
        GameEntry.UI.CloseUIForm(this.UIForm);
    }
}
