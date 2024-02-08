using Agent.Communication.Gestures;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Pose = Agent.Communication.Gestures.Pose;

namespace Agent.Communication.PoseFormatting
{
    public interface GestureFormatter
    {
        public Vector3[,] GestureToPositionsMatrix(Gesture gesture);
        public Gesture PositionsMatrixToGesture(Vector3[,] positionsMatrix);

        public string GestureToString(Gesture gesture);

    }
}
