using System.Collections;
using UnityEngine;

namespace Gameplay.Items
{
    [CreateAssetMenu(fileName = "ButtonEffectsSO", menuName = "Gameplay/Items/Effects")]
    public class ButtonEffectsSO : ScriptableObject
    {
        [SerializeField]
        private AudioSource interactionAudio;

        [SerializeField]
        private GameObject effectSuccess;
        [SerializeField]
        private GameObject effectFail;

        [SerializeField]
        private float duration = 2.0f;

        public ButtonEffectsSO() { }

        public IEnumerator PlayClickedSound(Transform transform)
        {
            AudioSource audioInstance = Instantiate(interactionAudio, transform);
            audioInstance.Play();
            yield return new WaitForSeconds(audioInstance.clip.length);
            if (audioInstance != null)
            {
                Destroy(audioInstance.gameObject);
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