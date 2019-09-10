using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.CharacterComponents;
using Assets.Scripts.Characters;

using Assets.Scripts.Inventory_System.Items;

namespace Assets.Scripts.Classes.Actions
{
    public class UseAction : AbstractAction
    {
        private AbstractItem item;

        public UseAction(int actionPointCost) : base(actionPointCost)
        {
        }

        protected override event EventHandler actionEnded;
        public override void PlayAction(AbstractCharacter character)
        {
            Player player = character as Player;
            item = player.ConsumableSlot.GetComponent<AbstractItem>();
            if (item is Position)
            {
                Position consumable = item as Position;
                consumable.UseItem(player);
            }
        }
    }
}