using UnityEngine;
namespace Twister
{
	internal class TwisterButton : MonoBehaviour {
		private void OnTriggerEnter(Collider other) {
			TwisterManager.instance.TryGuess(this);
		}
	}
}