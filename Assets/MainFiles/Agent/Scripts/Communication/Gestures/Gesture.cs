using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using AuxiliarContent;

namespace Agent.Communication.Gestures
{

    /// <summary> A list of sequential poses, with a specific frame (time) interval and (pose) match threshold. </summary>
    public class Gesture
    {
        /// <summary>
        /// Pose within a gesture,
        /// with information about the pose,
        /// the time between transition to the next pose,
        /// and how closely these need to match another to be called equal
        /// </summary>
        public class PoseInGesture {
            public Pose _poseToMatch;
            public float _matchThreshold; 
            public float _frameInterval;
            public float _frameIntervalThreshold;

            public PoseInGesture(Pose pose = null) {
                _poseToMatch = pose;
                _matchThreshold = 0.1f;
                _frameInterval = Mathf.Pow(30f, -1); // 30Hz / 30fps
                _frameIntervalThreshold = 0.2f;
            }
        }

        public List<PoseInGesture> _poseSequence;
        public float _matchThresholdPerPoseNr;

        public Gesture() {
            _poseSequence = new List<PoseInGesture>();
            _matchThresholdPerPoseNr = 0.5f;
        }

        /// <summary>
        /// Adds poses to a gesture's pose sequence
        /// </summary>
        public void AddPoses(List<Pose> poses, 
                                        List<float> matchThresholds = null, 
                                        List<float> frameIntervals = null,
                                        List<float> frameIntervalThresholds = null) {

            matchThresholds ??= Enumerable.Repeat(-1f, poses.Count).ToList();
            frameIntervals ??= Enumerable.Repeat(-1f, poses.Count).ToList();
            frameIntervalThresholds ??= Enumerable.Repeat(-1f, poses.Count).ToList();

            for (int i = 0; i < poses.Count; i++) {
                AddPose(poses[i], matchThresholds[i], frameIntervals[i], frameIntervalThresholds[i]);
            }
        }

        public void AddPose(Pose pose, 
                            float matchThreshold = -1f, 
                            float frameInterval = -1f,
                            float frameIntervalThreshold = -1f)
        {
            PoseInGesture poseInGesture = new PoseInGesture(pose);
            poseInGesture._matchThreshold = matchThreshold < 0 ?
                                poseInGesture._matchThreshold : matchThreshold;
            poseInGesture._frameInterval = frameInterval < 0 ?
                                poseInGesture._frameInterval : frameInterval;
            poseInGesture._frameIntervalThreshold = frameIntervalThreshold < 0 ?
                                poseInGesture._frameIntervalThreshold : frameIntervalThreshold;

            _poseSequence.Add(poseInGesture);
        }

        /// <summary>
        /// Converts a gesture to a matrix of coordinates, where each row represents a pose
        /// and each column represents a landmark.
        /// matrix[i, j] represents the position of landmark "j" in pose "i"
        /// </summary>
        public static Vector3[,] GestureToMatrix(Gesture gesture) {
            List<PoseInGesture> privatePoseSequence = gesture._poseSequence;

            Vector3[,] gestureMatrix = new Vector3 [privatePoseSequence.Count, Pose.LandmarkIds.Count];
            int nrPoses = privatePoseSequence.Count;
            int nrLandmarks = Pose.LandmarkIds.Count;

            for (int i = 0; i < nrPoses; i++) {
                for (int j = 0; j < nrLandmarks; j++) {
                    Pose.Landmark landmark = Pose.LandmarkIds[j];
                    Dictionary<Pose.Landmark, Vector3> landmarkArrangement = privatePoseSequence[i].
                                                                             _poseToMatch.
                                                                             _landmarkArrangement;

                    Vector3 pos = landmarkArrangement.ContainsKey(landmark) ?
                                            landmarkArrangement[landmark]: Vector3.zero;
                
                    gestureMatrix[i,j] = pos;
                }
            }
            return gestureMatrix;
        }

        /// <summary>
        /// Converts matrix of coordinates, where each row represents a pose
        /// and each column represents a landmark, to a gesture.
        /// matrix[i, j] represents the position of landmark "j" in pose "i"
        /// </summary>
        public static Gesture MatrixToGesture(Vector3[,] gestureMatrix)
        {
            Gesture gesture = new Gesture();
            int nrPoses = gestureMatrix.GetLength(0);
            int nrLandmarks = Pose.LandmarkIds.Count;
            CustomDebug.LogAlex("MatrixToGesture: Poses=" + nrPoses + ". LM =" + nrLandmarks +
            "\nCols =" + gestureMatrix.Length + ", Lines=" + gestureMatrix.LongLength  +
            "\nLINES =" + gestureMatrix.GetLength(0) + ", COLS=" + gestureMatrix.GetLength(1));

            for (int i = 0; i < nrPoses; i++)
            {
                Pose pose = new Pose();

                for (int j = 0; j < nrLandmarks; j++)
                {
                    Pose.Landmark landmark = Pose.LandmarkIds[j];
                    CustomDebug.LogAlex("MtG: " + i + ", "+ j);
                    pose._landmarkArrangement.Add(landmark, gestureMatrix[i, j]);
                }

                gesture.AddPose(pose);
            }
            return gesture;
        }

        /// <summary>
        /// Returns true if the none of the poses in the 
        /// other gesture's pose sequence vary more than the 
        /// match threshold from the current gesture.
        /// </summary>
        public bool GestureMatches(Gesture otherGesture) {
            for (int i = 0; i < _poseSequence.Count; i++) {
                Pose poseRef = _poseSequence[i]._poseToMatch;
                Pose poseOther = otherGesture._poseSequence[i]._poseToMatch;
                float poseMatchVariance = poseRef.MatchVariance(poseOther);

                if (poseMatchVariance > _poseSequence[i]._matchThreshold)
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
        public float GetMatchVariance(Gesture otherGesture) {
            float matchVarianceSquared = 0f;
            for (int i = 0; i < _poseSequence.Count; i++) {
                Pose poseRef = _poseSequence[i]._poseToMatch;
                Pose poseOther = otherGesture._poseSequence[i]._poseToMatch;
                float poseMatchVariance = poseRef.MatchVariance(poseOther);
 
                matchVarianceSquared += Mathf.Pow(poseMatchVariance, 2);
            }

            return Mathf.Sqrt(matchVarianceSquared);
        }

        public override string ToString()
        {
            StringBuilder gestureStringBuilder = new StringBuilder();

            for (int i = 0; i < _poseSequence.Count; i++) {
                gestureStringBuilder.Append("\nP" + i + ": ");
                gestureStringBuilder.Append(_poseSequence[i]._poseToMatch.ToString(false));
            }

            string gestureString = gestureStringBuilder.ToString();
            return gestureString;
        }

        /// <summary>
        /// Returns a list of which landmarks are used
        /// at least once
        /// by the poses inside this gesture
        /// </summary>
        public List<Pose.Landmark> relatedLandmarks()
        {
            List<Pose.Landmark> relatedLandmarkList = new List<Pose.Landmark>();
      
            for (int i = 0; i < _poseSequence.Count; i++)
            {
                Pose poseRef = _poseSequence[i]._poseToMatch;

                foreach (KeyValuePair<Pose.Landmark, Vector3> landmarkPos in poseRef._landmarkArrangement)
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
            foreach (var poseInGesture in _poseSequence)
            {
                poseInGesture._poseToMatch.RotatePose(rotation);
            }
        }
    }
}
