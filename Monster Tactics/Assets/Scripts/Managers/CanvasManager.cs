using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class CanvasManager : MonoBehaviour
    {
        [SerializeField] private UIInfoPanel uiInfoPanel; public UIInfoPanel UIInfoPanel => uiInfoPanel;
        [SerializeField] private UIActionPanel uiActionPanel; public UIActionPanel UIActionPanel => uiActionPanel;

        [SerializeField] public RectTransform GameOverScreen;
        [SerializeField] public RectTransform WinScreen;

        private static CanvasManager _instance;
        public static CanvasManager Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                else
                {
                    Debug.LogError("No Canvas Manager Found.");
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
            
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
