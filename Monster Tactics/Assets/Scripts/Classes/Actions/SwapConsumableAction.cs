using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Characters;
using System;
using Assets.Scripts.Inventory_System.Items;
using Assets.Scripts.CharacterComponents;

namespace Assets.Scripts.Classes.Actions
{
    public class SwapConsumableAction : AbstractAction
    {
        Player player;
        AbstractItem playerConsumable;
        AbstractItem inventoryConsumable;

        public SwapConsumableAction(ref Player player, int actionPointCost) : base(actionPointCost)
        {
            this.player = player;
        }

        protected override event EventHandler actionEnded;

        public override void PlayAction(AbstractCharacter character)
        {
            if (player.ConsumableSlot.GetComponent<AbstractItem>() != null)
            {
                playerConsumable = player.ConsumableSlot.GetComponent<AbstractItem>();
            }

            if (Inventory_System.InventorySystem.Instance.ConsumableSlot.GetComponent<AbstractItem>() != null)
            {
                inventoryConsumable = Inventory_System.InventorySystem.Instance.ConsumableSlot.GetComponent<AbstractItem>();
            }

            if (playerConsumable != null)
            {
                player.GetComponent<EquipmentComponent>().UnEquipComsumable();
                CopyComponent(playerConsumable, Inventory_System.InventorySystem.Instance.ConsumableSlot);
                playerConsumable.Destroythis();
            }
            if (inventoryConsumable != null)
            {
                player.GetComponent<EquipmentComponent>().EquipConsumable(inventoryConsumable as IConsumable);
                CopyComponent(inventoryConsumable, (player.ConsumableSlot));
                inventoryConsumable.Destroythis();
            }

            actionEnded?.Invoke(this, null);
        }

        Component CopyComponent(Component original, GameObject destination)
        {
            System.Type type = original.GetType();
            Component copy = destination.AddComponent(type);
            // Copied fields can be restricted with BindingFlags
            System.Reflection.FieldInfo[] fields = type.GetFields();
            foreach (System.Reflection.FieldInfo field in fields)
            {
                field.SetValue(copy, field.GetValue(original));
            }
            return copy;
        }
    }
}
