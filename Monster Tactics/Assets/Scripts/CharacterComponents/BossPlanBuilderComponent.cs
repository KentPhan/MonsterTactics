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

        // Start is called before the first frame update
        void Start()
        {
            this.assignedBoss = this.gameObject.GetComponent<Boss>();
            this.currentBuiltPlan = new Plan(this.assignedBoss, this.assignedBoss.ActionPointLimit);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (this.currentAttackSquares?.Count > 0)
                {
                    foreach (Square square in this.currentAttackSquares)
                    {

                    }
                }
            }


        }

        public void BuildBossPlan()
        {
            this.currentAttackSquares = GridSystem.Instance.GetRandomListOfSquares(0.5f);
            this.currentBuiltPlan.AddActionToPlanQueue(new BossAttackAction(this.currentAttackSquares, 10));
            this.currentBuiltPlan.FinishPlan();
            BattleManager.Instance.AdvanceFromBossPlanning();
        }
    }
}
