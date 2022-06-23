using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Backrooms.Player;

namespace Backrooms.Enemy
{
    public class Smiler : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] PlayerController player;
        [Header("Move")]
        bool canMove;
        [SerializeField] float walkSpeed;
        [SerializeField] float runSpeed;
        Rigidbody enemyRB;

        public float MoveSpeed 
        {
            get 
            {
                if(player.PlayerSpeedType == SpeedType.Fast) 
                {
                    return runSpeed;
                }
                else 
                {
                    return walkSpeed;
                }
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(StartMove());
            enemyRB = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        private void FixedUpdate()
        {
            if(canMove) Move();
        }
        IEnumerator StartMove() 
        {
            yield return new WaitForSeconds(2);
            canMove = true;
        }
        private void Move()
        {
             enemyRB.velocity = -Vector3.right * MoveSpeed;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) 
            {
                GameManager.Instance.DeathGame();
            }
        }
    }
}
