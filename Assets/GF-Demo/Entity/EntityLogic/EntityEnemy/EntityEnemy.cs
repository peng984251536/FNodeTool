using System.Collections.Generic;
using GameFramework;
using GameFramework.Fsm;
using UnityEngine;
using UnityEngine.AI;

namespace Demo
{
    public class EntityEnemy : EntityTargetable, IPause
    {
        #region 属性

        protected IFsm<EntityEnemy> fsm;
        //实体的 有限状态机
        protected List<FsmState<EntityEnemy>> stateList;

        public override EnumAlignment Alignment
        {
            get { return EnumAlignment.Enemy; }
        }

        public EntityDataEnemy EntityDataEnemy
        {
            get;
            private set;
        }
        
        public Attacker Attacker
        {
            get;
            private set;
        }
        public NavMeshAgent Agent
        {
            get;
            private set;
        }
        //是否接近了目标
        public bool isPathBlocked
        {
            get { return Agent.pathStatus == NavMeshPathStatus.PathPartial; }
        }
        #endregion


        public void Pause()
        {
        }

        public void Resume()
        {
        }

        #region FSM

        protected virtual void AddFsmState()
        {
            stateList.Add(MoveState.Create());
            stateList.Add(IdleState.Create());
            stateList.Add(AttackState.Create());
        }

        protected virtual void StartFsm()
        {
            fsm.Start<IdleState>();
        }

        private void CreateFsm()
        {
            AddFsmState();
            fsm = GameEntry.Fsm.CreateFsm<EntityEnemy>(gameObject.name, this, stateList);
            StartFsm();
        }

        private void DestroyFsm()
        {
            GameEntry.Fsm.DestroyFsm(fsm);
            foreach (var item in stateList)
            {
                ReferencePool.Release((IReference)item);
            }

            stateList.Clear();
            fsm = null;
        }

        #endregion
        
        
    }
}