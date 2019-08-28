using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Characters;

namespace Assets.Scripts.Classes.Actions
{
    public class BossAttackAction : AbstractPlayerAction
    {
        private List<Square> attackedSquares;

        public BossAttackAction(List<Square> attackedSquares,int actionPointCost) : base(actionPointCost)
        {
            this.attackedSquares = attackedSquares;
        }

        protected override event EventHandler actionEnded;
        public override void PlayAction(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
