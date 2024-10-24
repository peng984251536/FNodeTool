using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using System;
using GameFramework.Event;
using GameFramework;

namespace Demo
{
    /// <summary>
    /// 实体的逻辑抽象类
    /// </summary>
    public abstract class EntityLogicEx : EntityLogicWithData
    {
        private EventSubscriber eventSubscriber;

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            UnSubscribeAll();
            if (eventSubscriber != null)
            {
                ReferencePool.Release(eventSubscriber);
                eventSubscriber = null;
            }

            // HideAllEntity();
            // if (entityLoader != null)
            // {
            //     ReferencePool.Release(entityLoader);
            //     entityLoader = null;
            // }
            //
            // HideAllItem();
            // if (itemLoader != null)
            // {
            //     ReferencePool.Release(itemLoader);
            //     itemLoader = null;
            // }
        }
        protected void Subscribe(int id, EventHandler<GameEventArgs> handler)
        {
            if (eventSubscriber == null)
                eventSubscriber = EventSubscriber.Create(this);

            eventSubscriber.Subscribe(id, handler);
        }

        protected void UnSubscribe(int id, EventHandler<GameEventArgs> handler)
        {
            if (eventSubscriber != null)
                eventSubscriber.UnSubscribe(id, handler);
        }

        protected void UnSubscribeAll()
        {
            if (eventSubscriber != null)
                eventSubscriber.UnSubscribeAll();
        }
        
    }
}
