using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Agent.Communication.Cognition
{
    public abstract class StillnessDetector : MonoBehaviour, IStillnessDetector
    {
        public float stillnessThreshold; // used to "lock in" a pose
        public int stillnessFramesRequired;
        public UnityEvent OnStillnessDetected;

        public StillnessDetector(float stillnessThreshold, int stillnessFramesRequired)
        {
            this.stillnessThreshold = stillnessThreshold;
            this.stillnessFramesRequired = stillnessFramesRequired;
            this.OnStillnessDetected = new UnityEvent();
        }

        public void Start()
        {
            RecognitionManager recognitionManager = GetComponent<RecognitionManager>();
            recognitionManager.OnInquireStillness.AddListener(DetectStillness);
        }

        public StillnessDetector()
        {
            this.stillnessThreshold = 0.1f;
            this.stillnessFramesRequired = 2;
            this.OnStillnessDetected = new UnityEvent();
        }

        public abstract bool IsStill(Dictionary<Gestures.Pose.Landmark, Vector3>[] movementRecord = null);

        public virtual void DetectStillness(Dictionary<Gestures.Pose.Landmark, Vector3>[] playerMovementRecord = null)
        {
            if (IsStill(playerMovementRecord))
            {
                OnStillnessDetected.Invoke();
            }
        }
    }
}
