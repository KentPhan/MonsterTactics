using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UIActionPanel : MonoBehaviour
    {
        [SerializeField] private Button endPlanningButton;
        [SerializeField] private Button cancelPlanningButton;

        // Start is called before the first frame update
        void Start()
        {
            HideCancelPlanningButton();
            HideEndPlanningButton();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ShowCancelPlanningButton()
        {
            cancelPlanningButton.gameObject.SetActive(true);
        }
        public void HideCancelPlanningButton()
        {
            cancelPlanningButton.gameObject.SetActive(false);
        }
        public void SubscribeToCancelPlanningButton(UnityAction clickEvent)
        {
            cancelPlanningButton.onClick.AddListener(clickEvent);
        }
        public void UnsubscribeToCancelPlanningButton(UnityAction clickEvent)
        {
            cancelPlanningButton.onClick.RemoveListener(clickEvent);
        }
        public void UnsubscribeAllCancelPlanningButton()
        {
            cancelPlanningButton.onClick.RemoveAllListeners();
        }





        public void ShowEndPlanningButton()
        {
            endPlanningButton.gameObject.SetActive(true);
        }
        public void HideEndPlanningButton()
        {
            endPlanningButton.gameObject.SetActive(false);
        }
        public void SubscribeToEndPlanningButton(UnityAction clickEvent)
        {
            endPlanningButton.onClick.AddListener(clickEvent);
        }

        public void UnsubscribeToEndPlanningButton(UnityAction clickEvent)
        {
            endPlanningButton.onClick.RemoveListener(clickEvent);
        }

        public void UnsubscribeAllToEndPlanningButton()
        {
            endPlanningButton.onClick.RemoveAllListeners();
        }






    }
}
