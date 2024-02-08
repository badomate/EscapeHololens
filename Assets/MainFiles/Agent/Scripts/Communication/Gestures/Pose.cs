using Agent.Communication.PoseFormatting;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Agent.Communication.Gestures
{

    /// <summary> A pose corresponds to a specific (and static) set of landmark positions. </summary>
    public class Pose
    {
        /// <summary>
        /// Valid landmarks. Each correspond to a point in the player's body.
        /// </summary>
        public enum Landmark
        {
            //base landmarks (endpoints)
            LEFT_WRIST,
            RIGHT_WRIST,
            LEFT_FOOT,
            RIGHT_FOOT,

            //hint landmarks
            RIGHT_ELBOW,
            LEFT_ELBOW,
            RIGHT_KNEE,
            LEFT_KNEE,
            LEFT_SHOULDER,
            RIGHT_SHOULDER,

            NOSE,
            RIGHT_EAR,
            LEFT_EAR,

            //hand landmarks
            LEFT_WRIST_ROOT,
            RIGHT_WRIST_ROOT,

            RIGHT_INDEX,
            RIGHT_THUMB,
            RIGHT_RING,
            RIGHT_PINKY,
            RIGHT_MIDDLE,

            LEFT_INDEX,
            LEFT_THUMB,
            LEFT_RING,
            LEFT_PINKY,
            LEFT_MIDDLE,

            RIGHT_INDEX_KNUCKLE,
            RIGHT_THUMB_KNUCKLE,
            RIGHT_RING_KNUCKLE,
            RIGHT_PINKY_KNUCKLE,
            RIGHT_MIDDLE_KNUCKLE,

            LEFT_INDEX_KNUCKLE,
            LEFT_THUMB_KNUCKLE,
            LEFT_RING_KNUCKLE,
            LEFT_PINKY_KNUCKLE,
            LEFT_MIDDLE_KNUCKLE,

            RIGHT_INDEX_BASE,
            RIGHT_THUMB_BASE,
            RIGHT_RING_BASE,
            RIGHT_PINKY_BASE,
            RIGHT_MIDDLE_BASE,

            LEFT_INDEX_BASE,
            LEFT_THUMB_BASE,
            LEFT_RING_BASE,
            LEFT_PINKY_BASE,
            LEFT_MIDDLE_BASE,

            LEFT_HIP,
            RIGHT_HIP,
        }

        [SerializeField]
        public static PoseFormatter formatter = new HololensStreamPoseFormatter();

        public static Vector3 IRRELEVANT_POSITION { get { return PoseFormatter.IRRELEVANT_POSITION; } }

        public static int MaxLandmarks {get { return formatter.LandmarkIndexRegistry.Count; } }
        public int NumLandmarks { get { return landmarkArrangement.Count; } }

        private const float DEFAULT_VARIANCE = 3;

        /// <summary> 
        /// Match between a landmark and its position on the pose, relative to the player's location.
        /// Not all landmarks are relevant for a given pose. 
        /// </summary>
        public Dictionary<Landmark, Vector3> landmarkArrangement;

        public Pose() {
            landmarkArrangement = new Dictionary<Landmark, Vector3>();
        }

        public Pose(Dictionary<Landmark, Vector3> arranjement) {
            landmarkArrangement = arranjement;
        }

        public static Landmark GetLandmarkFromIndex(int index) {
            return formatter.LandmarkIndexRegistry.GetValue(index);
        }

        public static int GetIndexFromLandmark(Landmark landmark)
        {
            return formatter.LandmarkIndexRegistry.GetValue(landmark);
        }

        /// <summary> Variance between the landmark positions of two poses. </summary>
        public float MatchVariance(Pose otherPose) {
            float matchVarianceSquared = 0f;
            foreach(KeyValuePair<Landmark, Vector3> landmarkPosition in landmarkArrangement) {
                float landmarkVarianceSquared =  otherPose.landmarkArrangement.ContainsKey(landmarkPosition.Key)?
                    GetLandmarkVarianceSquared(
                        landmarkPosition.Value, // Landmark position in this pose
                        otherPose.landmarkArrangement[landmarkPosition.Key] // Landmark position in the other pose
                    ) : DEFAULT_VARIANCE;     
                matchVarianceSquared += landmarkVarianceSquared;
            }

            return Mathf.Sqrt(matchVarianceSquared);
        }

        /// <summary> Returns the variance between a landmark's position and its reference value </summary>
        public float GetLandmarkVariance(Vector3 reference, Vector3 toCompare) {
            float varianceSquared = GetLandmarkVarianceSquared(reference, toCompare);
            return Mathf.Sqrt(varianceSquared);
        }

        /// <summary> 
        /// Returns the squared variance between 
        /// a landmark's position and its reference value 
        /// </summary>
        public float GetLandmarkVarianceSquared(Vector3 reference, Vector3 toCompare) {
            float varianceSquared = 0f;

            for (int i = 0; i < 3; i++) {
                varianceSquared += Mathf.Pow(
                    reference[i]-toCompare[i], 
                    2);
            }

            return varianceSquared;
        }

        public void RotatePose(Quaternion rotation)
        {
            foreach (var landmark in landmarkArrangement.Keys.ToList())
            {
                Vector3 originalPosition = landmarkArrangement[landmark];
                Vector3 rotatedPosition = rotation * originalPosition;
                landmarkArrangement[landmark] = rotatedPosition;
            }
        }
    }
}
