using Agent.Communication.Cognition;
using SensorHub;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Pose = Agent.Communication.Gestures.Pose;

namespace Agent.Communication.Cognition
{
    public class GestureRecognizer : MonoBehaviour, IGestureRecognizer
    {
        private Dictionary<Pose.Landmark, Vector3>[] movementRecord;
        private bool isDebugActive = false;
        [SerializeField]
        private GameObject referenceCamera;

        public UnityEvent<List<ID>> GestureRecognizedEvent = new UnityEvent<List<ID>>();

        public enum ID
        {
            G00T_NONE,

            G01T_NEW_WORD,
            G02T_ATTENTION,

            G03T_YES,
            G04T_NO,

            G05T_UNRECOGNIZED,
            G06T_AMBIGUOUS,

            G07M_GO_LEFT,
            G08M_GO_RIGHT,
            G09M_GO_FORWARD,
            G10M_GO_BACKWARD,

            G11M_TURN_LEFT,
            G12M_TURN_RIGHT,

            G13S_CIRCLE,
            G14S_SQUARE,

            G15C_RED,
            G16C_BLUE,

            G17T_VICTORY
        }

        public void Update()
        {
            if (isDebugActive)
                FakeRecognize();
        }

        public void OnGestureRecognitionRequested(Dictionary<Pose.Landmark, Vector3>[] externalMovementRecord = null)
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

            List<ID> gesturesRecognized = new List<ID>(); 
            
            if (isCircle)
            {
                gesturesRecognized.Add(ID.G13S_CIRCLE);
            }
            else if (isSquare)
            {
                gesturesRecognized.Add(ID.G14S_SQUARE);
            }
            else if (isRed)
            {
                gesturesRecognized.Add(ID.G15C_RED);
            }
            else if (isBlue)
            {
                gesturesRecognized.Add(ID.G16C_BLUE);
            }
            else if (isDirectionForward)
            {
                gesturesRecognized.Add(ID.G09M_GO_FORWARD);
            }
            else if (isDirectionRight)
            {
                gesturesRecognized.Add(ID.G08M_GO_RIGHT);
            }
            else if (isDirectionLeft)
            {
                gesturesRecognized.Add(ID.G07M_GO_LEFT);
            }
            else if (isHello)
            {
                gesturesRecognized.Add(ID.G02T_ATTENTION);
            }
            else if (isNewWord)
            {
                gesturesRecognized.Add(ID.G01T_NEW_WORD);
            }
            else if (isYes)
            {
                gesturesRecognized.Add(ID.G03T_YES);
            }
            else if (isNo)
            {
                gesturesRecognized.Add(ID.G04T_NO);
            }
            else
            {
                gesturesRecognized.Add(ID.G05T_UNRECOGNIZED);
            }

            Debug.Log("Gestures recognized: " + gesturesRecognized.ToString());
            
            if (gesturesRecognized.Count > 1)
            {
                gesturesRecognized.Clear();
                gesturesRecognized.Add(ID.G06T_AMBIGUOUS);
            }
            GestureRecognizedEvent.Invoke(gesturesRecognized);
        }

        public void FakeRecognize()
        {
            ID action = ID.G00T_NONE;

            if (Input.GetKeyDown("1")) //this usually causes the event to fire multiple times, but that's fine, we want animations to play while the gesture is held
            {
                action = ID.G16C_BLUE;
            }
            else if (Input.GetKeyDown("2"))
            {
                action = ID.G15C_RED;
            }
            else if (Input.GetKeyDown("3"))
            {
                action = ID.G14S_SQUARE;
            }
            else if (Input.GetKeyDown("4"))
            {
                action = ID.G13S_CIRCLE;
            }
            else if (Input.GetKeyDown("5"))
            {
                action = ID.G03T_YES;
            }
            else if (Input.GetKeyDown("6"))
            {
                action = ID.G04T_NO;
            }
            else if (Input.anyKeyDown)
            {
                action = ID.G05T_UNRECOGNIZED;
            }

            GestureRecognizedEvent.Invoke(new List<ID> { action });
        }

        #region Recognition Methods
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

        #endregion
    }
}