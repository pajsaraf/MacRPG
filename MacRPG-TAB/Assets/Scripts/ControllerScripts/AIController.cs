using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        Fighter fighter;
        Health health;
        Mover mover;
        GameObject player;
        Vector3 guardStartPosition;


        private void Start()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");

            guardStartPosition = transform.position; //save start location of the guard spawn 
        }

        private void Update()
        {
            if (health.IsDead())  //if its dead then do nothing more
            {
                return;
            }

            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                fighter.Attack(player);
            }
            else
            {
                //fighter.Cancel();
                mover.StartMoveAction(guardStartPosition);
            }

        }
        private bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }

// special method called by unity and will show in editor - for patrol paths to be visible
        private void OnDrawGizmosSelected() 
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }   

    }
}