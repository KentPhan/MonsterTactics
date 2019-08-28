using System;
using Assets.Scripts.Characters;
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
        CHOOSE_ACTION,
    }

    public class PlanBuilderComponent : MonoBehaviour
    {
        public PlayerPlan Plan => currentBuiltPlan;
        
        private Player assignedPlayer;
        private PlayerPlan currentBuiltPlan;
        private int rayCastMask;
        private BuildingActionStates buildState;
        private Square lastMovingSquare;

        // Start is called before the first frame update
        void Start()
        {
            this.assignedPlayer = this.gameObject.GetComponent<Player>();
            this.currentBuiltPlan = new PlayerPlan(this.assignedPlayer, this.assignedPlayer.ActionPointLimit);
            this.rayCastMask = LayerMask.GetMask(Layers.GRID);
            this.buildState = BuildingActionStates.NONE;


            // TODO will have to adapt this to via which player is active
            CanvasManager.Instance.UIActionPanel.SubscribeToEndPlanningButton(SubmitPlan);
            CanvasManager.Instance.UIActionPanel.SubscribeToCancelPlanningButton(CancelPlan);
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

                            // if you click the player then cancel all the plan, have same effect as click cancel button
                            if (this.assignedPlayer.CurrentSquare == clickedSquare)
                            {
                                this.currentBuiltPlan.RemoveAllActionsFromQueue();
                                this.assignedPlayer.CurrentSquare.Clear();
                                this.buildState = BuildingActionStates.NONE;
                                GridSystem.Instance.resetRootRenderer();

                            // if you clicked same square as the last square you click for planning to move, then open a dialog and do some other actions
                            }else if (lastMovingSquare == clickedSquare)
                            {
                                goto case BuildingActionStates.CHOOSE_ACTION;
                            }
                            // if clicked square falls within player range Queue Action
                            else if (clickedSquare.IsInRange())
                            {
                                if (this.currentBuiltPlan.AddActionToPlanQueue(
                                    CreateMovementAction(clickedSquare, clickedSquare.ActionPointCost())))
                                {
                                    Debug.Log("Action added to queue");

                                    // HighlightRoot
                                    GridSystem.Instance.highlightRoot(assignedPlayer.CurrentSquare,clickedSquare);
                                    // Memorize the latest square the player clicked 
                                    lastMovingSquare = clickedSquare;
                                    // Clear and update range
                                    clickedSquare.Clear();
                                    int newActionsPointLeft = this.assignedPlayer.ActionPointLimit - this.currentBuiltPlan.ActionPointCost;
                                    if(newActionsPointLeft >0)
                                        clickedSquare.Range(newActionsPointLeft);
                                }
                                else
                                {
                                    Debug.Log("Action not added to queue");
                                }
                            }
                            break;
                        case BuildingActionStates.CHOOSE_ACTION:
                            DialogSystem.Instance.TurnOnDialog(Input.mousePosition.x, Input.mousePosition.y);
                            //this.buildState = BuildingActionStates.CHOOSE_MOVEMENT;
                            break;

                        // if outside of range. do nothing
                        default:
                            throw new ArgumentOutOfRangeException();
                    }


                    // Update UI at the end of every action
                    int currentCost = this.currentBuiltPlan.ActionPointCost;
                    CanvasManager.Instance.UIInfoPanel.UpdateActionSpentValue(currentCost);
                    if (currentCost > 0)
                    {
                        CanvasManager.Instance.UIActionPanel.ShowCancelPlanningButton();
                        CanvasManager.Instance.UIActionPanel.ShowEndPlanningButton();
                    }
                    else
                    {
                        CanvasManager.Instance.UIActionPanel.HideCancelPlanningButton();
                        CanvasManager.Instance.UIActionPanel.HideEndPlanningButton();
                    }
                }
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

        public void CancelPlan()
        {
            this.currentBuiltPlan.RemoveAllActionsFromQueue();
            this.assignedPlayer.CurrentSquare.Clear();
            this.buildState = BuildingActionStates.NONE;
            GridSystem.Instance.resetRootRenderer();

            // Update UI at the end of every action
            int currentCost = this.currentBuiltPlan.ActionPointCost;
            CanvasManager.Instance.UIInfoPanel.UpdateActionSpentValue(currentCost);
        }

        public void SubmitPlan()
        {
            this.currentBuiltPlan.FinishPlan();
            this.assignedPlayer.CurrentSquare.Clear();
            this.buildState = BuildingActionStates.NONE;
            CanvasManager.Instance.UIActionPanel.HideEndPlanningButton();
            BattleManager.Instance.AdvanceFromPlayerPlanning();
            GridSystem.Instance.resetRootRenderer();
        }
    }
}
