using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.CharacterComponents;
using Assets.Scripts.Characters;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Classes.Actions
{
    public class MovementAction : AbstractPlayerAction
    {
        protected override event EventHandler actionEnded;

        private Square destination;
        public MovementAction(Square destination, int actionPointCost) : base(actionPointCost)
        {
            this.destination = destination;
        }

        public override void PlayAction(Player player)
        {
            MovementComponent movementComponent = player.GetComponent<MovementComponent>();
            movementComponent.OnFinishedAction += OnMovementFinished;
            movementComponent.Move(this.destination);
        }

        private void OnMovementFinished(object sender, EventArgs args)
        {
            ((MovementComponent)sender).OnFinishedAction -= OnMovementFinished;
            actionEnded?.Invoke(this, new EventArgs());
        }
    }
}
