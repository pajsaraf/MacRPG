using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;


namespace RPG.Control 
{
    public class PlayerController : MonoBehaviour
    {
        
        private void Update()
        {
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
            //print("cant move");   //outside navmesh - edge of world
        }


        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) 
                {
                    continue;
                }

                //GameObject targetGameObject = target.gameObject;

                if(!GetComponent<Fighter>().CanAttack(target.gameObject)) //if we cant attack then...
                {
                    continue; // continue in for each loop
                }

                if(Input.GetMouseButtonDown(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }
                return true;
            }
            return false; 
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point);
                    //print("Kiss me i move");
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }

}
