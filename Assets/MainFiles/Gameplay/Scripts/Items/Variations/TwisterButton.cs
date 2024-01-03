using UnityEngine;

namespace Gameplay.Items.Variations
{
    public class TwisterButton : Item
    {

        [SerializeField]
        public ButtonEffectsSO buttonEffectsSO;

        public void InvokeClicked()
        {
            bool isCorrect = EqualsId(PseudoGameManager.Instance.targetId);
            InvokeClicked(isCorrect);
        }

        private void InvokeClicked(bool isCorrect)
        {
            PlayClickedSound();
            PlayClickedAnimation(isCorrect);
        }

        public void PlayClickedSound()
        {
            StartCoroutine(buttonEffectsSO.PlayClickedSound(transform));
        }

        public void PlayClickedAnimation(bool isCorrect)
        {
            StartCoroutine(buttonEffectsSO.PlayClickedAnimation(isCorrect, transform));
        }
    }
}
