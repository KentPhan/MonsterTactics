using System;
using Assets.Scripts.Classes.Actions;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.CharacterComponents
{
    public class AttackComponent : MonoBehaviour, IActionable
    {
        private EquipmentComponent equipmentComponent;

        // Start is called before the first frame update
        void Start()
        {
            this.equipmentComponent = transform.GetComponent<EquipmentComponent>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AttackSquare(Square target)
        {
            int baseAttackPower = 1;
            if (this.equipmentComponent != null && this.equipmentComponent.MeleeWeapon != null)
                baseAttackPower = this.equipmentComponent.MeleeWeapon.GetAttackPower();

            if (!target.IsTraversable())
            {
                BattleManager.Instance.GetBoss().TakeDamage(baseAttackPower);
            }

            GetComponent<Animator>().SetTrigger("attack");
        }

        public event EventHandler OnFinishedAction;
    }
}
