using GameFramework;
using GameFramework.Fsm;
using UnityEngine;

//using ProcedureOwner = GameFramework.Fsm.IFsm<Demo.EntityEnemy>;

namespace Demo
{
    public class AttackState : FsmState<EntityEnemy>, IReference
    {
        private EntityEnemy owner;
        protected EntityTargetable m_TargetTower;

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
            

            owner.Attacker.OnUpdate(elapseSeconds, realElapseSeconds);

            if (!owner.isPathBlocked)
            {
                ChangeState<MoveState>(procedureOwner);
                return;
            }

            // EntityTargetable tower = owner.Targetter.GetTarget();
            // if (tower != m_TargetTower)
            // {
            //     // if the current target is to be replaced, unsubscribe from removed event
            //     if (m_TargetTower != null)
            //     {
            //         m_TargetTower.OnHidden -= OnTargetTowerDestroyed;
            //     }
            //
            //     // assign target, can be null
            //     m_TargetTower = tower;
            //
            //     // if new target found subscribe to removed event
            //     if (m_TargetTower != null)
            //     {
            //         m_TargetTower.OnHidden += OnTargetTowerDestroyed;
            //     }
            // }
            // if (m_TargetTower == null)
            // {
            //     ChangeState<EnemyMoveState>(procedureOwner);
            // }
        }


        /// <summary>
        /// 通过反射删除回调
        /// </summary>
        public void Clear()
        {
        }
        
        public static AttackState Create()
        {
            AttackState state = ReferencePool.Acquire<AttackState>();
            return state;
        }
    }
}