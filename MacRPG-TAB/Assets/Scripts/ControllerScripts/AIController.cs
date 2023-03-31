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
        [SerializeField] float suspicionTime = 5f;
        Fighter fighter;
        Health health;
        Mover mover;
        GameObject player;

        Vector3 guardStartPosition; // start position of ai
        float timeSinceLastDetectPlayer = Mathf.Infinity; //cache time since last detection


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
                timeSinceLastDetectPlayer = 0;
                AttackBehaviour();
            }

            else if (timeSinceLastDetectPlayer < suspicionTime)
            {
                //suspect player is near state
                SuspicionBehaviour();
            }

            else
            {
                //fighter.Cancel();
                GuardBehaviour();
            }
            timeSinceLastDetectPlayer += Time.deltaTime;
        }

        private void GuardBehaviour()
        {
            mover.StartMoveAction(guardStartPosition);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            fighter.Attack(player);
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