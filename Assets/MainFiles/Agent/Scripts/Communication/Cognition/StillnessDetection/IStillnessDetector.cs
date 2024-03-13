using Agent.Communication.Gestures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Agent.Communication.Cognition
{
    public interface IStillnessDetector
    {
        public bool IsStill(Dictionary<Gestures.Pose.Landmark, Vector3>[] playerMovementRecord = null);
    }
}
