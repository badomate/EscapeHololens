using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Agent.Communication.Cognition
{
    public abstract class StillnessDetector : MonoBehaviour, IStillnessDetector
    {
        public float stillnessThreshold; // used to "lock in" a pose
        public int stillnessFramesRequired;
        public UnityEvent StillnessDetectedEvent;

        public StillnessDetector(float stillnessThreshold, int stillnessFramesRequired)
        {
            this.stillnessThreshold = stillnessThreshold;
            this.stillnessFramesRequired = stillnessFramesRequired;
            this.StillnessDetectedEvent = new UnityEvent();
        }

        public void Start()
        {
            RecognitionManager recognitionManager = GetComponent<RecognitionManager>();
            recognitionManager.StillnessInquiredEvent.AddListener(OnStillnessInquired);
        }

        public StillnessDetector()
        {
            this.stillnessThreshold = 0.1f;
            this.stillnessFramesRequired = 2;
            this.StillnessDetectedEvent = new UnityEvent();
        }

        public abstract bool IsStill(Dictionary<Gestures.Pose.Landmark, Vector3>[] movementRecord = null);

        public virtual void OnStillnessInquired(Dictionary<Gestures.Pose.Landmark, Vector3>[] playerMovementRecord = null)
        {
            if (IsStill(playerMovementRecord))
            {
                StillnessDetectedEvent.Invoke();
            }
        }
    }
}
