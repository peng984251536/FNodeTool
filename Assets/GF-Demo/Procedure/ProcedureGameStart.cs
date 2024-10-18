using System.Collections;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;



namespace Demo
{
    public class ProcedureGameStart:ProcedureBase
    {
        protected override void OnInit(IFsm<IProcedureManager> procedureOwner)
        {
            GFLog.Debug("Hello World");
            base.OnInit(procedureOwner);
            
            
        }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GFLog.Debug($"{nameof(ProcedureGameStart)}:OnEnter ==========================");
            //切换状态
            //ChangeState<ProcedureInit>(procedureOwner);
            
            GameEntry.Entity.ShowEntity<JammoEntityLogic>(1,EntityDefault.Jammo,"Prefabs");
        }


        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            
        }


        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            GFLog.Debug($"{nameof(ProcedureGameStart)}:OnLeave ==========================");
            base.OnLeave(procedureOwner, isShutdown);
            
            
        }
    }
}