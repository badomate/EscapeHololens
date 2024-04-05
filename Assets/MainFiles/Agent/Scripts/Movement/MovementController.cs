using UnityEngine;
using Agent.Communication.Cognition;
using System.Collections.Generic;

namespace Agent.Movement
{
    public class MovementController : MonoBehaviour
    {
        [SerializeField]
        private IKController ikController;

        [SerializeField]
        private bool isDebugOn = true;

        Animator animator;
        public bool performStandardAnimations = true;

        public enum ActionID
        {
            NONE,
            
            NEW_WORD,
            ATTENTION,
            
            YES,
            NO,

            UNRECOGNIZED,
            AMBIGUOUS,

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

            VICTORY
        }

        [SerializeField]
        private Dictionary<ActionID, string> actionMatcher =
            new Dictionary<ActionID, string> {
                { ActionID.BLUE, "Blue" },
                { ActionID.RED, "Red" },
                { ActionID.SQUARE, "Square" },
                { ActionID.CIRCLE, "Circle" },
                { ActionID.YES, "Yes" },
                { ActionID.NO, "No" },
                { ActionID.GO_FORWARD, "WalkFwd" },
                { ActionID.GO_RIGHT, "WalkRight" },
                { ActionID.GO_LEFT, "WalkLeft" },
                { ActionID.GO_BACKWARD, "WalkBwd" },
                { ActionID.TURN_LEFT, "TurnLeft" },
                { ActionID.TURN_RIGHT, "TurnRight" },
                { ActionID.UNRECOGNIZED, "Ambiguous" },
                { ActionID.AMBIGUOUS, "Ambiguous" }
        };

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            ikController = GetComponent<IKController>();
            AIController controller = GetComponent<AIController>();
            controller.ActionRequestedEvent.AddListener(OnActionRequested);
            controller.InteractionRequestedEvent.AddListener(OnInteractionRequested);
        }

        public void OnActionRequested(ActionID action)
        {
            if (actionMatcher.TryGetValue(action, out string animationID))
            {
                Debug.Log("Play animation " + animationID + " now!");
                animator.SetTrigger(animationID);
            }
            else
            {
                DeactivateWalkAnimations();
            }
        }

        public void OnInteractionRequested(GameObject target)
        {
            ikController.SetTarget(target.transform);
        }

        void DeactivateWalkAnimations()
        {
            animator.SetBool("WalkFwd", false);
            animator.SetBool("WalkRight", false);
            animator.SetBool("WalkLeft", false);
            animator.SetBool("WalkBwd", false);
        }

        // Update is called once per frame
        void Update()
        {
            if (!isDebugOn) return;
            
            ActionID action = ActionID.NONE;

            if (Input.GetKeyDown("1")) //this usually causes the event to fire multiple times, but that's fine, we want animations to play while the gesture is held
            {
                action = ActionID.BLUE;
            }
            else if (Input.GetKeyDown("2"))
            {
                action = ActionID.RED;
            }
            else if (Input.GetKeyDown("3"))
            {
                action = ActionID.SQUARE;
            }
            else if (Input.GetKeyDown("4"))
            {
                action = ActionID.CIRCLE;
            }
            else if (Input.GetKeyDown("5"))
            {
                action = ActionID.YES;
            }
            else if (Input.GetKeyDown("6"))
            {
                action = ActionID.NO;
            }
            else if(Input.anyKeyDown)
            {
                action = ActionID.UNRECOGNIZED;
            }

            if (action != ActionID.NONE)
            {
                OnActionRequested(action);
            }
        }
    }
}
