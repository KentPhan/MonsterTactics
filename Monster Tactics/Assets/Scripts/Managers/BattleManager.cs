using System;
using System.Collections.Generic;
using Assets.Scripts.Classes;
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
        // Unity Exposed Fields
        [SerializeField] [Range(1, 20)] private int actionPointsPerPlayer;

        // Member Properties
        private List<Tuple<Player, PlayerPlan>> players;

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

        public void SetPlayerPlans(Player player, PlayerPlan plan)
        {
            if (currentBattleState == BattleStates.PLAYER_PLAN)
            {
                PlayerPlan playerPlan = players.Find((set) => set.Item1 == player).Item2;
                playerPlan.Planned = true;
            }
        }

        private void AdvanceState()
        {
            switch (currentBattleState)
            {
                case BattleStates.START_BATTLE:
                    this.currentBattleState = BattleStates.PLAYER_PLAN;
                    break;
                case BattleStates.PLAYER_PLAN:
                    
                    // Advance state if all players finish planning their turns
                    bool playerNotPlanned = false;
                    foreach (var player in players)
                    {
                        if (!player.Item2.Planned)
                        {
                            playerNotPlanned = true;
                            break;
                        }
                    }
                    if (!playerNotPlanned)
                    {
                        this.currentBattleState = BattleStates.PLAYER_ACTION;
                        foreach (var player in players)
                        {
                            // TODO need to play players in pre determined order
                        }
                        // TODO Call functions to Play Player Actions. Subscribe to monster and player actions for when actions actually end
                    }
                    break;
                case BattleStates.PLAYER_ACTION:
                    // Player Action Plays
                    this.currentBattleState = BattleStates.BOSS_ACTION;
                    break;
                case BattleStates.BOSS_ACTION:
                    this.currentBattleState = BattleStates.RESOLUTION;
                    break;
                case BattleStates.RESOLUTION:
                    this.currentBattleState = BattleStates.PLAYER_PLAN;
                    break;
                case BattleStates.END_BATTLE:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnPlayerActionFinished()
        {
            AdvanceState();
        }

        private void OnBossActionFinished()
        {
            AdvanceState();
        }

        
        public void EndBattle()
        {

        }

    }
}
