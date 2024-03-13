using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Agent.Communication.Cognition
{
    public class AlwaysTrueStillnessDetector : StillnessDetector
    {
        public override bool IsStill(Dictionary<Gestures.Pose.Landmark, Vector3>[] movementRecord = null)
        {
            return true;
        }
    }
}
