using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Classes.Actions;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    public class PlayerPlan
    {
        public bool Planned { get; set; }
        private Queue<AbstractPlayerAction> actionQueue;
        private event EventHandler planEnded;

        private Player targetPlayer;
        public PlayerPlan(Player target)
        {
            this.targetPlayer = target;
        }

        public void AddActionToPlanQueue(AbstractPlayerAction newAction)
        {
            actionQueue.Enqueue(newAction);
        }

        public void RemoveAllActionsFromQueue()
        {
            actionQueue.Clear();
        }

        public void PlayPlan()
        {
            // Play first action
            AbstractPlayerAction action = actionQueue.Dequeue();

            if (action == null)
            {
                planEnded?.Invoke(this, null);
                return;
            }

            action.SubscribeToActionEnd(CurrentActionFinished);
            action.PlayAction(this.targetPlayer);
        }

        private void CurrentActionFinished(object sender, EventArgs args)
        {
            ((AbstractPlayerAction)sender).UnsubscribeToActionEnd(CurrentActionFinished);

            AbstractPlayerAction action = actionQueue.Dequeue();
            if (action == null)
            {
                planEnded?.Invoke(this, null);
                return;
            }

            action.SubscribeToActionEnd(CurrentActionFinished);
            action.PlayAction(this.targetPlayer);
        }

        public void SubscribeToPlanEnd(EventHandler callback)
        {
            planEnded += callback;
        }

        public void UnsubscribeToPlanEnd(EventHandler callback)
        {
            planEnded -= callback;
        }
    }
}
