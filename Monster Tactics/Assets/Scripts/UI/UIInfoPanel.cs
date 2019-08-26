using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UIInfoPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI actionPointTotalValueText;
        [SerializeField] private TextMeshProUGUI actionPointSpentValueText;

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
    }
}
