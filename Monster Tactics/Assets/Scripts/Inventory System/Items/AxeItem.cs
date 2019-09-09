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
        [SerializeField][Range(0,100)] private int attackPower = 2;

        private void Awake()
        {
            base.material = Resources.Load("Materials/Eye", typeof(Material)) as Material;
            base.mesh = Resources.Load("Meshes/Sword", typeof(Mesh)) as Mesh;
            base.sprite = Resources.Load("Sprites/SwordIcon", typeof(Sprite)) as Sprite;


            base.fixedPosition[0] = new Vector3(0, 6, 0);
            base.fixedRotation[0] = new Vector3(45, 0, 0);
            base.fixedScale[0] = new Vector3(15, 15, 10);

            base.fixedPosition[1] = new Vector3(4, 4, 0);
            base.fixedRotation[1] = new Vector3(-90, 0, 0);
            base.fixedScale[1] = new Vector3(15, 15, 5);
        }

        public int GetAttackPower()
        {
            return attackPower;
        }
    }
}
