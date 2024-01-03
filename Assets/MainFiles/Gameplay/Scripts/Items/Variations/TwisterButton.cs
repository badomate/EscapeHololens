using System.Collections.Generic;
using UnityEngine;
using Gameplay.Items.Properties;

namespace Gameplay.Items.Variations
{
    public class TwisterButton : Item
    {

        [SerializeField]
        public ButtonEffectsSO buttonEffectsSO;

        public void InvokeClicked()
        {
            Debug.Log("CLICK");
            bool isCorrect = EqualsId(PseudoGameManager.Instance.targetId);
            Debug.Log("ID matches? " + isCorrect + "!");
            InvokeClicked(isCorrect);
        }

        private void InvokeClicked(bool isCorrect)
        {
            PlayClickedSound();
            PlayClickedAnimation(isCorrect);
        }

        public void PlayClickedSound()
        {
            buttonEffectsSO.PlayClickedSound();
        }

        public void PlayClickedAnimation(bool isCorrect)
        {
            StartCoroutine(buttonEffectsSO.PlayClickedAnimation(isCorrect, transform));
        }
    }
}
