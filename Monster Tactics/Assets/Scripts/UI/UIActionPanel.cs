using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UIActionPanel : MonoBehaviour
    {
        [SerializeField] private Button endPlanningButton;

        // Start is called before the first frame update
        void Start()
        {
            HideActions();
        }

        // Update is called once per frame
        void Update()
        {

        }


        public void SubscribeToEndPlanningButton(UnityAction clickEvent)
        {
            endPlanningButton.onClick.AddListener(clickEvent);
        }

        public void UnsubscribeAllToEndPlanningButton()
        {
            endPlanningButton.onClick.RemoveAllListeners();
        }


        public void ShowActions()
        {
            endPlanningButton.gameObject.SetActive(true);
        }

        public void HideActions()
        {
            endPlanningButton.gameObject.SetActive(false);
        }

    }
}
