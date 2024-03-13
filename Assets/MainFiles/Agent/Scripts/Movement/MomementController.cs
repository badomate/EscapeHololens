using UnityEngine;
using Agent.Communication.Cognition;
using System.Collections.Generic;
using ActionID = Agent.Communication.Cognition.AIController.ActionID;

namespace Agent.Movement
{
    public class MomementController : MonoBehaviour
    {
        IKController ikController;

        [SerializeField]
        private bool isDebugOn = true;
        private bool isButtonPressed = false;

        Animator animator;
        public bool performStandardAnimations = true;

        [SerializeField]
        private Dictionary<ActionID, string> actionMatcher =
            new Dictionary<ActionID, string> {
                { ActionID.AMBIGUOUS, "Ambiguous" },
                { ActionID.GO_FORWARD, "WalkFwd" },
                { ActionID.GO_RIGHT, "WalkRight" },
                { ActionID.GO_LEFT, "WalkLeft" },
                { ActionID.GO_BACKWARD, "WalkBwd" },
                { ActionID.TURN_LEFT, "TurnLeft" },
                { ActionID.TURN_RIGHT, "TurnRight" },
                { ActionID.YES, "Yes" },
                { ActionID.RED, "Default State.red" }
        };

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            ikController = GetComponent<IKController>();
        }

        public void PerformAction(ActionID action)
        {
            if (actionMatcher.TryGetValue(action, out string animationID))
            {
                Debug.Log("Play animation " + animationID + " now!");
                animator.Play(animationID, 2);
            }
            else
            {
                DeactivateWalkAnimations();
            }
        }

        public void ReachObject(GameObject target)
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

            if (Input.GetKey("1")) //this usually causes the event to fire multiple times, but that's fine, we want animations to play while the gesture is held
            {
                isButtonPressed = true;
                action = ActionID.GO_FORWARD;
            }
            else if (Input.GetKey("2"))
            {
                isButtonPressed = true;
                action = ActionID.GO_BACKWARD;
            }
            else if (Input.GetKey("3"))
            {
                isButtonPressed = true;
                action = ActionID.GO_RIGHT;
            }
            else if (Input.GetKey("4"))
            {
                isButtonPressed = true;
                action = ActionID.GO_LEFT;
            }
            else if (Input.GetKeyDown("5"))
            {
                isButtonPressed = true;
                action = ActionID.TURN_LEFT;
            }
            else if (Input.GetKeyDown("6"))
            {
                isButtonPressed = true;
                action = ActionID.TURN_RIGHT;
            }
            else if (Input.GetKeyDown("7"))
            {
                isButtonPressed = true;
                action = ActionID.YES;
            }
            else if (Input.GetKeyDown("8"))
            {
                Debug.Log("Pressed RED");
                isButtonPressed = true;
                action = ActionID.RED;
            }
            else if (Input.GetKey("9"))
            {
                isButtonPressed = true;
                action = ActionID.AMBIGUOUS;
            }
            else if(isButtonPressed)
            {
                isButtonPressed = false;
                action = ActionID.UNRECOGNIZED;
            }

            PerformAction(action);
        }
    }
}
