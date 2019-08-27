using System;
using Assets.Scripts.Characters;
using Assets.Scripts.Classes.Actions;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.CharacterComponents
{
    [RequireComponent(typeof(Player))]
    public class MovementComponent : MonoBehaviour, IActionable
    {
        private NavMeshAgent agent;
        private bool isMoving;
        private Vector3 destination;
        Animator anim;

        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            isMoving = false;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (isMoving)
            {
                float dist = agent.remainingDistance;
                if (dist != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete &&
                    agent.remainingDistance <= 0.0f)
                {
                    Debug.Log("Invoking Finished Move Event");
                    isMoving = false;
                    GetComponent<Player>().FindAndUpdateSquare();
                    OnFinishedAction?.Invoke(this, null);
                }
            }
            anim.SetBool("move", isMoving);
        }
        public void Move(Square destinationSquare)
        {
            isMoving = true;
            this.destination = destinationSquare.transform.position;
            agent.SetDestination(this.destination);
        }

        public event EventHandler OnFinishedAction;
    }
}
