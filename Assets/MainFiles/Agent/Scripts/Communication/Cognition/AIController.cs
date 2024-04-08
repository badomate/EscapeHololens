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
        public UnityEvent<GameObject> InteractionRequestedEvent = new UnityEvent<GameObject>();
        public UnityEvent<ActionID> ActionRequestedEvent = new UnityEvent<ActionID>();
        public UnityEvent KnowledgeBaseSetupEvent = new UnityEvent();
        public Dictionary<int, Item> GameItems;

        [SerializeField]
        public List<PropertySO> KnownPropertiesList;

        // Translates the property string to the equivalent PropertySO
        private Dictionary<string, PropertySO> KnownProperties;
        
        IKController iKController;
        Animator animator;

        public AIController()
        {
            GameItems = new Dictionary<int, Item>();
        }

        private void Start()
        {
            GestureRecognizer recognizer = GetComponent<GestureRecognizer>();
            recognizer.GestureRecognizedEvent.AddListener(OnGestureDetected);
            InitializeKnownProperties();
            KnowledgeBaseSetupEvent.Invoke();
        }

        void Awake() {
            TwisterManager.OnP2Display += IndicateLevel;
            iKController = GetComponent<IKController>();
            animator = GetComponent<Animator>();
        }

        void IndicateLevel(TwisterLevel level) {
            iKController.SetIdle();
            Item[] items = level.gameObject.GetComponentsInChildren<Item>();
            GameItems.Clear();

            foreach (Item item in items)
            {
                GameItems.Add(item.Id, item);
            }
            //StartCoroutine(nameof(GestureProperties), level.goal);
        }

        private void InitializeKnownProperties()
        {
            KnownProperties = new Dictionary<string, PropertySO>();
            foreach (PropertySO property in KnownPropertiesList)
            {
                KnownProperties.Add(property.fullName.ToString(), property);
                Debug.Log("Added property to AI Controller: " + property.fullName);
            }
        }

        public void OnGestureDetected(List<GestureID> gestureIDs)
        {
            if (gestureIDs.Count != 0 && gestureIDs[0] != GestureID.G00T_NONE)
            {
                string name = "";

                switch (gestureIDs[0])
                {
                    case (GestureID.G16C_BLUE):
                        name = "color.blue";
                        break;
                    case (GestureID.G15C_RED):
                        name = "color.red";
                        break;
                    case (GestureID.G14S_SQUARE):
                        name = "shape.square";
                        break;
                    case (GestureID.G13S_CIRCLE):
                        name = "shape.circle";
                        break;
                    case (GestureID.G04T_NO):
                        ActionRequestedEvent.Invoke(ActionID.BLUE);
                        return;
                    case (GestureID.G03T_YES):
                        ActionRequestedEvent.Invoke(ActionID.RED);
                        return;
                    default:
                        break;
                }

                if (KnownProperties.TryGetValue(name, out PropertySO property))
                {
                    TryItem(property);
                }
                else
                {
                    ActionRequestedEvent.Invoke(ActionID.UNRECOGNIZED);
                }
            }
            else
            {
                ActionRequestedEvent.Invoke(ActionID.AMBIGUOUS);
            }
        }

        void TryItem(PropertySO property)
        {
            foreach (Item item in GameItems.Values)
            {
                if (item.GetProperty(property.type) == property)
                {
                    InteractionRequestedEvent.Invoke(item.gameObject);
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

