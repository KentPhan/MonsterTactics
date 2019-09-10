using Assets.Scripts.Inventory_System.Items;
using UnityEngine;

namespace Assets.Scripts.CharacterComponents
{
    public class EquipmentComponent : MonoBehaviour
    {
        private IWeapon meleeWeaponSlot;
        private IConsumable consumableSlot;
        public IWeapon MeleeWeapon => meleeWeaponSlot;
        public IConsumable Consumable => consumableSlot;

        public bool EquipWeapon(IWeapon weapon)
        {
            if (this.meleeWeaponSlot != null || weapon == null)
            {
                Debug.LogError("Weapon already equipped or weapon does not exist");
                return false;
            }
                
            this.meleeWeaponSlot = weapon;
            return true;
        }

        public IWeapon UnEquipWeapon()
        {
            IWeapon weaponToReturn = this.meleeWeaponSlot;
            this.meleeWeaponSlot = null;
            return weaponToReturn;
        }

        public bool EquipConsumable(IConsumable consumable)
        {
            if(this.consumableSlot != null || consumable == null)
            {
                Debug.LogError("Consumable already equipped or Consumable does not exist");
                return false;
            }

            this.consumableSlot = consumable;
            return true;
        }

        public IConsumable UnEquipComsumable()
        {
            IConsumable consumableToReturn = this.consumableSlot;
            this.consumableSlot = null;
            return consumableToReturn;
        }
    }
}
