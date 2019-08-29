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

        private Vector3 fixedPosition = Vector3.zero;
        private Vector3 fixedRotation = Vector3.zero;
        private Vector3 fixedScale = new Vector3(1,10,1);

        private void Awake()
        {
            base.material = Resources.Load("Materials/Eye", typeof(Material)) as Material;
            base.mesh = Resources.Load("Meshes/Sword", typeof(Mesh)) as Mesh;


            transform.localPosition = fixedPosition;
            transform.localEulerAngles = fixedRotation;
            transform.localScale = fixedScale;
        }

        public int GetAttackPower()
        {
            return attackPower;
        }
    }
}
