using Agent.Communication.Gestures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Pose = Agent.Communication.Gestures.Pose;

namespace Agent.Communication.Cognition
{
    public interface IGestureRecognizer
    {
        public void OnGestureRecognitionRequested(Dictionary<Pose.Landmark, Vector3>[] movementRecord = null);
    }
}