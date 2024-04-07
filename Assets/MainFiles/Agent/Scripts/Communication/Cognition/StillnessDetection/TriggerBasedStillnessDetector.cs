using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Agent.Communication.Cognition
{
    public class TriggerBasedStillnessDetector : StillnessDetector
    {
        public override bool IsStill(Dictionary<Gestures.Pose.Landmark, Vector3>[] movementRecord = null)
        {
            return Input.anyKey;
        }
    }
}
