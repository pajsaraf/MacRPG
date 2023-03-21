using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {

        [SerializeField] float WeaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        Transform target;
        float timeSinceLastAttack = 0f;



        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime; 

            if (target == null) return;
            
            if (target != null && !GetisInRange())
            {
                GetComponent<Mover>().MoveTo(target.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                GetComponent<Animator>().SetTrigger("attack");
                timeSinceLastAttack = 0;
            }
        }

        private bool GetisInRange()
        {
            return Vector3.Distance(transform.position, target.position) < WeaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform; //fighter knows if it should target because it has a target
        }

        public void Cancel()
        {
                target = null;
        }


    //animation event
        private void Hit ()
        {

        }
    }
}

