using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Classes.Actions;
using Assets.Scripts.Characters;
using System;
using Assets.Scripts.Inventory_System.Items;

namespace Assets.Scripts.Classes.Actions {
    public class PickUpAndEquipAction : AbstractPlayerAction
    {
        private AbstractItem item;

        public PickUpAndEquipAction(ref Square itemSquare, int actionPointCost) : base(actionPointCost)
        {
            GameObject itemslotforgrid =  itemSquare.gameObject.transform.GetChild(1).gameObject;
            item = itemslotforgrid.GetComponent<AbstractItem>();
        }

        protected override event EventHandler actionEnded;

        public override void PlayAction(Player player)
        {
            item.Destroythis();
            actionEnded?.Invoke(this, null);
        }
    }
}
