using System;
using UnityEngine;

namespace Twister
{
    internal class TwisterLevel : MonoBehaviour
    {
		[SerializeField]
        internal TwisterManager.Players guesser;
		[SerializeField]
        internal TwisterButton goal;

        internal void Spawn()
        {
            throw new NotImplementedException();
        }
    }
}