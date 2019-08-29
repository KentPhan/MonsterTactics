using System;
using Assets.Scripts.Characters;
using UnityEngine;

namespace Assets.Scripts.Classes.Actions
{
    public abstract class AbstractAction
    {
        [SerializeField] protected int actionPointCost; public int ActionPointCost => this.actionPointCost;
        protected abstract event EventHandler actionEnded;

        protected AbstractAction(int actionPointCost)
        {
            this.actionPointCost = actionPointCost;
        }


        public abstract void PlayAction(AbstractCharacter character);


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
