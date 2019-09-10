using System.Collections.Generic;
using Assets.Scripts.Characters;
using Assets.Scripts.Classes;
using Assets.Scripts.Classes.Actions;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.CharacterComponents
{
    public class BossPlanBuilderComponent : MonoBehaviour
    {
        public Plan Plan => currentBuiltPlan;

        private Boss assignedBoss; 

        private Plan currentBuiltPlan;

        private List<Square> currentAttackSquares;

        void Awake()
        {
            this.assignedBoss = this.gameObject.GetComponent<Boss>();
            this.currentBuiltPlan = new Plan(this.assignedBoss, this.assignedBoss.ActionPointLimit);
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                ShowAttackZone();
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                HideAttackZone();
            }


        }

        public void BuildBossPlan()
        {
            this.currentAttackSquares = GridSystem.Instance.GetRandomListOfSquares(0.5f);
            this.currentBuiltPlan.AddActionToPlanQueue(new BossAttackAction(this.currentAttackSquares, 10));
            this.currentBuiltPlan.FinishPlan();
            BattleManager.Instance.AdvanceFromBossPlanning();
        }

        public void ShowAttackZone()
        {
            if (this.currentAttackSquares?.Count > 0)
            {
                foreach (Square square in this.currentAttackSquares)
                {
                    square.SetBossAttackZone();
                }
            }
        }

        public void HideAttackZone()
        {
            if (this.currentAttackSquares?.Count > 0)
            {
                foreach (Square square in this.currentAttackSquares)
                {
                    square.ResetColorToState();
                }
            }
        }
    }
}
