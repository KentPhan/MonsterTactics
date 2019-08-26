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
        public bool FinishedPlanning { get; private set; }
        private Queue<AbstractPlayerAction> actionQueue;
        private event EventHandler planEnded;

        private Player targetPlayer;
        private int actionPointLimit;
        public int ActionPointCost
        {
            get
            {
                int totalCost = 0;
                foreach (AbstractPlayerAction action in actionQueue)
                {
                    totalCost += action.ActionPointCost;
                }
                return totalCost;
            }
        }

        public PlayerPlan(Player target, int actionPointLimit)
        {
            this.targetPlayer = target;
            this.actionPointLimit = actionPointLimit;
            this.actionQueue = new Queue<AbstractPlayerAction>();
        }

        public bool AddActionToPlanQueue(AbstractPlayerAction newAction)
        {
            if ((newAction.ActionPointCost + this.ActionPointCost) > this.actionPointLimit)
            {
                return false;
            }
            else
            {
                actionQueue.Enqueue(newAction);
                return true;
            }
        }

        public void RemoveAllActionsFromQueue()
        {
            actionQueue.Clear();
        }

        public void PlayPlan()
        {
            // Play first action
            if (this.FinishedPlanning)
            {
                AbstractPlayerAction action = actionQueue.Dequeue();

                if (action == null)
                {
                    planEnded?.Invoke(this, null);
                    return;
                }

                action.SubscribeToActionEnd(CurrentActionFinished);
                action.PlayAction(this.targetPlayer);
            }
            else
            {
                Debug.LogError("Plan has not been finished to play");
            }
        }

        private void CurrentActionFinished(object sender, EventArgs args)
        {
            ((AbstractPlayerAction)sender).UnsubscribeToActionEnd(CurrentActionFinished);

            AbstractPlayerAction action = (actionQueue.Count <= 0) ? null : actionQueue.Dequeue();
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

        public void FinishPlan()
        {
            this.FinishedPlanning = true;
        }
    }
}
