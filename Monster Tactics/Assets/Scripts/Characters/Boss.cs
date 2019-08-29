using System.Collections.Generic;
using Assets.Scripts.Constants;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Characters
{
    public class Boss : AbstractCharacter
    {
        [SerializeField] [Range(1, 100)] private int health;
        [SerializeField] [Range(1, 20)] private int actionPointLimit = 100; public int ActionPointLimit => actionPointLimit;

        [SerializeField] private List<GameObject> gameObjectsWithMarkers;
        
        // Start is called before the first frame update
        void Start()
        {
            // Foreach square raycast downwards and mark that square as occupied
            foreach (GameObject markerObject in gameObjectsWithMarkers)
            {
                foreach (SquareMarker marker in markerObject.GetComponentsInChildren<SquareMarker>())
                {
                    FindAndMarkSquare(marker);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }


        public void ShowWeakPoints()
        {

        }

        public void HideWeakPoints()
        {

        }

        public override void TakeDamage(int damage)
        {
            this.health -= damage;
            if(this.health <= 0)
                GameManager.Instance.TriggerWin();
        }

        private void FindAndMarkSquare(SquareMarker marker)
        {
            // Raycast downward to find current square
            bool squareFound = true;
            int squareMask = LayerMask.GetMask(Layers.GRID);
            RaycastHit hitInfo;
            if (Physics.Raycast(marker.transform.position, Vector3.down, out hitInfo, 5000, squareMask,
                QueryTriggerInteraction.UseGlobal))
            {
                Square foundSquare = hitInfo.transform.GetComponent<Square>();
                if (foundSquare == null)
                {
                    squareFound = false;
                }
                else
                {
                    foundSquare.SetNotTraversable();
                }
            }
            else
            {
                squareFound = false;
            }

            if (!squareFound)
            {
                Debug.LogError("No Square found below marker for the monster");
            }
        }
    }
}
