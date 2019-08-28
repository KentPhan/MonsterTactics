using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Inventory_System.Items
{
    public class AxeItem : AbstractItem, IWeapon
    {
        [SerializeField][Range(0,100)] private int attackPower;

        public int GetAttackPower()
        {
            return attackPower;
        }
    }
}
