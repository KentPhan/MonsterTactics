using Assets.Scripts.Inventory_System.Items;
using UnityEngine;

namespace Assets.Scripts.CharacterComponents
{
    public class EquipmentComponent : MonoBehaviour
    {
        private IWeapon meleeWeaponSlot;
        public IWeapon MeleeWeapon => meleeWeaponSlot;

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
    }
}
