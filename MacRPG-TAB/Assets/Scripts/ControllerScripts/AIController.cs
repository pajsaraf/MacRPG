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
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 3f;

        Fighter fighter;
        Health health;
        Mover mover;
        GameObject player;

        Vector3 guardStartPosition; // start position of ai
        float timeSinceLastDetectPlayer = Mathf.Infinity; //cache time since last detection
        float timeSinceArrivedAtWaypoint = Mathf.Infinity; //cache waypoint waiting time
        int currentWaypointIndex = 0;

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

                AttackBehaviour();
            }

            else if (timeSinceLastDetectPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }

            else
            {
                PatrolBehaviour();
            }
            
            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastDetectPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardStartPosition;

            if (patrolPath != null)
            {
                if (AtWayPoint())
                {
                    timeSinceArrivedAtWaypoint = 0f;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                mover.StartMoveAction(nextPosition);
            }
        }

        private bool AtWayPoint()
        {
            float distanceToWaypoint = Vector3.Distance (transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            timeSinceLastDetectPlayer = 0;
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