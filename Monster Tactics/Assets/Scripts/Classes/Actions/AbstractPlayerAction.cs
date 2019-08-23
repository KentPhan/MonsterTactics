using System;
using UnityEngine;

namespace Assets.Scripts.Classes.Actions
{
    public abstract class AbstractPlayerAction 
    {
        [SerializeField] protected int ActionPointCost;
        protected abstract event EventHandler actionEnded;

        
        public abstract void PlayAction(Player player);
        
        
        public void SubscribeToActionEnd(EventHandler inputEvent)
        {
            actionEnded += inputEvent;
        }

        public void UnsubscribeToActionEnd(EventHandler inputEvent)
        {
            actionEnded -= inputEvent;
        }

    }
}
