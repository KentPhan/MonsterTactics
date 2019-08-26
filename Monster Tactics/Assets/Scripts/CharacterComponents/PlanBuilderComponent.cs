using System;
using Assets.Scripts.Classes;
using Assets.Scripts.Classes.Actions;
using Assets.Scripts.Constants;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.CharacterComponents
{
    public enum BuildingActionStates
    {
        NONE,
        CHOOSE_MOVEMENT,
    }

    public class PlanBuilderComponent : MonoBehaviour
    {
        public PlayerPlan Plan => currentBuiltPlan;




        private Player assignedPlayer;
        private PlayerPlan currentBuiltPlan;
        private int rayCastMask;
        private BuildingActionStates buildState;

        // Start is called before the first frame update
        void Start()
        {
            this.assignedPlayer = this.gameObject.GetComponent<Player>();
            this.currentBuiltPlan = new PlayerPlan(this.assignedPlayer, this.assignedPlayer.ActionPointLimit);
            this.rayCastMask = LayerMask.GetMask(Layers.GRID);
            this.buildState = BuildingActionStates.NONE;


            // TODO will have to adapt this to via which player is active
            CanvasManager.Instance.UIActionPanel.SubscribeToEndPlanningButton(SubmitPlan);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Raycast detect square click
                Ray ray = this.assignedPlayer.PlayerCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo, 500, this.rayCastMask, QueryTriggerInteraction.UseGlobal))
                {
                    Square clickedSquare = hitInfo.transform.GetComponent<Square>();

                    if (clickedSquare == null)
                    {
                        Debug.Log("Non square detected");
                        return;
                    }

                    int actionPointsLeft = this.assignedPlayer.ActionPointLimit - this.currentBuiltPlan.ActionPointCost;
                    switch (this.buildState)
                    {
                        case BuildingActionStates.NONE:
                            // If the square clicked on is the player's square. Show Range
                            if (this.assignedPlayer.CurrentSquare == clickedSquare)
                            {
                                // If can move
                                if (actionPointsLeft > 0)
                                {
                                    this.assignedPlayer.CurrentSquare.Range(actionPointsLeft);
                                    this.buildState = BuildingActionStates.CHOOSE_MOVEMENT;
                                }
                            }
                            break;
                        case BuildingActionStates.CHOOSE_MOVEMENT:

                            // if clicked square is the same square the player is on. Cancel plan
                            if (this.assignedPlayer.CurrentSquare == clickedSquare)
                            {
                                this.currentBuiltPlan.RemoveAllActionsFromQueue();
                                this.assignedPlayer.CurrentSquare.Clear();
                                this.buildState = BuildingActionStates.NONE;
                            }
                            // if clicked square falls within player range Queue Action
                            // TODO hook up UI external subscribing as well as detecting what exists on the square here to determine allocated
                            // action
                            else if (this.assignedPlayer.CurrentSquare.IsInRange(clickedSquare, actionPointsLeft))
                            {
                                if (this.currentBuiltPlan.AddActionToPlanQueue(
                                    CreateMovementAction(clickedSquare, 1)))
                                {
                                    Debug.Log("Action added to queue");
                                    // Clear and update range
                                    clickedSquare.Clear();
                                    clickedSquare.Range(this.assignedPlayer.ActionPointLimit -
                                                        this.currentBuiltPlan.ActionPointCost);
                                }
                                else
                                {
                                    Debug.Log("Action not added to queue");
                                }
                            }

                            // if outside of range. do nothing

                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }


                    // Update UI at the end of every action
                    int currentCost = this.currentBuiltPlan.ActionPointCost;
                    CanvasManager.Instance.UIInfoPanel.UpdateActionSpentValue(currentCost);
                    if (currentCost > 0)
                    {
                        CanvasManager.Instance.UIActionPanel.ShowActions();
                    }
                    else
                    {
                        CanvasManager.Instance.UIActionPanel.HideActions();
                    }
                }
            }

            if (Input.GetButtonDown(Inputs.SUBMIT))
            {
                SubmitPlan();
            }

            //
            //if (GridSystem.Instance.clickedSquare)
            //    GridSystem.Instance.clickedSquare.Range(range);
        }

        private MovementAction CreateMovementAction(Square destinationSquare, int actionPointCost)
        {
            MovementAction o_moveAction = new MovementAction(destinationSquare, actionPointCost);
            return o_moveAction;
        }

        public void SubmitPlan()
        {
            this.currentBuiltPlan.FinishPlan();
            CanvasManager.Instance.UIActionPanel.HideActions();
            BattleManager.Instance.AdvanceFromPlayerPlanning();
        }
    }
}
