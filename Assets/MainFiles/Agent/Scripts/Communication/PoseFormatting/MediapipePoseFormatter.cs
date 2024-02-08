using AuxiliarContent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Utilities;
using static Agent.Communication.Gestures.Pose;
using Pose = Agent.Communication.Gestures.Pose;

namespace Agent.Communication.PoseFormatting
{
    public class MediapipeStreamPoseFormatter: PoseFormatter
    {

        public static Vector3 IRRELEVANT_POSITION { get; set; } = new Vector3(-1, -1, -1);

        public int numLandmarks { get; set; }

        /// <summary> Format: " LM0 Position=[ lm00Pos_x, lm00Pos_y,  lm00Pos_z]. LM1 Position=[ lm01Pos_x, lm01Pos_y,  lm01Pos_z]." </summary>
        public Regex poseRegex { get; set; }
        public TwoWayDictionary<Landmark, int> LandmarkIndexRegistry { get; set; }
        public TwoWayDictionary<Landmark, List<int>> LandmarkLimbRegistry { get; set; }

        public MediapipeStreamPoseFormatter()
        {
            poseRegex = new Regex("");
            
            LandmarkIndexRegistry = new TwoWayDictionary<Pose.Landmark, int>();
            LandmarkLimbRegistry = new TwoWayDictionary<Pose.Landmark, List<int>>();
            
            FillLandmarkRegistries();

            numLandmarks = LandmarkIndexRegistry.Count;
        }

        private void FillLandmarkRegistries()
        {
            FillIndexRegistry();
            FillLimbRegistry();
        }

        private void FillIndexRegistry()
        {
            LandmarkIndexRegistry.Add(Pose.Landmark.LEFT_WRIST, 15);
            LandmarkIndexRegistry.Add(Pose.Landmark.RIGHT_WRIST, 16);
            LandmarkIndexRegistry.Add(Pose.Landmark.LEFT_FOOT, 27);
            LandmarkIndexRegistry.Add(Pose.Landmark.RIGHT_FOOT, 28);


            LandmarkIndexRegistry.Add(Pose.Landmark.LEFT_KNEE, 25);
            LandmarkIndexRegistry.Add(Pose.Landmark.RIGHT_KNEE, 26);
            LandmarkIndexRegistry.Add(Pose.Landmark.LEFT_ELBOW, 13);
            LandmarkIndexRegistry.Add(Pose.Landmark.RIGHT_ELBOW, 14);

            //LandmarkIndexRegistry.Add(Pose.Landmark.RIGHT_INDEX, 20);

        }

        private void FillLimbRegistry()
        {
            LandmarkLimbRegistry.Add(Pose.Landmark.LEFT_WRIST, new List<int> { 11, 13, 15, 17, 19, 21 });
            LandmarkLimbRegistry.Add(Pose.Landmark.RIGHT_WRIST, new List<int> { 12, 14, 16, 18, 20, 22 });
            LandmarkLimbRegistry.Add(Pose.Landmark.LEFT_FOOT, new List<int> { 25, 27, 29, 31 });
            LandmarkLimbRegistry.Add(Pose.Landmark.RIGHT_FOOT, new List<int> { 26, 28, 30, 32 });

        }

        #region POSE_STRING

        public string PoseToString(Pose pose, bool doUseAllLandmarks = false)
    {
            throw new System.NotImplementedException();
        }

        public Pose StringToPose(string poseString)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region POSE_VECTOR_ARRAY

        public Pose VectorArrayToPose(Vector3[] poseArray)
    {
            Pose pose = new Pose();

            for (int i = 0; i < LandmarkIndexRegistry.Count; i++)
            {
                Landmark landmark = LandmarkIndexRegistry.GetValue(i);
                Vector3 landmarkPosition = poseArray[i];

                bool isLandmarkRelevant = !landmarkPosition.Equals(PoseFormatter.IRRELEVANT_POSITION);
                if (isLandmarkRelevant)
                    pose.landmarkArrangement.Add(landmark, landmarkPosition);
            }
            return pose;
        }

        public Vector3[] PoseToVectorArray(Pose pose)
        {
            int totalLandmarks = LandmarkIndexRegistry.Count;
            Vector3[] poseVectorArray = new Vector3[totalLandmarks];

            for (int i = 0; i < totalLandmarks; i++)
            {
                Landmark landmark = LandmarkIndexRegistry.GetValue(i);
                bool isLandmarkRelevant = pose.landmarkArrangement.
                                                        TryGetValue(landmark, out Vector3 landmarkPosition);
                if (!isLandmarkRelevant)
                    landmarkPosition = Vector3.zero;

                poseVectorArray[i] = landmarkPosition;
            }
            return poseVectorArray;
        }

        #endregion

        #region STRING_VECTOR_ARRAY

        public Vector3[] StringToVectorArray(string poseString)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
