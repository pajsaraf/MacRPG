using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {

        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float weaponDamage = 25f;
        Health target;
        float timeSinceLastAttack = 0f;



        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime; 

            if (target == null) return;
            if (target.IsDead()) return;
            
            if (target != null && !GetisInRange()) 
            {
                GetComponent<Mover>().MoveTo(target.transform.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt (target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                // actual damage is in Hit() event so as to be triggered by animation
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        //animation event
        private void Hit()
        {
            //damge is done when the Hit() occurs from the weapon animation Event
            if (target == null)
            {
                return;
            }
            target.TakeDamage(weaponDamage);
        }

        private bool GetisInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) 
                {
                    return false;
                }

            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>(); //fighter knows if it should target because it has a target
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }
    }
}

