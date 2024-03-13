using Agent.Communication.Cognition;
using SensorHub;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Pose = Agent.Communication.Gestures.Pose;

namespace Agent.Communication.Cognition
{
    public class GestureRecognizer : MonoBehaviour, IGestureRecognizer
    {
        private Dictionary<Pose.Landmark, Vector3>[] movementRecord;

        [SerializeField]
        private GameObject referenceCamera;

        public enum ID
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


        public UnityEvent<ID> OnGestureRecognized = new UnityEvent<ID>();

        public void Recognize(Dictionary<Pose.Landmark, Vector3>[] externalMovementRecord = null)
        {
            movementRecord = externalMovementRecord != null ?
                externalMovementRecord : new Dictionary<Pose.Landmark, Vector3>[1];

            //Watch out: Depending on mediapipe configuration, left and right hand indicators may be flipped 
            bool isLeftHandStraight = IsJointStraight(Pose.Landmark.LEFT_SHOULDER, Pose.Landmark.LEFT_ELBOW, Pose.Landmark.LEFT_WRIST, 45f);
            bool isRightHandStraight = IsJointStraight(Pose.Landmark.RIGHT_SHOULDER, Pose.Landmark.RIGHT_ELBOW, Pose.Landmark.RIGHT_WRIST, 45f);

            bool isLeftHandLeveled = IsJointLeveled(Pose.Landmark.LEFT_SHOULDER, Pose.Landmark.LEFT_WRIST, 0.3f);
            bool isRightHandLeveled = IsJointLeveled(Pose.Landmark.RIGHT_SHOULDER, Pose.Landmark.RIGHT_WRIST, 0.3f);


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
                OnGestureRecognized.Invoke(ID.VICTORY);
                Debug.Log("YES detected");
            }
            else if (isRed)
            {
                OnGestureRecognized.Invoke(ID.GO_LEFT);
                Debug.Log("RED detected");
            }
            else if (isCircle)
            {
                OnGestureRecognized.Invoke(ID.GO_RIGHT);
                Debug.Log("CIRCLE detected");
            }
            else if (isSquare)
            {
                OnGestureRecognized.Invoke(ID.TURN_LEFT);
                Debug.Log("CIRCLE detected");
            }
            /*
            if (isBlue)
            {
                RecognizeGesture.OnGestureRecognized.Invoke(ID.VICTORY);
                Debug.Log("BLUE detected");
            }
            else if (isRed)
            {
                RecognizeGesture.OnGestureRecognized.Invoke(ID.SUPERMAN);
                Debug.Log("RED detected");
            }
            else if (isHello)
            {
                RecognizeGesture.OnGestureRecognized.Invoke(ID.SUPERMAN);
                Debug.Log("HELLO detected");
            }
            else if (isYes)
            {
                RecognizeGesture.OnGestureRecognized.Invoke(ID.TURN_LEFT);
                Debug.Log("YES detected");
            }
            else if (isNo)
            {
                RecognizeGesture.OnGestureRecognized.Invoke(ID.TURN_RIGHT);
                Debug.Log("NO detected");
            }
            else if (isDirectionForward)
            {
                RecognizeGesture.OnGestureRecognized.Invoke(ID.GO_FORWARD);
                Debug.Log("FORWARD detected");
            }
            else if (isDirectionLeft)
            {
                RecognizeGesture.OnGestureRecognized.Invoke(ID.GO_LEFT);
                Debug.Log("LEFT detected");
            }
            else if (isDirectionRight)
            {
                RecognizeGesture.OnGestureRecognized.Invoke(ID.GO_RIGHT);
                Debug.Log("RIGHT detected");
            }*/

        }

        private bool IsWristRotated(bool leftHand, Quaternion targetRotation, int threshold)
        {
            Quaternion palmRotation = leftHand ? HololensHandPoseSolver.leftPalmRot : HololensHandPoseSolver.rightPalmRot;

            // Step 1: Get the inverse of the camera's rotation
            Quaternion inverseCameraRotation = Quaternion.Inverse(referenceCamera.transform.rotation);

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


        //TODO: this function parses a lot of enums as strings and vice-versa, there might be a better apporoach, perhaps by pre-defining the hierarchy of which landmarks belong to which finger or hand
        private bool IsFingerDown(Pose.Landmark fingerTip)
        {
            if (movementRecord[movementRecord.GetLength(0) - 1] == null)
            {
                return false;
            }
            Dictionary<Pose.Landmark, Vector3> poseToExamine = movementRecord[movementRecord.GetLength(0) - 1];


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
                Vector3 startVect = movementRecord[movementRecord.GetLength(0) - 1][start];
                Vector3 endVect = movementRecord[movementRecord.GetLength(0) - 1][end];

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
                Vector3 startVect = movementRecord[movementRecord.GetLength(0) - 1][start];
                Vector3 endVect = movementRecord[movementRecord.GetLength(0) - 1][end];

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
                Vector3 startVect = movementRecord[movementRecord.GetLength(0) - 1][start];
                Vector3 middleVect = movementRecord[movementRecord.GetLength(0) - 1][middle];
                Vector3 endVect = movementRecord[movementRecord.GetLength(0) - 1][end];

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
                Vector3 startVect = movementRecord[movementRecord.GetLength(0) - 1][start];
                Vector3 middleVect = movementRecord[movementRecord.GetLength(0) - 1][middle];
                Vector3 endVect = movementRecord[movementRecord.GetLength(0) - 1][end];

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
                Vector3 startVect = movementRecord[movementRecord.GetLength(0) - 1][Pose.Landmark.LEFT_FOOT];
                Vector3 endVect = movementRecord[movementRecord.GetLength(0) - 1][Pose.Landmark.RIGHT_FOOT];

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
                Vector3 startVect = movementRecord[movementRecord.GetLength(0) - 1][Pose.Landmark.LEFT_FOOT];
                Vector3 endVect = movementRecord[movementRecord.GetLength(0) - 1][Pose.Landmark.RIGHT_FOOT];

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