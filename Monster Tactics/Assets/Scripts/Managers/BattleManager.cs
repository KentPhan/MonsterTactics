using System;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class BattleManager : MonoBehaviour
    {
        public enum BattleStates
        {
            START,
            PLAYER_PLAN,
            PLAYER_ACTION,
            BOSS_PLAN,// TODO do I even need this?
            BOSS_ACTION,
        }

        private BattleStates currentBattleState;
        private int currentPlayerIndex;

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
            this.currentBattleState = BattleStates.START;
            this.currentPlayerIndex = 0;// 0 index player
        }

        // Update is called once per frame
        private void Update()
        {
        
        }

        public void AdvancePhase()
        {
            switch (currentBattleState)
            {
                case BattleStates.START:
                    this.currentBattleState = BattleStates.PLAYER_PLAN;
                    break;
                case BattleStates.PLAYER_PLAN:
                    
                    // Advance state if all players finish
                    currentPlayerIndex++;
                    if (currentPlayerIndex >= GameManager.Instance.NumberOfPlayers)
                    {
                        this.currentBattleState = BattleStates.PLAYER_ACTION;
                        // TODO Call functions to Play Player Actions. Subscribe to monster and player actions for when actions actually end
                    }
                    break;
                case BattleStates.PLAYER_ACTION:
                    // Can't Advance manually
                    Debug.Log("Cannot advance from Player Action Manually?");
                    break;
                case BattleStates.BOSS_PLAN:
                    // Monster calculates what it's gonna do
                    this.currentBattleState = BattleStates.BOSS_ACTION;
                    break;
                case BattleStates.BOSS_ACTION:
                    // Player acts on what it does
                    Debug.Log("Cannot advance from Monster Action Manually?");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void EndBattle()
        {

        }

    }
}
