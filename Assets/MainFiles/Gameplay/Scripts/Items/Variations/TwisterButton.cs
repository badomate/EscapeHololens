using Gameplay;
using UnityEngine;

namespace Gameplay.Items
{
    public class TwisterButton : Item
    {

        [SerializeField]
        public ButtonEffectsSO buttonEffectsSO;

        // Should listen to the button's "OnClick" event
        public void ValidateGoal()
        {
            TwisterManager.instance.TryGuess(this);
        }

        public void ReactToClick(bool isCorrect)
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
