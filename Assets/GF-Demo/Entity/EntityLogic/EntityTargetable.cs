using System;
using UnityEngine;

namespace Demo
{
    public enum EnumAlignment
    {
        None,
        Tower,
        Enemy
    }
    
    /// <summary>
    /// 可瞄准的实体类（血量、攻击）
    /// </summary>
    public abstract class EntityTargetable: EntityLogicEx
    {
        #region 属性
        protected Transform hpBarRoot;

        private Vector3 m_CurrentPosition, m_PreviousPosition;

        //随机的音效
        //private RandomSound randomSound;
        
        /// <summary>
        /// 实体类型
        /// </summary>
        public virtual EnumAlignment Alignment
        {
            get
            {
                return EnumAlignment.None;
            }
        }
        
        protected float hp;
        public float HP
        {
            get
            {
                return hp;
            }

            protected set
            {
                hp = value;
            }
        }
        
        //是否加载血条
        private bool loadedHPBar = false;
        private GameObject entityHPBar;

        public event Action<EntityTargetable> OnDead;
        public event Action<EntityTargetable> OnHidden;

        private Transform effectPointData;
        private Transform deadEffectOffset;
        #endregion
        
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            // randomSound = GetComponent<RandomSound>();
            effectPointData = transform.Find("effectPointData");
            deadEffectOffset = transform.Find("deadEffectOffset");
            hpBarRoot = transform.Find("hpBarRoot");
            // deadEffect = GetComponent<DeadEffect>();
            // hpBarRoot = transform.Find("HealthBar");
        }
        
        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            if (OnHidden != null)
                OnHidden(this);

            OnHidden = null;
            OnDead = null;

            //隐藏血量
            //HideHpBar();
        }
        
        protected virtual void FixedUpdate()
        {
            m_CurrentPosition = transform.position;
            Vector3 Velocity = (m_CurrentPosition - m_PreviousPosition) / Time.fixedDeltaTime;
            m_PreviousPosition = m_CurrentPosition;
        }
        
        public virtual void Damage(float value)
        {
            // if (IsDead)
            //     return;

            
            // if (!loadedHPBar)
            // {
            //     GameEntry.Event.Fire(this, ShowEntityInLevelEventArgs.Create(
            //         (int)EnumEntity.HPBar,
            //         typeof(EntityHPBar),
            //         OnLoadHpBarSuccess,
            //         EntityDataFollower.Create(hpBarRoot)));
            //
            //     loadedHPBar = true;
            // }
            
            hp -= value;

            if (entityHPBar)
            {
                //entityHPBar.UpdateHealth(hp / MaxHP);
            }


            if (hp <= 0)
            {
                hp = 0;
                Dead();
            }
        }
        
        protected virtual void Dead()
        {
            if (OnDead != null)
                OnDead(this);

            // if (deadEffect != null)
            // {
            //     GameEntry.Event.Fire(this, ShowEntityInLevelEventArgs.Create(
            //         (int)deadEffect.deadEffectEntity,
            //         typeof(EntityParticleAutoHide),
            //         null,
            //         EntityDataFollower.Create(randomSound ? randomSound.GetRandomSound() : EnumSound.None, transform.position + DeadEffectOffset, transform.rotation)));
            // }
        }
        
        private void HideHpBar()
        {
            if (entityHPBar)
            {
                //GameEntry.Event.Fire(this, HideEntityInLevelEventArgs.Create(entityHPBar.Id));
                loadedHPBar = false;
                entityHPBar = null;
            }
        }
    }
}