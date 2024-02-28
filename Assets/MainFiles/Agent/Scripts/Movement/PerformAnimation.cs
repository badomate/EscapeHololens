using UnityEngine;
using Agent.Communication.Cognition;

namespace Agent.Movement
{
    public class PerformAnimation : MonoBehaviour
    {
        Animator animator;
        public bool performStandardAnimations = true;


        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        void HandleRecognitionEvent(GestureRecognized action)
        {
            if (performStandardAnimations)
            {
                switch (action)
                {
                    case GestureRecognized.AMBIGUOUS:
                        Debug.Log("Play animation AMBIGUOUS now!");
                        animator.SetTrigger("Ambiguous");
                        break;
                    case GestureRecognized.GO_FORWARD:
                        Debug.Log("Play animation GO_FORWARD now!");
                        animator.SetBool("WalkFwd", true);
                        break;
                    case GestureRecognized.GO_RIGHT:
                        Debug.Log("Play animation GO_RIGHT now!");
                        animator.SetBool("WalkRight", true);
                        break;
                    case GestureRecognized.GO_LEFT:
                        Debug.Log("Play animation GO_LEFT now!");
                        animator.SetBool("WalkLeft", true);
                        break;
                    case GestureRecognized.GO_BACKWARD:
                        Debug.Log("Play animation GO_BACKWARD now!");
                        animator.SetBool("WalkBwd", true);
                        break;
                    case GestureRecognized.TURN_LEFT:
                        Debug.Log("Play animation TURN_LEFT now!");
                        animator.SetTrigger("TurnLeft");
                        break;
                    case GestureRecognized.TURN_RIGHT:
                        Debug.Log("Play animation TURN_RIGHT now!");
                        animator.SetTrigger("TurnRight");
                        break;
                    case GestureRecognized.YES:
                        Debug.Log("Play animation YES now!");
                        animator.SetTrigger("Yes");
                        break;
                    case GestureRecognized.RED:
                        Debug.Log("Play animation RED now!");
                        animator.SetTrigger("Red");
                        break;
                    /*case Actions.VICTORY:
                        Debug.Log("Victory sign detected");
                        animator.SetTrigger("Backflip"); //needs to have the exact name of an animationController trigger (currently using DemoAnimController)
                        break;
                    case Actions.SUPERMAN:
                        Debug.Log("Flying sign detected");
                        animator.SetTrigger("Flying"); //needs to have the exact name of an animationController trigger (currently using DemoAnimController)
                        break;
                    */
                    default: //turn off all animations when gesture unrecognized
                        animator.SetBool("WalkFwd", false);
                        animator.SetBool("WalkRight", false);
                        animator.SetBool("WalkLeft", false);
                        animator.SetBool("WalkBwd", false);
                        //animator.SetBool("TurnRight", false);
                        //animator.SetBool("TurnLeft", false);
                        break;
                }
            }
        }

        bool pressing = false;
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey("1")) //this usually causes the event to fire multiple times, but that's fine, we want animations to play while the gesture is held
            {
                pressing = true;
                GestureRecognizer.RecognitionEvent.Invoke(GestureRecognized.GO_FORWARD);
            }
            else if (Input.GetKey("2"))
            {
                pressing = true;
                GestureRecognizer.RecognitionEvent.Invoke(GestureRecognized.GO_BACKWARD);
            }
            else if (Input.GetKey("3"))
            {
                pressing = true;
                GestureRecognizer.RecognitionEvent.Invoke(GestureRecognized.GO_RIGHT);
            }
            else if (Input.GetKey("4"))
            {
                pressing = true;
                GestureRecognizer.RecognitionEvent.Invoke(GestureRecognized.GO_LEFT);
            }
            else if (Input.GetKeyDown("5"))
            {
                pressing = true;
                GestureRecognizer.RecognitionEvent.Invoke(GestureRecognized.TURN_LEFT);
            }
            else if (Input.GetKeyDown("6"))
            {
                pressing = true;
                GestureRecognizer.RecognitionEvent.Invoke(GestureRecognized.TURN_RIGHT);
            }
            else if (Input.GetKeyDown("7"))
            {
                pressing = true;
                GestureRecognizer.RecognitionEvent.Invoke(GestureRecognized.YES);
            }
            else if (Input.GetKeyDown("8"))
            {
                pressing = true;
                GestureRecognizer.RecognitionEvent.Invoke(GestureRecognized.RED);
            }
            else if (Input.GetKeyDown("9"))
            {
                pressing = true;
                GestureRecognizer.RecognitionEvent.Invoke(GestureRecognized.AMBIGUOUS);
            }
            else if(pressing)
            {
                pressing = false;
                GestureRecognizer.RecognitionEvent.Invoke(GestureRecognized.UNRECOGNIZED);
            }
        }
    }
}
