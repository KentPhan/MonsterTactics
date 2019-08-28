using System;
using System.Collections.Generic;
using Assets.Scripts.CharacterComponents;
using Assets.Scripts.Characters;
using Assets.Scripts.Classes;
using Cinemachine;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public enum BattleStates
    {
        START_BATTLE, // Battle Start

        // Main battle loop here
        PLAYER_PLAN, // Player Planning phase (where player takes control)
        PLAYER_ACTION,// Playing the player's action
        BOSS_ACTION, // Boss Action
        RESOLUTION, // For determining who is dead as well as round information

        END_BATTLE // Battle End
    }



    public class BattleManager : MonoBehaviour
    {
        [Serializable]
        private struct PlayerParts
        {
            public Player Player;
            public CinemachineVirtualCamera VCamera;
        }

        // Unity Exposed Fields


        // Member Properties
        [SerializeField] private List<PlayerParts> players;
        [SerializeField] private Boss boss;
        private int currentPlayerIndex;

        private BattleStates currentBattleState; public BattleStates CurrentBattleState => currentBattleState;

        private static BattleManager _instance;
        public static BattleManager Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                else
                {
                    Debug.LogError("No Game Manager Found.");
                    return null;
                }
            }
        }

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else if (_instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        // Start is called before the first frame update
        private void Start()
        {
            this.currentBattleState = BattleStates.START_BATTLE;
            this.currentPlayerIndex = 0;
            AdvanceState();
        }

        // Update is called once per frame
        private void Update()
        {

            switch (currentBattleState)
            {
                case BattleStates.START_BATTLE:
                    break;
                case BattleStates.PLAYER_PLAN:
                    break;
                case BattleStates.PLAYER_ACTION:
                    break;
                case BattleStates.BOSS_ACTION:
                    break;
                case BattleStates.RESOLUTION:
                    break;
                case BattleStates.END_BATTLE:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AdvanceState()
        {
            switch (currentBattleState)
            {
                case BattleStates.START_BATTLE: // Input Advance
                    this.currentBattleState = BattleStates.PLAYER_PLAN;
                    this.players[0].Player.GetComponent<PlanBuilderComponent>().EnableAsPlanningPlayer();
                    PrioritizeCamera(this.players[0]);
                    break;
                case BattleStates.PLAYER_PLAN: // Input Advance

                    // Advance state if all players finish planning their turns
                    bool playerNotPlanned = false;
                    foreach (var player in players)
                    {
                        PlanBuilderComponent plan = player.Player.GetComponent<PlanBuilderComponent>();
                        if (!plan.Plan.FinishedPlanning)
                        {
                            playerNotPlanned = true;
                            PrioritizeActivePlayer(player);
                            PrioritizeCamera(player);
                            break;
                        }
                    }
                    if (!playerNotPlanned)
                    {
                        this.currentBattleState = BattleStates.PLAYER_ACTION;
                        AdvanceState();
                    }
                    break;
                case BattleStates.PLAYER_ACTION:// Auto Advance
                    // Play next plan
                    if (currentPlayerIndex < players.Count)
                    {
                        PlayerPlan currentPlan = this.players[currentPlayerIndex].Player.GetComponent<PlanBuilderComponent>().Plan;
                        currentPlan.SubscribeToPlanEnd(OnPlayerPlanFinished);
                        currentPlan.PlayPlan();
                    }
                    // else all plans played, move to next state
                    else
                    {
                        this.currentBattleState = BattleStates.BOSS_ACTION;
                        this.currentPlayerIndex = 0;
                        // TODO build Boss Callback
                        AdvanceState();
                    }

                    break;
                case BattleStates.BOSS_ACTION:
                    this.currentBattleState = BattleStates.RESOLUTION;

                    // TODO Temp
                    AdvanceState();
                    break;
                case BattleStates.RESOLUTION:
                    this.currentBattleState = BattleStates.PLAYER_PLAN;
                    PrioritizeActivePlayer(players[0]);
                    PrioritizeCamera(players[0]);

                    break;
                case BattleStates.END_BATTLE:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnPlayerPlanFinished(object sender, EventArgs args)
        {
            ((PlayerPlan)sender).UnsubscribeToPlanEnd(OnPlayerPlanFinished);
            this.currentPlayerIndex++;
            AdvanceState();
        }

        private void OnBossActionFinished()
        {
            AdvanceState();
        }

        public void StartBattle()
        {

        }

        public void AdvanceFromPlayerPlanning()
        {
            AdvanceState();
        }

        public Boss GetBoss()
        {
            return this.boss;
        }

        private void PrioritizeCamera(PlayerParts player)
        {
            foreach (PlayerParts person in this.players)
            {
                if (person.Player == player.Player)
                {
                    person.VCamera.Priority = 1;
                }
                else
                {
                    person.VCamera.Priority = 0;
                }
            }
        }

        private void PrioritizeActivePlayer(PlayerParts player)
        {

            foreach (PlayerParts person in this.players)
            {
                PlanBuilderComponent plan = person.Player.GetComponent<PlanBuilderComponent>();
                if (person.Player == player.Player)
                {
                    plan.EnableAsPlanningPlayer();
                }
                else
                {
                    plan.DisableAsPlanningPlayer();
                }
            }
        }


        public void EndBattle()
        {

        }
    }
}
