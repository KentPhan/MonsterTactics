using System;
using System.Collections.Generic;
using Assets.Scripts.Classes.Actions;
using Assets.Scripts.Managers;
using Cinemachine.Utility;
using UnityEngine;

namespace Assets.Scripts.CharacterComponents
{
    public class BossAttackComponent : MonoBehaviour, IActionable
    {
        [SerializeField][Range(0,100)]private int baseDamage;
        [SerializeField] private GameObject SpikePrefab;
        [SerializeField] private float DistanceBelowGround;
        [SerializeField] private float DistanceOfMovement;
        [SerializeField] [Range(0.1f, 1.0f)]private float SinWaveAttackFrequency = 0.5f;
        [SerializeField] [Range(0.1f, 10.0f)] private int NumberOfAttacks = 2;

        private bool playingAttack;
        private List<Tuple<Vector3,GameObject>> spikeGameObjects;// Stores base position of spike and spike gameobject
        private float sinTime;
        private float attackTime;

        private void Awake()
        {
            this.playingAttack = false;
            this.spikeGameObjects = new List<Tuple<Vector3,GameObject>>();
            this.sinTime = 0.0f;
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                List<Square> squares = GridSystem.Instance.GetRandomListOfSquares(0.5f);
                SuperAttackSquares(squares);
            }

            if (playingAttack)
            {
                foreach (var spike in spikeGameObjects)
                {
                    Vector3 spikePos = spike.Item1;
                    float sinOffset = ((Mathf.PI / 2.0f) * this.SinWaveAttackFrequency);
                    float sinValue = Mathf.Sin(((sinTime - sinOffset) / this.SinWaveAttackFrequency)) + .25f; // Value between 0 and 1
                    spike.Item2.transform.position = new Vector3(spikePos.x, spikePos.y + (sinValue * DistanceOfMovement), spikePos.z);
                }
                // Apply Damage after finishing animating Attack
                sinTime += Time.deltaTime;

                // End Attack
                Debug.Log(sinTime + " " + this.attackTime);
                if (sinTime > this.attackTime)
                {
                    ResetAttack();
                }
            }
        }

        private void ResetAttack()
        {
            playingAttack = false;
            this.sinTime = 0.0f;
            foreach(var shitToDelete in spikeGameObjects)
            {
                Destroy(shitToDelete.Item2.gameObject);
            }
            spikeGameObjects.Clear();
        }

        public void SuperAttackSquares(List<Square> targets)
        {
            ResetAttack();
            this.playingAttack = true;
            this.attackTime = this.SinWaveAttackFrequency * this.NumberOfAttacks * Mathf.PI * 2.0f;
            foreach (Square target in targets)
            {
                Vector3 SpawnPosition = target.transform.position;
                SpawnPosition.y -= DistanceBelowGround;
                GameObject newGameObject = Instantiate(this.SpikePrefab, SpawnPosition, Quaternion.identity,this.transform.parent.transform);
                this.spikeGameObjects.Add(new Tuple<Vector3, GameObject>(newGameObject.transform.position,newGameObject));
                
            }
        }


        public event EventHandler OnFinishedAction;
    }
}
