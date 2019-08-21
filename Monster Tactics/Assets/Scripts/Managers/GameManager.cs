using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public enum GameStates
        {
            START,
            PLAY,
            GAME_OVER
        }

        private GameStates currentGameState;


        [SerializeField] [Range(0, 8)] private int numberOfPlayers = 2;
        public int NumberOfPlayers => numberOfPlayers;

        private static GameManager _instance;
        public static GameManager Instance
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
            else if(_instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
        Vector3
        }

        // Update is called once per frame
        void Update()
        {
        
        }




    }
}
