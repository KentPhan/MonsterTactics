﻿using System.Collections.Generic;
using Assets.Scripts.Inventory_System.Items;
using UnityEngine;

namespace Assets.Scripts.Inventory_System
{
    public class InventorySystem : MonoBehaviour
    {

        public GameObject ConsumableSlot;
        public GameObject WeaponSlot;
        public GameObject SpellSlot;
        
        private static InventorySystem _instance;
        public static InventorySystem Instance { get { return _instance; } }

        private void Awake()
        {
            Debug.Log("Inventory System with name made:" + this.gameObject.name);
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }


        private void OnDestroy()
        {
            Debug.Log("Destroying");
        }
    }
}
