using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System;
using Agent.Communication.Gestures;
using SensorHub;
using Pose = Agent.Communication.Gestures.Pose;

namespace Agent.Communication.Cognition
{
    public enum Actions
    {
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

    public class GestureRecognizer : MonoBehaviour
    {
        public int recordingLength = 2; // how many frames do we save for comparison? should match the dictionary
        public int stillnessFramesRequired = 2;


        public bool recording = false;
        public bool stillnessCheck = false;

        public static Dictionary<Pose.Landmark, Vector3>[] playerMovementRecord;
        private int recordingProgress = 0; // how many samples of the currently playing gesture have we saved so far
        public float matchThreshold = 0.01f; // 0 would mean an absolute perfect match across all samples
        public float stillnessThreshold = 0.1f; // used to "lock in" a pose

        public static UnityEvent StillnessEvent = new UnityEvent();
        // public UnityEvent MimicEvent = new UnityEvent(); 

        public delegate void RecognitionEventDel(Actions action);
        public static RecognitionEventDel RecognitionEvent;

        // Start is called before the first frame update

        public GameObject playerCamera;
        void Start()
        {
            playerMovementRecord = new Dictionary<Pose.Landmark, Vector3>[recordingLength];
            StillnessEvent.AddListener(HandleStillness);
        }

        private void HandleStillness()
        {
            //Watch out: Depending on mediapipe configuration, left and right hand indicators may be flipped 
            bool isLeftHandStraight = IsJointStraight(Pose.Landmark.LEFT_SHOULDER, Pose.Landmark.LEFT_ELBOW, Pose.Landmark.LEFT_WRIST, 45f);
            bool isRightHandStraight = IsJointStraight(Pose.Landmark.RIGHT_SHOULDER, Pose.Landmark.RIGHT_ELBOW, Pose.Landmark.RIGHT_WRIST, 45f);

            bool isLeftHandLeveled = IsJointLeveled(Pose.Landmark.LEFT_SHOULDER, Pose.Landmark.LEFT_WRIST, 0.3f);
            bool isRightHandLeveled = IsJointLeveled(Pose.Landmark.RIGHT_SHOULDER, Pose.Landmark.RIGHT_WRIST, 0.3f);

            //Debug.Log(" LEFT_THUMB." + fingerDown(Pose.Landmark.LEFT_THUMB) + "LEFT_INDEX:" + fingerDown(Pose.Landmark.LEFT_INDEX) + "LEFT_MIDDLE:" + fingerDown(Pose.Landmark.LEFT_MIDDLE) + "LEFT_RING: " + fingerDown(Pose.Landmark.LEFT_RING) + "LEFT_PINKY: " + fingerDown(Pose.Landmark.LEFT_PINKY));
            //Debug.Log(PollHl2Hands.leftPalmRot.eulerAngles + ";" + PollHl2Hands.rightPalmRot.eulerAngles);

            //********************************** Shapes **********************************
            bool isCircle = IsFingerDown(Pose.Landmark.LEFT_THUMB) &&
                              IsFingerDown(Pose.Landmark.LEFT_INDEX) &&
                              IsFingerDown(Pose.Landmark.LEFT_MIDDLE) &&
                              IsFingerDown(Pose.Landmark.LEFT_RING) &&
                              IsFingerDown(Pose.Landmark.LEFT_PINKY);

            //replaced with "victory" sign
            bool isSquare = !IsFingerDown(Pose.Landmark.LEFT_THUMB) &&
                              IsFingerDown(Pose.Landmark.LEFT_INDEX) &&
                              IsFingerDown(Pose.Landmark.LEFT_MIDDLE) &&
                              !IsFingerDown(Pose.Landmark.LEFT_RING) &&
                              !IsFingerDown(Pose.Landmark.LEFT_PINKY);

            //********************************** Colors **********************************

            //"namaste"
            bool isBlue = (IsFingerDown(Pose.Landmark.RIGHT_INDEX) && 
                              IsFingerDown(Pose.Landmark.RIGHT_MIDDLE) &&
                              IsFingerDown(Pose.Landmark.RIGHT_RING) &&
                              IsFingerDown(Pose.Landmark.RIGHT_PINKY) &&
                              IsWristRotated(false, Quaternion.Euler(330, 240, 120), 45))
            && (!IsFingerDown(Pose.Landmark.LEFT_INDEX) &&
                              !IsFingerDown(Pose.Landmark.LEFT_MIDDLE) &&
                              !IsFingerDown(Pose.Landmark.LEFT_RING) &&
                              !IsFingerDown(Pose.Landmark.LEFT_PINKY) &&
                              !IsWristRotated(true, Quaternion.Euler(0, 130, 300), 45));

            //"spiderman"
            bool isRed = (!IsFingerDown(Pose.Landmark.RIGHT_INDEX) &&
                              IsFingerDown(Pose.Landmark.RIGHT_MIDDLE) &&
                              IsFingerDown(Pose.Landmark.RIGHT_RING) &&
                              !IsFingerDown(Pose.Landmark.RIGHT_PINKY))
            || (!IsFingerDown(Pose.Landmark.LEFT_INDEX) &&
                              IsFingerDown(Pose.Landmark.LEFT_MIDDLE) &&
                              IsFingerDown(Pose.Landmark.LEFT_RING) &&
                              !IsFingerDown(Pose.Landmark.LEFT_PINKY));
            //********************************** Directions **********************************
            bool isDirectionForward = !IsFingerDown(Pose.Landmark.LEFT_INDEX) &&
                              !IsFingerDown(Pose.Landmark.LEFT_MIDDLE) &&
                              !IsFingerDown(Pose.Landmark.LEFT_RING) &&
                              !IsFingerDown(Pose.Landmark.LEFT_PINKY) &&
                              !IsFingerDown(Pose.Landmark.LEFT_THUMB) &&
                              IsWristRotated(true, Quaternion.Euler(300, 30, 45), 45) &&
                              !IsFingerDown(Pose.Landmark.RIGHT_INDEX) &&
                              !IsFingerDown(Pose.Landmark.RIGHT_MIDDLE) &&
                              !IsFingerDown(Pose.Landmark.RIGHT_RING) &&
                              !IsFingerDown(Pose.Landmark.RIGHT_PINKY) &&
                              !IsFingerDown(Pose.Landmark.RIGHT_THUMB) &&
                              IsWristRotated(false, Quaternion.Euler(300, 0, 330), 45);


            bool isDirectionRight = !IsFingerDown(Pose.Landmark.LEFT_INDEX) &&
                              !IsFingerDown(Pose.Landmark.LEFT_MIDDLE) &&
                              !IsFingerDown(Pose.Landmark.LEFT_RING) &&
                              !IsFingerDown(Pose.Landmark.LEFT_PINKY) &&
                              !IsFingerDown(Pose.Landmark.LEFT_THUMB) &&
                              IsWristRotated(true, Quaternion.Euler(0, 250, 100), 45) &&
                              !IsFingerDown(Pose.Landmark.RIGHT_INDEX) &&
                              !IsFingerDown(Pose.Landmark.RIGHT_MIDDLE) &&
                              !IsFingerDown(Pose.Landmark.RIGHT_RING) &&
                              !IsFingerDown(Pose.Landmark.RIGHT_PINKY) &&
                              !IsFingerDown(Pose.Landmark.RIGHT_THUMB) &&
                              IsWristRotated(false, Quaternion.Euler(0, 75, 280), 45);


            bool isDirectionLeft = !IsFingerDown(Pose.Landmark.LEFT_INDEX) &&
                              !IsFingerDown(Pose.Landmark.LEFT_MIDDLE) &&
                              !IsFingerDown(Pose.Landmark.LEFT_RING) &&
                              !IsFingerDown(Pose.Landmark.LEFT_PINKY) &&
                              !IsFingerDown(Pose.Landmark.LEFT_THUMB) &&
                              IsWristRotated(true, Quaternion.Euler(0, 0, 90), 45) &&
                              !IsFingerDown(Pose.Landmark.RIGHT_INDEX) &&
                              !IsFingerDown(Pose.Landmark.RIGHT_MIDDLE) &&
                              !IsFingerDown(Pose.Landmark.RIGHT_RING) &&
                              !IsFingerDown(Pose.Landmark.RIGHT_PINKY) &&
                              !IsFingerDown(Pose.Landmark.RIGHT_THUMB) &&
                              IsWristRotated(false, Quaternion.Euler(0, 200, 260), 45);

            //********************************** Feedback **********************************
            bool isYes = IsFingerDown(Pose.Landmark.LEFT_INDEX) &&
                              IsFingerDown(Pose.Landmark.LEFT_MIDDLE) &&
                              IsFingerDown(Pose.Landmark.LEFT_RING) &&
                              IsFingerDown(Pose.Landmark.LEFT_PINKY) &&
                              !IsFingerDown(Pose.Landmark.LEFT_THUMB) &&
                              IsWristRotated(true, Quaternion.Euler(300, 80, 30), 45) ||
                              IsFingerDown(Pose.Landmark.RIGHT_INDEX) &&
                              IsFingerDown(Pose.Landmark.RIGHT_MIDDLE) &&
                              IsFingerDown(Pose.Landmark.RIGHT_RING) &&
                              IsFingerDown(Pose.Landmark.RIGHT_PINKY) &&
                              !IsFingerDown(Pose.Landmark.RIGHT_THUMB) &&
                              IsWristRotated(false, Quaternion.Euler(300, 320, 320), 45);

            bool isNo = !IsFingerDown(Pose.Landmark.LEFT_INDEX) &&
                              !IsFingerDown(Pose.Landmark.LEFT_MIDDLE) &&
                              !IsFingerDown(Pose.Landmark.LEFT_RING) &&
                              !IsFingerDown(Pose.Landmark.LEFT_PINKY) &&
                              IsWristRotated(true, Quaternion.Euler(330, 125, 270), 45) &&
                              !IsFingerDown(Pose.Landmark.RIGHT_INDEX) &&
                              !IsFingerDown(Pose.Landmark.RIGHT_MIDDLE) &&
                              !IsFingerDown(Pose.Landmark.RIGHT_RING) &&
                              !IsFingerDown(Pose.Landmark.RIGHT_PINKY) &&
                              IsWristRotated(false, Quaternion.Euler(325, 230, 90), 45);

            bool isHello = IsWristRotated(true, Quaternion.Euler(320, 90, 230), 30) &&
                              !IsFingerDown(Pose.Landmark.LEFT_INDEX) &&
                              !IsFingerDown(Pose.Landmark.LEFT_MIDDLE) &&
                              !IsFingerDown(Pose.Landmark.LEFT_RING) &&
                              !IsFingerDown(Pose.Landmark.LEFT_PINKY) &&
                              !IsFingerDown(Pose.Landmark.LEFT_THUMB) ^ //xor
                              IsWristRotated(false, Quaternion.Euler(330, 270, 180), 130) &&
                              !IsFingerDown(Pose.Landmark.RIGHT_INDEX) &&
                              !IsFingerDown(Pose.Landmark.RIGHT_MIDDLE) &&
                              !IsFingerDown(Pose.Landmark.RIGHT_RING) &&
                              !IsFingerDown(Pose.Landmark.RIGHT_PINKY) &&
                              !IsFingerDown(Pose.Landmark.RIGHT_THUMB);


            bool isNewWord = IsWristRotated(true, Quaternion.Euler(45, 250, 0), 30) &&
                              !IsFingerDown(Pose.Landmark.LEFT_INDEX) &&
                              !IsFingerDown(Pose.Landmark.LEFT_MIDDLE) &&
                              !IsFingerDown(Pose.Landmark.LEFT_RING) &&
                              !IsFingerDown(Pose.Landmark.LEFT_PINKY) &&
                              !IsFingerDown(Pose.Landmark.LEFT_THUMB);



            if (isYes)
            {
                GestureRecognizer.RecognitionEvent.Invoke(Actions.VICTORY);
                Debug.Log("YES detected");
            }
            else if (isRed)
            {
                GestureRecognizer.RecognitionEvent.Invoke(Actions.GO_LEFT);
                Debug.Log("RED detected");
            }
            else if (isCircle)
            {
                GestureRecognizer.RecognitionEvent.Invoke(Actions.GO_RIGHT);
                Debug.Log("CIRCLE detected");
            }
            else if (isSquare)
            {
                GestureRecognizer.RecognitionEvent.Invoke(Actions.TURN_LEFT);
                Debug.Log("CIRCLE detected");
            }
            /*
            if (isBlue)
            {
                RecognizeGesture.RecognitionEvent.Invoke(Actions.VICTORY);
                Debug.Log("BLUE detected");
            }
            else if (isRed)
            {
                RecognizeGesture.RecognitionEvent.Invoke(Actions.SUPERMAN);
                Debug.Log("RED detected");
            }
            else if (isHello)
            {
                RecognizeGesture.RecognitionEvent.Invoke(Actions.SUPERMAN);
                Debug.Log("HELLO detected");
            }
            else if (isYes)
            {
                RecognizeGesture.RecognitionEvent.Invoke(Actions.TURN_LEFT);
                Debug.Log("YES detected");
            }
            else if (isNo)
            {
                RecognizeGesture.RecognitionEvent.Invoke(Actions.TURN_RIGHT);
                Debug.Log("NO detected");
            }
            else if (isDirectionForward)
            {
                RecognizeGesture.RecognitionEvent.Invoke(Actions.GO_FORWARD);
                Debug.Log("FORWARD detected");
            }
            else if (isDirectionLeft)
            {
                RecognizeGesture.RecognitionEvent.Invoke(Actions.GO_LEFT);
                Debug.Log("LEFT detected");
            }
            else if (isDirectionRight)
            {
                RecognizeGesture.RecognitionEvent.Invoke(Actions.GO_RIGHT);
                Debug.Log("RIGHT detected");
            }*/

        }

        private bool IsWristRotated(bool leftHand, Quaternion targetRotation, int threshold)
        {
            Quaternion palmRotation = leftHand ? HololensHandPoseSolver.leftPalmRot : HololensHandPoseSolver.rightPalmRot;

            // Step 1: Get the inverse of the camera's rotation
            Quaternion inverseCameraRotation = Quaternion.Inverse(playerCamera.transform.rotation);

            // Step 2: Apply this inverse to the palm rotation
            Quaternion palmLocalToCamera = inverseCameraRotation * palmRotation;
        
            if (leftHand)
            {
                //Debug.Log(palmLocalToCamera.eulerAngles + "; Player camera: " + playerCamera.transform.eulerAngles);
            }

            // Step 4: Compare with target rotation
            float angleDifference = Quaternion.Angle(palmLocalToCamera, targetRotation);

            return angleDifference <= threshold;
        }

        private readonly float frameInterval = 1f / 30f; // 30hz
        private float timeSinceLastFrame = 0f;
        // Update is called once per frame
        void Update()
        {

            if (!stillnessCheck)
            {
                StillnessEvent.Invoke();
                return;
            }

            timeSinceLastFrame += Time.deltaTime;
            // Check if the desired time interval has passed (30fps)
            if (timeSinceLastFrame >= frameInterval && recording && stillnessCheck)
            {
                SaveGestureFrame();
                if (DetectStillness()) //TO-DO: reimplement the stillness check, it is likely still useful
                {
                    StillnessEvent.Invoke();
                }
                else
                {
                    GestureRecognizer.RecognitionEvent.Invoke(Actions.UNRECOGNIZED);
                }
                timeSinceLastFrame = 0f; // Reset the time counter
            }
        }

        public bool DetectStillness()
        {
            // Check if there are enough rows to check for stillness
            if (recordingLength < stillnessFramesRequired)
            {
                Debug.LogWarning("Stillness frames required are greater than the set total recording memory!");
                return false;
            }

            // the difference in each element of the last stillnessFramesRequired rows must be under threshold
            for (int recordingIndex = recordingLength - stillnessFramesRequired; recordingIndex < recordingLength - 1; recordingIndex++)
            {
                if (playerMovementRecord[recordingIndex] == null || playerMovementRecord[recordingIndex + 1] == null)
                {
                    return false; //recording is incomplete
                }

                foreach (var landmark in playerMovementRecord[recordingIndex].Keys.ToList()) //adjust hand origin
                {
                    if (playerMovementRecord[recordingIndex].ContainsKey(landmark) && playerMovementRecord[recordingIndex+1].ContainsKey(landmark) && Vector3.Distance(playerMovementRecord[recordingIndex][landmark], playerMovementRecord[recordingIndex + 1][landmark]) > stillnessThreshold)
                    {
                        return false; // Difference exceeded the threshold
                    }

                }
            }
            if (StillnessEvent != null)
            {
                return true;
            }
            else
            {
                StillnessEvent = new UnityEvent();
                return true;
            }

        }

        public bool GestureCompleted(Gesture goalGesture, Vector3[,] gestureToCompareMatrix)
        {
            Gesture gestureToCompare = Gesture.formatter.PositionsMatrixToGesture(gestureToCompareMatrix);
            return recording && recordingProgress == recordingLength && goalGesture.GestureMatches(gestureToCompare);
        }

        //We save the gesture's samples received through the mediapipe stream as a matrix and keep comparing it to the goal until they match. Every row is a sample (at 30hz)
        //If the matrix is full, we will throw away the oldest sample so we can keep matrix size the same
        //TODO: we may be taking more samples than we are receiving from mediapipe if the stream rate is low. It would be good to make sure we only call this function when the player's pose has definitely been updated.
        public void SaveGestureFrame()
        {
            if (recording)
            {
                //Dictionary<Pose.Landmark, Vector3> landmarksCopy = new Dictionary<Pose.Landmark, Vector3>(PollHl2Hands.poseDictionary);


                if (recordingProgress < recordingLength) //building up the matrix
                {
                    playerMovementRecord[recordingProgress] = new Dictionary<Pose.Landmark, Vector3>(HololensHandPoseSolver.poseDictionary);
                    recordingProgress++;
                }
                else //updating the matrix
                {
                    for (int i = 0; i < playerMovementRecord.GetLength(0) - 1; i++)
                    {
                        playerMovementRecord[i] = playerMovementRecord[i + 1];
                    }

                    playerMovementRecord[playerMovementRecord.GetLength(0) - 1] = new Dictionary<Pose.Landmark, Vector3>(HololensHandPoseSolver.poseDictionary);

                }
            }
        }

        //TODO: this function parses a lot of enums as strings and vice-versa, there might be a better apporoach, perhaps by pre-defining the hierarchy of which landmarks belong to which finger or hand
        private bool IsFingerDown(Pose.Landmark fingerTip)
        {
            if (playerMovementRecord[playerMovementRecord.GetLength(0) - 1] == null)
            {
                return false;
            }
            Dictionary<Pose.Landmark, Vector3> poseToExamine = playerMovementRecord[playerMovementRecord.GetLength(0) - 1];


            Pose.Landmark wrist;
            Pose.Landmark fingerMiddle;
            Enum.TryParse(fingerTip + "_KNUCKLE", out fingerMiddle);

            Enum.TryParse(fingerTip.ToString().Substring(0, fingerTip.ToString().IndexOf('_') + 1) + "WRIST_ROOT", out wrist);

                if (!poseToExamine.ContainsKey(fingerMiddle) || !poseToExamine.ContainsKey(wrist) || !poseToExamine.ContainsKey(fingerTip))
            {
                return false;
            }

            if (!fingerTip.ToString().Contains("THUMB"))
            {
                if (Vector3.Distance(poseToExamine[fingerMiddle], poseToExamine[wrist]) < Vector3.Distance(poseToExamine[fingerTip], poseToExamine[wrist])) //I'm just using distances here but this could be done with angles
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else //calculate the thumb, it is an exception
            {
                Pose.Landmark wristLeft;
                Pose.Landmark wristRight;
                Enum.TryParse(fingerTip.ToString().Substring(0, fingerTip.ToString().IndexOf('_') + 1) + "INDEX_BASE", out wristLeft);
                Enum.TryParse(fingerTip.ToString().Substring(0, fingerTip.ToString().IndexOf('_') + 1) + "PINKY_BASE", out wristRight);

                float toleranceThreshold = 90f;
                Vector3 wristDirection = poseToExamine[wristRight] - poseToExamine[wristLeft];
                Vector3 thumbDirection = poseToExamine[fingerTip] - poseToExamine[fingerMiddle];

                float thumbDirectionDot = Vector3.Angle(wristDirection.normalized, thumbDirection.normalized);

                bool thumbInsidePalm = thumbDirectionDot < toleranceThreshold;
                return thumbInsidePalm;
            }
        }

        private bool IsJointLeveled(Pose.Landmark start, Pose.Landmark end, float margin)
        {
            try
            {
                Vector3 startVect = playerMovementRecord[playerMovementRecord.GetLength(0) - 1][start];
                Vector3 endVect = playerMovementRecord[playerMovementRecord.GetLength(0) - 1][end];

                Vector3 v = endVect - startVect;

                v = Vector3.Normalize(v);

                return Math.Abs(v.y) < margin && Math.Abs(v.z) < margin;
            }
            catch
            {
                return false;
            }
        }

        private bool IsJointAbove(Pose.Landmark start, Pose.Landmark end, float margin)
        {
            try
            {
                Vector3 startVect = playerMovementRecord[playerMovementRecord.GetLength(0) - 1][start];
                Vector3 endVect = playerMovementRecord[playerMovementRecord.GetLength(0) - 1][end];

                Vector3 v = endVect - startVect;

                v = Vector3.Normalize(v);

                return Math.Abs(v.z) < margin && Math.Abs(v.x) < margin && 0 < v.y;
            }
            catch
            {
                return false;
            }
        }

        private bool IsJoint90Degrees(Pose.Landmark start, Pose.Landmark middle, Pose.Landmark end, float margin)
        {
            try
            {
                Vector3 startVect = playerMovementRecord[playerMovementRecord.GetLength(0) - 1][start];
                Vector3 middleVect = playerMovementRecord[playerMovementRecord.GetLength(0) - 1][middle];
                Vector3 endVect = playerMovementRecord[playerMovementRecord.GetLength(0) - 1][end];

                Vector3 V1 = middleVect - startVect;
                Vector3 V2 = endVect - middleVect;

                V1 = Vector3.Normalize(V1);
                V2 = Vector3.Normalize(V2);

                Vector3 rotationAxis = Vector3.Cross(V1, V2);

                float cosTheta = Vector3.Dot(V1, V2);

                return cosTheta < margin;
            }
            catch
            {
                return false;
            }
        }

        private bool IsJointStraight(Pose.Landmark start, Pose.Landmark middle, Pose.Landmark end, float margin)
        {
            try
            {
                Vector3 startVect = playerMovementRecord[playerMovementRecord.GetLength(0) - 1][start];
                Vector3 middleVect = playerMovementRecord[playerMovementRecord.GetLength(0) - 1][middle];
                Vector3 endVect = playerMovementRecord[playerMovementRecord.GetLength(0) - 1][end];

                Vector3 V1 = middleVect - startVect;
                Vector3 V2 = endVect - middleVect;

                V1 = Vector3.Normalize(V1);
                V2 = Vector3.Normalize(V2);

                Vector3 rotationAxis = Vector3.Cross(V1, V2);

                float cosTheta = Vector3.Dot(V1, V2);
                float theta = (float)Math.Acos(cosTheta) * 180 / (float)Math.PI;

                return theta < margin;
            }
            catch
            {
                return false;
            }
        }

        private bool IsRightLegRaised(float margin)
        {
            try
            {
                Vector3 startVect = playerMovementRecord[playerMovementRecord.GetLength(0) - 1][Pose.Landmark.LEFT_FOOT];
                Vector3 endVect = playerMovementRecord[playerMovementRecord.GetLength(0) - 1][Pose.Landmark.RIGHT_FOOT];

                Vector3 v = endVect - startVect;

                v = Vector3.Normalize(v);

                return margin < v.y;
            }
            catch
            {
                return false;
            }
        }

        private bool IsLeftLegRaised(float margin)
        {
            try
            {
                Vector3 startVect = playerMovementRecord[playerMovementRecord.GetLength(0) - 1][Pose.Landmark.LEFT_FOOT];
                Vector3 endVect = playerMovementRecord[playerMovementRecord.GetLength(0) - 1][Pose.Landmark.RIGHT_FOOT];

                Vector3 v = startVect - endVect;

                v = Vector3.Normalize(v);

                return margin < v.y;
            }
            catch
            {
                return false;
            }
        }
    }
}