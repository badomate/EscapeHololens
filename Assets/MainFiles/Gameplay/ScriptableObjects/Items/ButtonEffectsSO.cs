using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Items
{
    [CreateAssetMenu(fileName = "ButtonEffects", menuName = "Gameplay/Effects")]
    public class ButtonEffectsSO : ScriptableObject
    {
        [SerializeField]
        private AudioSource interactionAudio;

        [SerializeField]
        private GameObject effectSuccess;
        [SerializeField]
        private GameObject effectFail;

        [SerializeField]
        private float duration = 3.0f;

        public ButtonEffectsSO() { }

        public void PlayClickedSound()
        {
            if (interactionAudio.isActiveAndEnabled) { 
                interactionAudio.Play(); 
            }
        }

        public IEnumerator PlayClickedAnimation(bool isCorrect, Transform transform)
        {
            GameObject effect = isCorrect ? effectSuccess : effectFail;
            GameObject clickedAnimation = Instantiate(effect, transform.position, transform.rotation);
            yield return new WaitForSeconds(duration);
            if (clickedAnimation != null)
            {
                Destroy(clickedAnimation);
            }
        }
    }
}