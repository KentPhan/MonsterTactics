using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.CharacterComponents;
using Assets.Scripts.Characters;

namespace Assets.Scripts.Classes.Actions
{
    public class AttackAction : AbstractPlayerAction
    {
        private Square target;

        public AttackAction(Square target, int actionPointCost) : base(actionPointCost)
        {
            this.target = target;
        }

        protected override event EventHandler actionEnded;
        public override void PlayAction(Player player)
        {
            AttackComponent attackComponent = player.GetComponent<AttackComponent>();
            attackComponent.AttackSquare(target);
            actionEnded?.Invoke(this,null);
        }
    }
}
