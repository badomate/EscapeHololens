using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pose = Agent.Communication.Gestures.Pose;

namespace SensorHub
{
    [CreateAssetMenu(fileName = "CombinedPS", menuName = "Pose Solver/Combined", order = 0)]
    public class CombinedPoseSolver : PoseSolver
    {
        public Pose combinedPlayerPose;
	    public PoseSolver[] solversToCombine;
        //Customize the following lists in the inspector view to set which landmarks should come from HL2. If a listed landmark appears in both sources, HL2 will be used instead.
        //For example, legs will come from the mediapipe camera but we might want finger landmarks (when possible) from HL2
        public List<Pose.Landmark> landmarksFromHL2;

        public override void Init()
        {
            solversToCombine.ToList().ForEach((s) => s.Init());
        }
        public override void Process()
        {
            solversToCombine.ToList().ForEach((s) => s.Process());
		    Dictionary<Pose.Landmark, Vector3> dictionary = new();
		    solversToCombine.ToList().ForEach(
			    (s) => {

				    dictionary = dictionary.Concat(s.GetPose().landmarkArrangement.Where(kv => landmarksFromHL2.Contains(kv.Key))
                    .ToDictionary(kv => kv.Key, kv => kv.Value)).ToDictionary(kv => kv.Key, kv => kv.Value);
			    });
		    combinedPlayerPose = new Pose(dictionary);
        }
        public override void Close()
        {
            solversToCombine.ToList().ForEach((s) => s.Close());
        }

        public override Pose GetPose()
        {
            return combinedPlayerPose;
        }

        // Method to combine dictionaries based on landmark lists
        public void CombineDictionaries()
        {
            /*//TODO: Consider making this more dynamic. If mediapipe has finger landmarks available, and they arent visible to the HL2 camera, then fall back to mediapipe info.
            if (Socket_toHl2.hololensPlayerPose != null && CameraStream.playerPose != null 
                && Socket_toHl2.hololensPlayerPose._landmarkArrangement.Count > 0 && CameraStream.playerPose._landmarkArrangement.Count > 0)
            {
                var combinedLandmarksHL2 = HololensPoseSolver.hololensPlayerPose._landmarkArrangement.Where(kv => landmarksFromHL2.Contains(kv.Key))
                    .ToDictionary(kv => kv.Key, kv => kv.Value);

                var combinedLandmarksCamera = CameraStreamPoseSolver.playerPose._landmarkArrangement.Where(kv => !combinedLandmarksHL2.ContainsKey(kv.Key))
                    .ToDictionary(kv => kv.Key, kv => kv.Value);

                var combinedDictionaries = combinedLandmarksHL2.Concat(combinedLandmarksCamera)
                    .ToDictionary(kv => kv.Key, kv => kv.Value);

                combinedPlayerPose = new Pose(combinedDictionaries);
            } */
        }
    }
}
