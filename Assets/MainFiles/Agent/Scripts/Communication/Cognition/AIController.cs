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
        public UnityEvent<GameObject> OnRequestInteraction = new UnityEvent<GameObject>();
        public UnityEvent<ActionID> OnRequestAction = new UnityEvent<ActionID>();

        [SerializeField]
        private List<Property> KnownPropertiesList;

        // Translates the property string to the equivalent Property
        private Dictionary<string, Property> KnownProperties;
        
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

        private void InitializeKnownProperties()
        {
            KnownProperties = new Dictionary<string, Property>();
            foreach (Property property in KnownPropertiesList)
            {
                KnownProperties.Add(property.FullName.ToString(), property);
                Debug.Log("Added property to AI Controller: " + property.FullName);
            }
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
            if (gestureIDs.Count != 0 && gestureIDs[0] != GestureID.G00T_NONE)
            {
                string name = "";

                switch (gestureIDs[0])
                {
                    case (GestureID.G13S_CIRCLE):
                        name = "SHAPE_CIRCLE";
                        break;
                    case (GestureID.G14S_SQUARE):
                        name = "SHAPE_SQUARE";
                        break;
                    case (GestureID.G16C_BLUE):
                        name = "COLOR_BLUE";
                        break;
                    case (GestureID.G15C_RED):
                        name = "COLOR_RED";
                        break;
                    case (GestureID.G04T_NO):
                        OnRequestAction.Invoke(ActionID.BLUE);
                        return;
                    case (GestureID.G03T_YES):
                        OnRequestAction.Invoke(ActionID.RED);
                        return;
                    default:
                        break;
                }

                if (KnownProperties.TryGetValue(name, out Property property))
                {
                    TryItem(property);
                }
                else
                {
                    OnRequestAction.Invoke(ActionID.UNRECOGNIZED);
                }
            }
            else
            {
                OnRequestAction.Invoke(ActionID.AMBIGUOUS);
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

