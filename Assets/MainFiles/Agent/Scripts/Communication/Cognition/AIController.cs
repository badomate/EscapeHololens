using System.Collections;
using Gameplay;
using Gameplay.Items;
using Gameplay.Items.Properties;
using UnityEngine;
using Agent.Movement;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Agent.Communication.Cognition
{
    public class AIController : MonoBehaviour
    {
        [SerializeField]
        private ColorProperty testingProperty;

        public UnityEvent<GameObject> OnRequestInteraction = new UnityEvent<GameObject>();
        public enum ActionID
        {
            NONE,
            GO_LEFT,
            GO_RIGHT,
            GO_FORWARD,
            GO_BACKWARD,

            TURN_LEFT,
            TURN_RIGHT,

            CIRCLE,
            SQUARE,

            RED,
            BLUE,

            NEW_WORD,
            ATTENTION,
            YES,
            NO,

            VICTORY,

            UNRECOGNIZED,
            AMBIGUOUS
        }

        IKController iKController;
        Animator animator;

        void Awake() {
            TwisterManager.OnP2Display += IndicateLevel;
            iKController = GetComponent<IKController>();
            animator = GetComponent<Animator>();
        }

        void IndicateLevel(TwisterLevel level) {
            iKController.SetIdle();
            StartCoroutine(nameof(GestureProperties), level.goal);
        }

        IEnumerator GestureProperties(TwisterButton tb){
            animator.Play(tb.GetProperty<ShapeProperty>().ShapeTraitsSO.gesture.name);
            yield return new WaitForSeconds(4);
            animator.Play(tb.GetProperty<ColorProperty>().ColorTraitsSO.gesture.name);
            yield return new WaitForSeconds(4);
            animator.Play("Armature|Idle");
        }

        public void InterpretGesture(GestureRecognizer.ID gestureID)
        {
            ColorProperty colorProperty = testingProperty;
            
            if (gestureID != GestureRecognizer.ID.NONE)
            {
                TryItem(colorProperty);
            }
            else
            {
                TryItem(colorProperty);
            }
            
        }

        void TryItem(Property property)
        {
            foreach (Item item in FindObjectsByType<Item>(FindObjectsSortMode.None))
            {
                if (item.GetProperty(property.Type) == property)
                {
                    OnRequestInteraction.Invoke(item.gameObject);
                }
            }
        }

        void TrySolve(Property property) {
            foreach (Item item in FindObjectsByType<Item>(FindObjectsSortMode.None))
            {
                if(item.GetProperty(property.Type) == property) {
                    iKController.SetTarget(item.transform);
                    return;
                }
            }
        }

        [ContextMenu("PlayRed")]
        void DebugPlay() {
            iKController.SetIdle();
            animator.Play("Armature|Red");
        }
    }
}

