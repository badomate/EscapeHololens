using System;
using UnityEngine;
using Gameplay.Items;

namespace Gameplay
{
    internal class TwisterLevel : MonoBehaviour
    {
		[SerializeField]
        internal TwisterManager.Players guesser;
		[SerializeField]
        internal TwisterButton goal;

        internal void Spawn(Vector3 spawnPosition)
        {
            GameObject instancedLevel = Instantiate(gameObject, spawnPosition, Quaternion.identity);
            ValidateGoalInstance(instancedLevel);
            
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
    }
}