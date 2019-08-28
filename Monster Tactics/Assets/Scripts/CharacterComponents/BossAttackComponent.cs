using System;
using System.Collections.Generic;
using Assets.Scripts.Classes.Actions;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.CharacterComponents
{
    public class BossAttackComponent : MonoBehaviour, IActionable
    {
        [SerializeField][Range(0,100)]private int baseDamage;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {

            }
        }

        public void SuperAttackSquares(List<Square> targets)
        {
            
        }


        public event EventHandler OnFinishedAction;
    }
}
