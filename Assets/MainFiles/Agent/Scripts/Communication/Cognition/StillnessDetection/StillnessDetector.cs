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

        public StillnessDetector()
        {
            this.stillnessThreshold = 0.1f;
            this.stillnessFramesRequired = 2;
            this.OnStillnessDetected = new UnityEvent();
        }

        public abstract bool IsStill(Dictionary<Gestures.Pose.Landmark, Vector3>[] movementRecord = null);
    }
}
