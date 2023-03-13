using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
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
                GetComponent<Mover>().Stop();
            }
        }

        private bool GetisInRange()
        {
            return Vector3.Distance(transform.position, target.position) < WeaponRange;
        }

        public void Attack(CombatTarget combatTarget)
            {
                target = combatTarget.transform; //fighter knows if it should target because it has a target
            }

            public void Cancel()
            {
                target = null;
            }
    }
}

