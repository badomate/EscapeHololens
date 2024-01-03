using System.Collections.Generic;
using UnityEngine;
using Gameplay.Items.Properties;
using Unity.VisualScripting;

namespace Gameplay.Items.Variations
{
    public class TwisterButton : Item
    {

        [SerializeField]
        public ButtonEffectsSO buttonEffectsSO;
/* TBA
        [SerializeField]
        private float Cooldown = 1.5f;

        [SerializeField]
        private float timeSinceClick = 0;
*/
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
