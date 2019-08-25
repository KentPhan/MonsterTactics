using Assets.Scripts.Classes;
using Assets.Scripts.Classes.Actions;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.CharacterComponents
{
    public enum BuildingActionStates
    {
        NONE,
        MOVEMENT_STATE
    }

    public class PlanBuilderComponent : MonoBehaviour
    {
        public PlayerPlan Plan => currentBuiltPlan;


        [SerializeField] [Range(1, 20)] private int actionPointLimit = 6;

        private Player assignedPlayer;
        private PlayerPlan currentBuiltPlan;

        // Start is called before the first frame update
        void Start()
        {
            this.assignedPlayer = this.gameObject.GetComponent<Player>();
            this.currentBuiltPlan = new PlayerPlan(this.assignedPlayer, actionPointLimit);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log($"Mouse position {GridSystem.Instance.clickedSquare}");
            }
        }

        private MovementAction CreateMovementAction(Vector3 destination, int actionPointCost)
        {
            MovementAction o_moveAction = new MovementAction(destination, actionPointCost);
            return o_moveAction;
        }

        public void SubmitPlan()
        {
            this.currentBuiltPlan.FinishPlan();
            //BattleManager.Instance.Advance
        }
    }
}
