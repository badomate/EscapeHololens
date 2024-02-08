using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Utilities;
using Pose = Agent.Communication.Gestures.Pose;

namespace Agent.Communication.PoseFormatting
{
    public interface PoseFormatter
    {
        Regex poseRegex { get; set; }

        public static Vector3 IRRELEVANT_POSITION { get; set; }

        public TwoWayDictionary<Pose.Landmark, int> LandmarkIndexRegistry { get; set; }
        public TwoWayDictionary<Pose.Landmark, List<int>> LandmarkLimbRegistry { get; set; }
        
        public string PoseToString(Pose pose, bool doUseAllLandmarks = false);
        public Pose StringToPose(string poseString);

        public Pose VectorArrayToPose(Vector3[] poseArray);
        public Vector3[] PoseToVectorArray(Pose pose);

        public Vector3[] StringToVectorArray(string poseString);

    }
}
