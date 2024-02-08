using Agent.Communication.PoseFormatting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Agent.Communication.Gestures
{
    public class Gesture
    {
        public readonly List<Pose> poseSequence;
        public Animation animation;
        private List<float> matchThresholds;
        public readonly float matchThresholdPerPose;

        public static GestureFormatter formatter = new DefaultGestureFormatter();

        public Gesture()
        {
            poseSequence = new List<Pose>();
            matchThresholdPerPose = 0.3f;
        }

        public void AddPoses(List<Pose> poses, List<float> matchLimits = null)
        {
            poseSequence.AddRange(poses);
            List<float> newMatchThresholds =
                matchLimits == null ? 
                Enumerable.Repeat(matchThresholdPerPose, poses.Count).ToList() : matchLimits;

            matchThresholds.AddRange(newMatchThresholds);
        }

        public void AddPose(Pose pose, float matchLimit = -1f)
        {
            poseSequence.Add(pose);
            matchThresholds.Add(matchLimit < 0 ? matchThresholdPerPose : matchLimit);
        }

        /// <summary>
        /// Returns true if the none of the poses in the 
        /// other gesture's pose sequence vary more than the 
        /// match threshold from the current gesture.
        /// </summary>
        public bool GestureMatches(Gesture otherGesture)
        {
            for (int i = 0; i < poseSequence.Count; i++)
            {
                Pose poseRef = poseSequence[i];
                Pose poseOther = otherGesture.poseSequence[i];
                float poseMatchVariance = poseRef.MatchVariance(poseOther);

                if (poseMatchVariance > matchThresholds[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns a value representing how much the poses in the 
        /// other gesture's pose sequence vary from the current gestures'.
        /// </summary>
        public float GetMatchVariance(Gesture otherGesture)
        {
            float matchVarianceSquared = 0f;
            for (int i = 0; i < poseSequence.Count; i++)
            {
                Pose poseRef = poseSequence[i];
                Pose poseOther = otherGesture.poseSequence[i];
                float poseMatchVariance = poseRef.MatchVariance(poseOther);

                matchVarianceSquared += Mathf.Pow(poseMatchVariance, 2);
            }

            return Mathf.Sqrt(matchVarianceSquared);
        }

        public override string ToString()
        {
            StringBuilder gestureStringBuilder = new StringBuilder();

            for (int i = 0; i < poseSequence.Count; i++)
            {
                gestureStringBuilder.Append("\nP" + i + ": ");
                gestureStringBuilder.Append(Pose.formatter.PoseToString(poseSequence[i]));
            }

            string gestureString = gestureStringBuilder.ToString();
            return gestureString;
        }

        /// <summary>
        /// Returns a list of which landmarks are used
        /// at least once
        /// by the poses inside this gesture
        /// </summary>
        public List<Pose.Landmark> GetRelatedLandmarks()
        {
            List<Pose.Landmark> relatedLandmarkList = new List<Pose.Landmark>();

            for (int i = 0; i < poseSequence.Count; i++)
            {
                Pose poseRef = poseSequence[i];

                foreach (KeyValuePair<Pose.Landmark, Vector3> landmarkPos in poseRef.landmarkArrangement)
                {
                    relatedLandmarkList.Add(landmarkPos.Key);
                }
            }

            return relatedLandmarkList.Distinct().ToList();
        }

        /// <summary>
        /// Rotates all poses contained 
        /// in this gesture
        /// modifying every vector of every landmark contained in them.
        /// </summary>
        public void RotateGesture(Quaternion rotation)
        {
            foreach (var poseInGesture in poseSequence)
            {
                poseInGesture.RotatePose(rotation);
            }
        }


    }
}
