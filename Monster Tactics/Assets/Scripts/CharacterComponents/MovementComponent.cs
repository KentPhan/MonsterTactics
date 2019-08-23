using System;
using Assets.Scripts.Classes.Actions;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.CharacterComponents
{
    public class MovementComponent : MonoBehaviour, IActionable
    {
        private NavMeshAgent agent;
        private bool isMoving;
        private Vector3 destination;
        
        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            isMoving = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (isMoving)
            {
                if ((agent.transform.position - this.destination).magnitude <= 0.1f)
                {
                    isMoving = false;
                    OnFinishedAction?.Invoke(this, null);
                }
            }
        }
        public void Move(Vector3 dest)
        {
            isMoving = true;
            this.destination = dest;
            agent.SetDestination(this.destination);
        }
        
        public event EventHandler OnFinishedAction;
    }
}
