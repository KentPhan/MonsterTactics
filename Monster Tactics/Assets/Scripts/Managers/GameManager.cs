using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public enum GameStates
        {
            START,
            PLAY,
            WIN,
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
        }

        // Update is called once per frame
        void Update()
        {
            switch (currentGameState)
            {
                case GameStates.START:
                    if (Input.GetKeyDown(KeyCode.Return))
                    {
                        this.currentGameState = GameStates.PLAY;
                        BattleManager.Instance.StartBattle();
                    }
                    break;
                case GameStates.PLAY:
                    break;
                case GameStates.WIN:
                    break;
                case GameStates.GAME_OVER:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();

            }

            if (Input.GetKeyDown(KeyCode.F1))
            {
                SceneManager.LoadScene(0);
            }

        }

        public void TriggerWin()
        {
            this.currentGameState = GameStates.WIN;
            CanvasManager.Instance.WinScreen.gameObject.SetActive(true);
            Debug.Log("WINNN!");
        }

        public void TriggerGameOver()
        {
            this.currentGameState = GameStates.GAME_OVER;
            CanvasManager.Instance.GameOverScreen.gameObject.SetActive(true);
            Debug.Log("GAME OVER");
        }


    }
}
