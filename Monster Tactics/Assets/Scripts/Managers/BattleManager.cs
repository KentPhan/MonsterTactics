using System;
using System.Collections.Generic;
using System.Linq;
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
        BOSS_PLAN,
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
            this.currentPlayerIndex = 0;
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SwapCamera();
            }

            switch (currentBattleState)
            {
                case BattleStates.START_BATTLE:
                    break;
                case BattleStates.BOSS_PLAN:
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

        private void GoToState(BattleStates newBattleState)
        {
            this.currentBattleState = newBattleState;

            // Advance State to new State, and handle start of new state
            switch (currentBattleState)
            {
                case BattleStates.START_BATTLE: 
                    break;
                // Start of Round
                case BattleStates.BOSS_PLAN:
                    this.boss.GetComponent<BossPlanBuilderComponent>().BuildBossPlan();
                    break;
                case BattleStates.PLAYER_PLAN:
                    // Enable first player
                    PrioritizeActivePlayer(players[0]);
                    PrioritizeCamera(this.players[0]);
                    break;
                case BattleStates.PLAYER_ACTION:
                    PlayNextPlayerAction();
                    break;
                case BattleStates.BOSS_ACTION:
                    PlayBossAction();
                    break;
                case BattleStates.RESOLUTION:
                    GoToRoundStart();
                    break;
                case BattleStates.END_BATTLE:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PlayNextPlayerAction()
        {
            // Play next plan
            if (this.currentPlayerIndex < players.Count)
            {
                Plan currentPlan = this.players[currentPlayerIndex].Player.GetComponent<PlanBuilderComponent>().Plan;
                currentPlan.SubscribeToPlanEnd(OnPlayerPlanFinished);
                currentPlan.PlayPlan();
            }
            // else all plans played, move to next state
            else
            {
                this.currentPlayerIndex = 0;
                GoToState(BattleStates.BOSS_ACTION);
            }
        }

        private void OnPlayerPlanFinished(object sender, EventArgs args)
        {
            ((Plan)sender).UnsubscribeToPlanEnd(OnPlayerPlanFinished);
            this.currentPlayerIndex++;
            PlayNextPlayerAction();
        }

        public void PlayBossAction()
        {
            Plan currentPlan = this.boss.GetComponent<BossPlanBuilderComponent>().Plan;
            currentPlan.SubscribeToPlanEnd(OnBossActionFinished);
            currentPlan.PlayPlan();
        }

        private void OnBossActionFinished(object sender, EventArgs args)
        {
            ((Plan)sender).UnsubscribeToPlanEnd(OnBossActionFinished);
            GoToState(BattleStates.RESOLUTION);
        }

        public void StartBattle()
        {
            GoToRoundStart();
        }

        public void GoToRoundStart()
        {
            GoToState(BattleStates.BOSS_PLAN);
        }

        public void AdvanceFromBossPlanning()
        {
            GoToState(BattleStates.PLAYER_PLAN);
        }

        public void AdvanceFromPlayerPlanning()
        {
            // Advance state if all players finish planning their turns
            // Otherwise
            bool playerNotPlanned = false;
            foreach (var player in players)
            {
                PlanBuilderComponent plan = player.Player.GetComponent<PlanBuilderComponent>();
                if (!plan.Plan.FinishedPlanning)
                {
                    playerNotPlanned = true;

                    // Set next active player
                    PrioritizeActivePlayer(player);
                    PrioritizeCamera(player);
                    break;
                }
            }

            // Advance state if everyone finished planning their plans
            if (!playerNotPlanned)
            {
                GoToState(BattleStates.PLAYER_ACTION);
            }
        }

        public List<Player> GetPlayers()
        {
            return this.players.Select((p)=> p.Player).ToList();
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

        private void SwapCamera()
        {
            // rotate camera priorties
            int priorityFirstCamera = players[0].VCamera.Priority;
            for (int i = 0; i < players.Count; i++)
            {
                int nextPlayerIndex = (i + 1) % players.Count;
                players[i].VCamera.Priority = players[nextPlayerIndex].VCamera.Priority;
            }
            // Update last camera priority
            players[players.Count - 1].VCamera.Priority = priorityFirstCamera;
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
