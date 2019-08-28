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

        private void Awake()
        {
            base.material = Resources.Load("Materials/Eye", typeof(Material)) as Material;
            base.mesh = Resources.Load("Meshes/Rock", typeof(Mesh)) as Mesh;
        }

        public int GetAttackPower()
        {
            return attackPower;
        }
    }
}
