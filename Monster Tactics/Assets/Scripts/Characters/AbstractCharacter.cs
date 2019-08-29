using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Characters
{
    public abstract class AbstractCharacter: MonoBehaviour
    {
        [SerializeField] [Range(1, 100)] protected int maxHealth;
        [SerializeField] protected UIHealthBar healthBar;
        protected int currentHealth;

        public virtual void Awake()
        {
            this.currentHealth = maxHealth;
        }
        

        public abstract void TakeDamage(int damage);

        protected void UpdateHealthBar()
        {
            if(healthBar != null)
                healthBar.UpdateHealthIndicator((float)this.currentHealth / (float) this.maxHealth);
        }

    }
}
