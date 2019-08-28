using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Characters;

namespace Assets.Scripts.Classes.Actions
{
    public class AttackAction : AbstractPlayerAction
    {
        public AttackAction(Square target, int actionPointCost) : base(actionPointCost)
        {
            
        }

        protected override event EventHandler actionEnded;
        public override void PlayAction(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
