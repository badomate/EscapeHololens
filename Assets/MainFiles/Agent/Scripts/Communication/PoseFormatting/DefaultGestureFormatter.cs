using Agent.Communication.Gestures;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Pose = Agent.Communication.Gestures.Pose;

namespace Agent.Communication.PoseFormatting
{
    public class DefaultGestureFormatter : GestureFormatter
    {
        public Vector3[,] GestureToPositionsMatrix(Gesture gesture)
        {
            List<Pose> poseSequence = gesture.poseSequence;

            Vector3[,] gestureMatrix = new Vector3[poseSequence.Count, Pose.MaxLandmarks];

            for (int i = 0; i < poseSequence.Count; i++)
            {
                for (int j = 0; j < Pose.MaxLandmarks; j++)
                {
                    Pose.Landmark landmark = Pose.GetLandmarkFromIndex(j);
                    Dictionary<Pose.Landmark, Vector3> landmarkArrangement = poseSequence[i].
                                                                             landmarkArrangement;

                    Vector3 pos = landmarkArrangement.ContainsKey(landmark) ?
                                            landmarkArrangement[landmark] : Pose.IRRELEVANT_POSITION;

                    gestureMatrix[i, j] = pos;
                }
            }
            return gestureMatrix;
        }

        public Gesture PositionsMatrixToGesture(Vector3[,] positionsMatrix)
        {
            Gesture gesture = new Gesture();
            int nrPoses = positionsMatrix.GetLength(0);
            int nrLandmarks = Pose.MaxLandmarks;

            for (int i = 0; i < nrPoses; i++)
            {
                Pose pose = new Pose();

                for (int j = 0; j < nrLandmarks; j++)
                {
                    if (positionsMatrix[i, j] == Pose.IRRELEVANT_POSITION)
                    {
                        continue;
                    }

                    Pose.Landmark landmark = Pose.GetLandmarkFromIndex(j);

                    pose.landmarkArrangement.Add(landmark, positionsMatrix[i, j]);
                }

                gesture.AddPose(pose);
            }
            return gesture;
        }


        public string GestureToString(Gesture gesture)
        {
            StringBuilder gestureStringBuilder = new StringBuilder();

            for (int i = 0; i < gesture.poseSequence.Count; i++)
            {
                gestureStringBuilder.Append("\nP" + i + ": ");
                gestureStringBuilder.Append(Pose.formatter.PoseToString(gesture.poseSequence[i], false));
            }

            string gestureString = gestureStringBuilder.ToString();
            return gestureString;
        }
    }
}
