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
        Transform target;



        private void Update()
        {
            if (target == null) return;
            
            if (target != null && !GetisInRange())
            {
                GetComponent<Mover>().MoveTo(target.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
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
    }
}

