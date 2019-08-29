using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.CharacterComponents;
using Assets.Scripts.Characters;

namespace Assets.Scripts.Classes.Actions
{
    public class BossAttackAction : AbstractAction
    {
        private List<Square> attackedSquares;

        public BossAttackAction(List<Square> attackedSquares,int actionPointCost) : base(actionPointCost)
        {
            this.attackedSquares = attackedSquares;
        }

        protected override event EventHandler actionEnded;
        public override void PlayAction(AbstractCharacter character)
        {
            List<Square> squares = GridSystem.Instance.GetRandomListOfSquares(0.5f);
            BossAttackComponent attackComponent = character.GetComponent<BossAttackComponent>();
            attackComponent.OnFinishedAction += OnAttackFinished;
            attackComponent.SuperAttackSquares(squares);
        }

        private void OnAttackFinished(object sender, EventArgs args)
        {
            ((BossAttackComponent)sender).OnFinishedAction -= OnAttackFinished;
            actionEnded?.Invoke(this, new EventArgs());
        }
    }
}
