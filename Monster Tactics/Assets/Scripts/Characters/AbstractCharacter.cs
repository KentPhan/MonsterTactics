using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Characters
{
    public abstract class AbstractCharacter: MonoBehaviour
    {

        public abstract void TakeDamage(int damage);
    }
}
