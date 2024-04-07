using Gameplay;
using Gameplay.Items.Properties;
using UnityEngine;

namespace Gameplay.Items
{
    [RequireComponent(typeof(ColorPropertySO), typeof(ShapePropertySO))]
    public class TwisterButton : Item
    {
        [SerializeField]
        public ButtonEffectsSO buttonEffectsSO;

        public void RegisterId()
        {
            Id = gameObject.GetInstanceID();
        }

        // Should listen to the button's "OnClick" event
        [ContextMenu("Press")]
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
