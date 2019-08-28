using UnityEngine;

namespace Assets.Scripts.Characters
{
    public class Boss : MonoBehaviour, ICharacter
    {
        [SerializeField] [Range(1, 100)] private int health;

        

        // Start is called before the first frame update
        void Start()
        {

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

        public void TakeDamage(int damage)
        {
            throw new System.NotImplementedException();
        }
    }
}
