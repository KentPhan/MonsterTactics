using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Characters;

namespace Assets.Scripts.Inventory_System.Items
{
    public class Position : AbstractItem, IConsumable
    {

        private void Awake()
        {
            base.material = Resources.Load("Materials/White", typeof(Material)) as Material;
            base.mesh = Resources.Load("Meshes/Potion", typeof(Mesh)) as Mesh;
            base.sprite = Resources.Load("Sprites/PotionIcon", typeof(Sprite)) as Sprite;

            base.fixedPosition[0] = new Vector3(0, 2, 0);
            base.fixedRotation[0] = new Vector3(30, 0, 0);
            base.fixedScale[0] = new Vector3(.7f, .7f, .7f);

            base.fixedPosition[1] = new Vector3(-3.23f, -4.85f, -3.14f);
            base.fixedRotation[1] = new Vector3(0, -41.18f, -7.55f);
            base.fixedScale[1] = new Vector3(.5f, .5f, .5f);
        }

        // Update is called once per frame
        void Update()
        {
            if (state == ItemState.OnField)
                transform.Rotate(Vector3.up * Time.deltaTime * 100, Space.World);
        }

        void IConsumable.UseItem()
        {
            throw new System.NotImplementedException();
        }

        public void UseItem(Player user)
        {
            user.Heal(1);
            base.Destroythis();
        }
    }
}
