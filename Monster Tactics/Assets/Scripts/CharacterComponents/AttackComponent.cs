using System;
using Assets.Scripts.Classes.Actions;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.CharacterComponents
{
    public class AttackComponent : MonoBehaviour, IActionable
    {
        [SerializeField] AudioClip attack;
        AudioSource audioSource;
        private EquipmentComponent equipmentComponent;

        private void Awake()
        {
            this.equipmentComponent = transform.GetComponent<EquipmentComponent>();
            this.audioSource = GetComponent<AudioSource>();
        }

        // Start is called before the first frame update
        void Start()
        {
            
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
            audioSource.PlayOneShot(attack);
            if (!target.IsTraversable())
            {
                BattleManager.Instance.GetBoss().TakeDamage(baseAttackPower);
            }

            GetComponent<Animator>().SetTrigger("attack");
        }

        public event EventHandler OnFinishedAction;
    }
}
