using System.Collections;
using Gameplay;
using Gameplay.Items;
using Gameplay.Items.Properties;
using UnityEngine;
using Agent.Movement;

namespace Agent.Communication.Cognition
{
    public class AIController : MonoBehaviour
    {
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

