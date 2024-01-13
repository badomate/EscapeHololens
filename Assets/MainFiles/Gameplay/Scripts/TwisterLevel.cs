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
            Instantiate(gameObject, spawnPosition, Quaternion.identity);
        }
    }
}