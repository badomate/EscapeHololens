using System.Collections;
using Gameplay;
using Gameplay.Items;
using Gameplay.Items.Properties;
using UnityEngine;
using Agent.Movement;
using System.Collections.Generic;
using UnityEngine.Events;
using ActionID = Agent.Movement.MovementController.ActionID;
using GestureID = Agent.Communication.Cognition.GestureRecognizer.ID;

namespace Agent.Communication.Cognition
{
    public class AIController : MonoBehaviour
    {
        [SerializeField]
        private ColorProperty testingProperty;

        public UnityEvent<GameObject> OnRequestInteraction = new UnityEvent<GameObject>();

        [SerializeField]
        private List<Property> KnownProperties;
        
        IKController iKController;
        Animator animator;

        void Awake() {
            TwisterManager.OnP2Display += IndicateLevel;
            iKController = GetComponent<IKController>();
            animator = GetComponent<Animator>();
        }

        void IndicateLevel(TwisterLevel level) {
            iKController.SetIdle();
            //StartCoroutine(nameof(GestureProperties), level.goal);
        }
        
        /*
        IEnumerator GestureProperties(TwisterButton tb){
            animator.Play(tb.GetProperty<ShapeProperty>().ShapeTraitsSO.gesture.name);
            yield return new WaitForSeconds(4);
            animator.Play(tb.GetProperty<ColorProperty>().ColorTraitsSO.gesture.name);
            yield return new WaitForSeconds(4);
            animator.Play("Armature|Idle");
        }
        */

        public void InterpretGestures(List<GestureID> gestureIDs)
        {
            ColorProperty colorProperty = testingProperty;
            
            if (gestureIDs.Count != 0 && gestureIDs[0] != GestureID.G00T_NONE)
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


        [ContextMenu("PlayRed")]
        void DebugPlay() {
            iKController.SetIdle();
            animator.Play("Armature|Red");
        }
    }
}

