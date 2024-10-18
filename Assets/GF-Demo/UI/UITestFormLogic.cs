using System.Collections;
using System.Collections.Generic;
using Demo;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = Demo.GameEntry;

public class UITestFormLogic : UIFormLogic
{
    [SerializeField] private Button m_BtnClick;

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
        GFLog.Debug($"UITestFormLogic£∫¥Ú”°»’÷æ°£");
        m_ProcedureStart.ChangeProcedure();
        GameEntry.UI.CloseUIForm(this.UIForm);
    }
}
