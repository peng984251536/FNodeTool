using GameFramework;
using GameFramework.Fsm;
using UnityEngine;

//using ProcedureOwner = GameFramework.Fsm.IFsm<Demo.EntityEnemy>;

namespace Demo
{
    public class MoveState : FsmState<EntityEnemy>, IReference
    {
        private EntityEnemy owner;

        protected override void OnInit(IFsm<EntityEnemy> procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(IFsm<EntityEnemy> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            owner = procedureOwner.Owner;
            // owner.Agent.isStopped = false;
            // owner.Attacker.enabled = false;
            // owner.Agent.SetDestination(owner.LevelPath.PathNodes[targetPathNodeIndex].position);
            // owner.Targetter.transform.position = owner.transform.position;
        }

        protected override void OnUpdate(IFsm<EntityEnemy> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            Vector3 targetTowerPos = Vector3.up*101;
            float distanceToTower = Vector3.Distance(owner.transform.position, targetTowerPos);
            if (distanceToTower > owner.EntityDataEnemy.Range)
            {
                return;
            }

            ChangeState<AttackState>(procedureOwner);
        }


        /// <summary>
        /// 通过反射删除回调
        /// </summary>
        public void Clear()
        {
        }
        
        public static MoveState Create()
        {
            MoveState state = ReferencePool.Acquire<MoveState>();
            return state;
        }
    }
}