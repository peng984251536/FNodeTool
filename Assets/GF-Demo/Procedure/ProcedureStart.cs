using System.Collections;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Demo
{
    /// <summary>
    /// 游戏启动流程
    /// </summary>
    public class ProcedureStart:ProcedureBase
    {
        private bool m_IsChangeProcedure = false;
        
        protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
        {
            GFLog.Debug("Hello World");
            base.OnInit(procedureOwner);
            
        }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GFLog.Debug($"{nameof(ProcedureStart)}:OnEnter");
            GameEntry.UI.OpenUIForm(UIPathDefault.NormalPath, UILayer.Normal,this);
            //切换状态
            //ChangeState<ProcedureInit>(procedureOwner);
        }


        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_IsChangeProcedure)
            {
                //切换状态
                ChangeState<ProcedureGameStart>(procedureOwner);
            }
            
        }


        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            GFLog.Debug($"{nameof(ProcedureStart)}:OnLeave");
            base.OnLeave(procedureOwner, isShutdown);
            
            
        }

        public void ChangeProcedure()
        {
            m_IsChangeProcedure = true;
        }
    }
}