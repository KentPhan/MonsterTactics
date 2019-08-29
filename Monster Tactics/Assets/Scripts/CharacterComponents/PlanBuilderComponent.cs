using System;
using Assets.Scripts.Characters;
using Assets.Scripts.Classes;
using Assets.Scripts.Classes.Actions;
using Assets.Scripts.Constants;
using Assets.Scripts.Managers;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.CharacterComponents
{
    public enum BuildingActionStates
    {
        NONE,
        CHOOSE_MOVEMENT,
        CHOOSE_ACTION
    }

    public class PlanBuilderComponent : MonoBehaviour
    {
        public Plan Plan => currentBuiltPlan;

        private Player assignedPlayer;
        private Plan currentBuiltPlan;
        private int rayCastMask;
        private BuildingActionStates buildState;
        private Square lastMovingSquare;
        private Square attackTargetedSquare; //Hack


        public void Awake()
        {
            this.enabled = false;
            this.assignedPlayer = this.gameObject.GetComponent<Player>();
            this.currentBuiltPlan = new Plan(this.assignedPlayer, this.assignedPlayer.ActionPointLimit);
            this.rayCastMask = LayerMask.GetMask(Layers.GRID);
            this.buildState = BuildingActionStates.NONE;
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        public void EnableAsPlanningPlayer()
        {
            // TODO will have to adapt this to via which player is active
            this.enabled = true;
            CanvasManager.Instance.UIActionPanel.SubscribeToEndPlanningButton(SubmitPlan);
            CanvasManager.Instance.UIActionPanel.SubscribeToCancelPlanningButton(CancelPlan);
            DialogUI.ActionButtonClicked += AddActionToQueue;
        }

        public void DisableAsPlanningPlayer()
        {
            this.enabled = false;
            CanvasManager.Instance?.UIActionPanel.UnsubscribeToEndPlanningButton(SubmitPlan);
            CanvasManager.Instance?.UIActionPanel.UnsubscribeToCancelPlanningButton(CancelPlan);
            DialogUI.ActionButtonClicked -= AddActionToQueue;
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
                                CancelPlan();
                            }
                            else if (lastMovingSquare == clickedSquare)
                            {
                                goto case BuildingActionStates.CHOOSE_ACTION;
                            }
                            // if clicked square falls within player range Queue Action
                            else if (clickedSquare.IsInRange())
                            {
                                if (this.currentBuiltPlan.AddActionToPlanQueue(
                                    CreateMovementAction(clickedSquare, clickedSquare.ActionPointCost())))
                                {
                                    // HighlightRoot
                                    GridSystem.Instance.highlightRoot(assignedPlayer.CurrentSquare, clickedSquare);
                                    // Memorize the latest square the player clicked 
                                    lastMovingSquare = clickedSquare;

                                    // Clear and update range
                                    UpdateMovingSquareRange(this.lastMovingSquare);

                                    // If there is any dialog was opened then closed it
                                    DialogSystem.Instance.TurnOffDialog();
                                }
                                else
                                {
                                    Debug.Log("Action not added to queue");
                                }
                            }
                            break;
                        case BuildingActionStates.CHOOSE_ACTION:
                            List<Actions> sendingactions = new List<Actions>();

                            if (actionPointsLeft <= 0)
                            {
                                this.buildState = BuildingActionStates.CHOOSE_MOVEMENT;
                                break;
                            }

                            sendingactions.Add(Actions.SwapWeapon);

                            // Add Pickups if applicable
                            if (clickedSquare.hasItemOnThis() && this.lastMovingSquare == clickedSquare)
                            {
                                sendingactions.Add(Actions.PickUpAndEquip);
                                sendingactions.Add(Actions.PickUpAndStore);
                            }

                            // Add Attack if applicable
                            if ((this.attackTargetedSquare = clickedSquare.getNearestAttackableZone()) != null && this.lastMovingSquare == clickedSquare)
                            {
                                sendingactions.Add(Actions.Attack);
                            }

                            // Display actions
                            if (sendingactions.Count > 0)
                            {
                                sendingactions.Add(Actions.Cancel);
                                DialogSystem.Instance.SendActionList(sendingactions);
                                DialogSystem.Instance.TurnOnDialog(Input.mousePosition.x, Input.mousePosition.y);
                            }
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
            this.lastMovingSquare?.Clear();
            this.buildState = BuildingActionStates.NONE;
            this.lastMovingSquare = null;
            GridSystem.Instance.resetRootRenderer();
            DialogSystem.Instance.TurnOffDialog();

            // Update UI at the end of every action
            int currentCost = this.currentBuiltPlan.ActionPointCost;
            CanvasManager.Instance.UIInfoPanel.UpdateActionSpentValue(currentCost);
        }

        public void SubmitPlan()
        {
            this.currentBuiltPlan.FinishPlan();
            this.assignedPlayer.CurrentSquare.Clear();
            this.lastMovingSquare.Clear();
            this.buildState = BuildingActionStates.NONE;
            CanvasManager.Instance.UIActionPanel.HideEndPlanningButton();
            CanvasManager.Instance.UIActionPanel.HideCancelPlanningButton();
            BattleManager.Instance.AdvanceFromPlayerPlanning();
            GridSystem.Instance.resetRootRenderer();
        }

        public void AddActionToQueue(Actions action)
        {
            switch (action)
            {
                case Actions.SwapWeapon:
                    Debug.Log("SwapWeapon!");
                    this.currentBuiltPlan.AddActionToPlanQueue(new SwapWeaponAction(ref assignedPlayer,1));
                    break;
                case Actions.PickUpAndEquip:
                    Debug.Log("PickUpAndEquip");
                    this.currentBuiltPlan.AddActionToPlanQueue(new PickUpAndEquipAction(ref lastMovingSquare,1));
                    break;
                case Actions.PickUpAndStore:
                    this.currentBuiltPlan.AddActionToPlanQueue(new PickUpAndStoreAction(ref lastMovingSquare, 1));
                    break;
                case Actions.Attack:
                    this.currentBuiltPlan.AddActionToPlanQueue(new AttackAction(this.attackTargetedSquare, 1));
                    this.attackTargetedSquare = null;
                    break;
                case Actions.Cancel:
                    this.buildState = BuildingActionStates.CHOOSE_MOVEMENT;
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
            this.buildState = BuildingActionStates.CHOOSE_MOVEMENT;

            // Update Range indicator
            UpdateMovingSquareRange(this.lastMovingSquare);


            // Update UI
            int currentCost = this.currentBuiltPlan.ActionPointCost;
            CanvasManager.Instance.UIInfoPanel.UpdateActionSpentValue(currentCost);
        }

        private void UpdateMovingSquareRange(Square squareToUpdate)
        {
            squareToUpdate.Clear();
            int newActionsPointLeft = this.assignedPlayer.ActionPointLimit - this.currentBuiltPlan.ActionPointCost;
            if (newActionsPointLeft > 0)
                squareToUpdate.Range(newActionsPointLeft);
        }
    }
}
