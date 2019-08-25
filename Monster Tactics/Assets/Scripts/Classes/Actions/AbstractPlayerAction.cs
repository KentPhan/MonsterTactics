using System;
using UnityEngine;

namespace Assets.Scripts.Classes.Actions
{
    public abstract class AbstractPlayerAction
    {
        [SerializeField] protected int actionPointCost; public int ActionPointCost => this.actionPointCost;
        protected abstract event EventHandler actionEnded;

        protected AbstractPlayerAction(int actionPointCost)
        {
            this.actionPointCost = actionPointCost;
        }


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
