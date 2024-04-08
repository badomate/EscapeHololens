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
    // Serves as a bridge between a stillness detector and the gesture detector it activates
    [RequireComponent(typeof(StillnessDetector)), RequireComponent(typeof(GestureRecognizer))]
    public class RecognitionManager : MonoBehaviour
    {
        public static RecognitionManager instance;

        // Inquires if the player is still. Caught by the StillnessDetector class.
        public UnityEvent<Dictionary<Pose.Landmark, Vector3>[]> StillnessInquiredEvent = 
            new UnityEvent<Dictionary<Pose.Landmark, Vector3>[]>();

        public UnityEvent<Dictionary<Pose.Landmark, Vector3>[]> GestureRecognitionRequestedEvent = 
            new UnityEvent<Dictionary<Pose.Landmark, Vector3>[]>();

        public bool isRecording = true;
        
        public int recordingLength = 2; // how many frames do we save for comparison? should match the dictionary
        private int recordingProgress = 0; // how many samples of the currently playing gesture have we saved so far

        private readonly float frameInterval = 1f; // 30hz
        private float timeSinceLastFrame = 0f;

        public Dictionary<Pose.Landmark, Vector3>[] playerMovementRecord;

        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        // Start is called before the first frame update

        void Start()
        {
            playerMovementRecord = new Dictionary<Pose.Landmark, Vector3>[recordingLength];
            gameObject.GetComponent<StillnessDetector>().StillnessDetectedEvent.AddListener(OnStillnessDetected);
        }


        // Update is called once per frame
        void Update()
        {
            timeSinceLastFrame += Time.deltaTime;
            
            // Check if the desired time interval has passed (30fps)
            if (!isRecording || timeSinceLastFrame < frameInterval)
            {   // No need to check for stillness if we are not recording or if the time interval has not passed
                return;
            }
            else
            {
                SaveGestureFrame();
                timeSinceLastFrame = 0f; // Reset the time counter

                StillnessInquiredEvent.Invoke(playerMovementRecord); // Ask the stillness detector if the player is still
            }
        }

        public void OnStillnessDetected()
        {
            GestureRecognitionRequestedEvent.Invoke(playerMovementRecord);
        }

        //We save the gesture's samples received through the mediapipe stream as a matrix and keep comparing it to the goal until they match. Every row is a sample (at 30hz)
        //If the matrix is full, we will throw away the oldest sample so we can keep matrix size the same
        //TODO: we may be taking more samples than we are receiving from mediapipe if the stream rate is low. It would be good to make sure we only call this function when the player's pose has definitely been updated.
        public void SaveGestureFrame()
        {
            if (isRecording)
            {

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
    }
}