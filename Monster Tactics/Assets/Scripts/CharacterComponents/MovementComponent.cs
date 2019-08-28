using System;
using System.Numerics;
using Assets.Scripts.Characters;
using Assets.Scripts.Classes.Actions;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

namespace Assets.Scripts.CharacterComponents
{
    [RequireComponent(typeof(Player))]
    public class MovementComponent : MonoBehaviour, IActionable
    {
        private NavMeshAgent agent;
        private bool isMoving;
        private Vector3 destination;
        private Square destinationSquare;
        Animator anim;

        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            isMoving = false;
        }


        public void Update()
        {
            if (isMoving)
            {
                float dist = Vector3.Distance(transform.position, this.destination);
                if (dist != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete &&
                    dist <= 10.0f)
                {
                    Debug.Log("Invoking Finished Move Event");
                    isMoving = false;
                    GetComponent<Player>().SetSquare(destinationSquare);
                    OnFinishedAction?.Invoke(this, null);
                }
                Debug.Log(dist);
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            anim.SetBool("move", isMoving);
        }
        public void Move(Square destSquare)
        {
            Debug.Log("Setting Movement");
            Vector3 newDestination = destSquare.transform.position;
            if (agent.SetDestination(newDestination))
            {
                isMoving = true;
                this.destination = newDestination;
                this.destinationSquare = destSquare;
            }
        }

        public event EventHandler OnFinishedAction;
    }
}
