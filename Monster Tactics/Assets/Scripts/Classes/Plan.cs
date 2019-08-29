using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Characters;
using Assets.Scripts.Classes.Actions;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    public class Plan
    {
        public bool FinishedPlanning { get; private set; }
        private AbstractAction lastQueued;
        private Queue<AbstractAction> actionQueue;
        private event EventHandler planEnded;

        private AbstractCharacter targetCharacter;
        private int actionPointLimit;
        public int ActionPointCost
        {
            get
            {
                int totalCost = 0;
                foreach (AbstractAction action in actionQueue)
                {
                    totalCost += action.ActionPointCost;
                }
                return totalCost;
            }
        }

        public Plan(Player target, int actionPointLimit)
        {
            this.targetCharacter = target;
            this.actionPointLimit = actionPointLimit;
            this.actionQueue = new Queue<AbstractAction>();
        }

        public bool AddActionToPlanQueue(AbstractAction newAction)
        {
            if ((newAction.ActionPointCost + this.ActionPointCost) > this.actionPointLimit)
            {
                return false;
            }
            else
            {
                lastQueued = newAction;
                actionQueue.Enqueue(newAction);
                return true;
            }
        }

        public AbstractAction GetLastestActionFromPlanQueue()
        {
            if (lastQueued != null)
            {
                return lastQueued;
            }
            return null;
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
                AbstractAction action = actionQueue.Dequeue();

                if (action == null)
                {
                    planEnded?.Invoke(this, null);
                    return;
                }

                action.SubscribeToActionEnd(CurrentActionFinished);
                action.PlayAction(this.targetCharacter);
            }
            else
            {
                Debug.LogError("Plan has not been finished to play");
            }
        }

        private void CurrentActionFinished(object sender, EventArgs args)
        {
            ((AbstractAction)sender).UnsubscribeToActionEnd(CurrentActionFinished);

            AbstractAction action = (actionQueue.Count <= 0) ? null : actionQueue.Dequeue();
            if (action == null)
            {
                this.FinishedPlanning = false;
                planEnded?.Invoke(this, null);
                return;
            }

            action.SubscribeToActionEnd(CurrentActionFinished);
            action.PlayAction(this.targetCharacter);
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
