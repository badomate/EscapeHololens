using System;
using UnityEngine;
using Gameplay.Items;
using Unity.VisualScripting;

namespace Gameplay
{
    public class TwisterLevel : MonoBehaviour
    {
		[SerializeField]
        internal TwisterManager.Players guesser;

		[SerializeReference]
        public TwisterButton goal;

        public void Spawn(Vector3 spawnPosition)
        {
            ValidateInstanceIds();
            Instantiate(gameObject, spawnPosition, Quaternion.identity).GetComponent<TwisterLevel>();
            //ValidateGoalInstance(instancedLevel);
            
        }

        // To get around Unity's lack of support for nested prefab instancing
        internal void ValidateGoalInstance(GameObject instancedLevel)
        {
            TwisterButton[] buttons = instancedLevel.GetComponentsInChildren<TwisterButton>();
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].transform.localPosition == goal.transform.localPosition)
                {
                    goal = buttons[i];
                    break;
                }
            }
        }

        internal void ValidateInstanceIds()
        {
            TwisterButton[] buttons = GetComponentsInChildren<TwisterButton>();
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].RegisterId();
            }
        }
    }
}