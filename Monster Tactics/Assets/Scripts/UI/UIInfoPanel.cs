using System;
using Assets.Scripts.Managers;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UIInfoPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI actionPointTotalValueText;
        [SerializeField] private TextMeshProUGUI actionPointSpentValueText;
        [SerializeField] private TextMeshProUGUI titleValueText;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateActionPointTotalValue(int value)
        {
            actionPointTotalValueText.text = value.ToString();
        }
        public void UpdateActionSpentValue(int value)
        {
            actionPointSpentValueText.text = value.ToString();
        }

        public void UpdateTitleText(BattleStates battleState)
        {
            string text = string.Empty;
            switch(battleState)
            {
                case BattleStates.START_BATTLE:
                    text = "START";
                    break;
                case BattleStates.BOSS_PLAN:
                    text = "START";
                    break;
                case BattleStates.PLAYER_PLAN:
                    text = "PLAYER TURN";
                    break;
                case BattleStates.PLAYER_ACTION:
                    text = "PLAYER ACTION";
                    break;
                case BattleStates.BOSS_ACTION:
                    text = "BOSS TURN";
                    break;
                case BattleStates.RESOLUTION:
                    text = "BOSS TURN";
                    break;
                case BattleStates.END_BATTLE:
                    text = "END";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(battleState), battleState, null);
            }

            titleValueText.text = text;
        }
    }
}
