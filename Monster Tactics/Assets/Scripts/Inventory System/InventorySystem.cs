using System.Collections.Generic;
using Assets.Scripts.Inventory_System.Items;
using UnityEngine;

namespace Assets.Scripts.Inventory_System
{
    public class InventorySystem : MonoBehaviour
    {
        private IWeapon meleeSlot;
        public IWeapon MeleeSlot => meleeSlot;
        
        private static InventorySystem _instance;
        public static InventorySystem Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        private void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public bool StoreMeleeWeapon(IWeapon weapon)
        {
            if (weapon == null || this.meleeSlot != null)
                return false;
            this.meleeSlot = weapon;
            return true;
        }

        public IWeapon WithdrawMeleeWeapon()
        {
            IWeapon weaponToReturn = this.meleeSlot;
            this.meleeSlot = null;
            return weaponToReturn;
        }
    }
}
