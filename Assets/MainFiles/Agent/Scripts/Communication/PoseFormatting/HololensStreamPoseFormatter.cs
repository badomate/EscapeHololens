using AuxiliarContent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using Utilities;
using static Agent.Communication.Gestures.Pose;
using Pose = Agent.Communication.Gestures.Pose;

namespace Agent.Communication.PoseFormatting
{
    public class HololensStreamPoseFormatter: PoseFormatter
    {

        public static Vector3 IRRELEVANT_POSITION { get; set; } = new Vector3(-1, -1, -1);

        public int numLandmarks { get; set; }

        /// <summary> Format: " LM0 Position=[ lm00Pos_x, lm00Pos_y,  lm00Pos_z]. LM1 Position=[ lm01Pos_x, lm01Pos_y,  lm01Pos_z]." </summary>
        public Regex poseRegex { get; set; }

        public TwoWayDictionary<Landmark, int> LandmarkIndexRegistry { get; set; }
        public TwoWayDictionary<Landmark, List<int>> LandmarkLimbRegistry { get; set; }


        public HololensStreamPoseFormatter()
        {
            poseRegex = new Regex("Position=\\[\\s(?<x>-?\\d+(?:\\.\\d+)?),\\s(?<y>-?\\d+(?:\\.\\d+)?),\\s\\s(?<z>-?\\d+(?:\\.\\d+)?)\\]");

            LandmarkIndexRegistry = new TwoWayDictionary<Pose.Landmark, int>();
            LandmarkLimbRegistry = new TwoWayDictionary<Pose.Landmark, List<int>>();

            FillLandmarkRegistries();

            numLandmarks = LandmarkIndexRegistry.Count;
        }

        private void FillLandmarkRegistries()
        {
            FillIndexRegistries();
            FillLimbRegistries();
        }

        private void FillIndexRegistries()
        {
            // TODO: Fill the landmark-index registry
        }

        private void FillLimbRegistries()
        {
            // TODO: Fill the landmark-limb registry
        }

        #region POSE_STRING

        public string PoseToString(Pose pose, bool doUseAllLandmarks = false)
        {
            StringBuilder poseStringBuilder = new StringBuilder();

            for (int i = 0; i < Pose.MaxLandmarks; i++)
            {
                Landmark landmark = LandmarkIndexRegistry.GetValue(i);
                bool isLandmarkRelevant = pose.landmarkArrangement.
                                                        TryGetValue(landmark, out Vector3 landmarkPos);
                if (!isLandmarkRelevant)
                    landmarkPos = Pose.IRRELEVANT_POSITION;

                if (doUseAllLandmarks)
                {
                    string landmarkPosString =
                        $" {landmark}: Position=[{landmarkPos[0]}, {landmarkPos[1]},  {landmarkPos[2]}].";
                    poseStringBuilder.Append(landmarkPosString);
                }
            }

            string poseString = poseStringBuilder.ToString();
            return poseString;
        }

        public Pose StringToPose(string poseString)
        {
            Pose pose = new Pose();

            MatchCollection landmarkPositions = poseRegex.Matches(poseString);
            int nrLandmarksRegistered = 0;

            foreach (Match landmarkPosition in landmarkPositions.Cast<Match>())
            {
                GroupCollection landmarkCoordinate = landmarkPosition.Groups;
                Landmark currentLandmarkId = LandmarkIndexRegistry.GetValue(nrLandmarksRegistered);
                pose.landmarkArrangement[currentLandmarkId] =
                    new Vector3(
                        float.Parse(landmarkCoordinate["x"].Value),
                        float.Parse(landmarkCoordinate["y"].Value),
                        float.Parse(landmarkCoordinate["z"].Value))
                    ;

                nrLandmarksRegistered++;
            }

            return pose;
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
            MatchCollection landmarkPositions = poseRegex.Matches(poseString);
            int nrLandmarksRegistered = 0;

            int totalLandmarks = landmarkPositions.Count;

            Vector3[] poseVector = new Vector3[totalLandmarks];

            foreach (Match landmarkPosition in landmarkPositions.Cast<Match>())
            {
                GroupCollection landmarkCoordinate = landmarkPosition.Groups;
                Landmark currentLandmarkId = LandmarkIndexRegistry.GetValue(nrLandmarksRegistered);
                Vector3 landmarkVector = new Vector3(
                        float.Parse(landmarkCoordinate["x"].Value),
                        float.Parse(landmarkCoordinate["y"].Value),
                        float.Parse(landmarkCoordinate["z"].Value))
                    ;

                Debug.Log("Pose[" + currentLandmarkId + "]: " + landmarkVector);

                poseVector[nrLandmarksRegistered] = landmarkVector;
                nrLandmarksRegistered++;
            }

            return poseVector;
        }

        #endregion
    }
}
