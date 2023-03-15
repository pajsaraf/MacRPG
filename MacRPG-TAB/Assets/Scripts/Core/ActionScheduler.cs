using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {

        IAction currentAction;

        public void StartAction(IAction action)  //cancel fight when move start and cancel move when combat starts
        {
            if (currentAction == action) return;
            if (currentAction != null)
            {
                currentAction.Cancel();
            }

            currentAction = action;
        }
    }
}