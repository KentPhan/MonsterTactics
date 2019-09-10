using Assets.Scripts.Constants;
using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Characters
{
    public class Player : AbstractCharacter
    {
        // Exposed properties
        [SerializeField] [Range(1, 20)] private int actionPointLimit = 6;
        public int ActionPointLimit => actionPointLimit;
        [SerializeField] private Camera playerCamera;
        public Camera PlayerCamera => playerCamera;

        [SerializeField] AudioClip impale;

        // Members
        private NavMeshAgent agent;

        Animator anim;
        AudioSource audioSource;

        private Square currentSquare;
        public Square CurrentSquare => currentSquare;

        public GameObject ConsumableSlot;
        public GameObject WeaponSlot;
        public GameObject SpellSlot;

        public override void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            anim = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            base.Awake();
        }

        // Start is called before the first frame update
        private void Start()
        {
            FindAndUpdateSquare();
            CanvasManager.Instance.UIInfoPanel.UpdateActionPointTotalValue(actionPointLimit);
        }

        public void SetSquare(Square square)
        {
            this.currentSquare = square;
        }

        public void FindAndUpdateSquare()
        {
            // Raycast downward to find current square
            bool squareFound = true;
            int squareMask = LayerMask.GetMask(Layers.GRID);
            RaycastHit hitInfo;
            if (Physics.Raycast(this.transform.position, Vector3.down, out hitInfo, 5000, squareMask,
                QueryTriggerInteraction.UseGlobal))
            {
                this.currentSquare = hitInfo.transform.GetComponent<Square>();
                if (this.currentSquare == null)
                {
                    squareFound = false;
                }
            }
            else
            {
                squareFound = false;
            }

            if (!squareFound)
            {
                Debug.LogError("No Square found below the player via raycast");
            }
        }

        void Update()
        {
            
        }
    


        public override void TakeDamage(int damage)
        {
            this.currentHealth -= damage;
            audioSource.PlayOneShot(impale);
            UpdateHealthBar();
            
            if (this.currentHealth <= 0)
                GameManager.Instance.TriggerGameOver();
        }

        public bool OnVisionSquare()
        {
            return this.currentSquare.IsVisionSquare();
        }

        public void Heal(int heal)
        {
            this.currentHealth += heal;
            if (this.maxHealth < this.currentHealth)
                this.currentHealth = this.maxHealth;
            UpdateHealthBar();
        }

    }
}
