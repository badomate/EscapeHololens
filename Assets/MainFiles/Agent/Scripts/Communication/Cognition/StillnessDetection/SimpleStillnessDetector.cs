using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Agent.Communication.Cognition
{
    public class SimpleStillnessDetector : StillnessDetector
    {
        public override bool IsStill(Dictionary<Gestures.Pose.Landmark, Vector3>[] movementRecord = null)
        {
            int recordingLength = movementRecord.Length;

            if (movementRecord == null)
                return true;

            // Check if there are enough rows to check for stillness
            if (recordingLength < stillnessFramesRequired)
            {
                Debug.LogWarning("Stillness frames required are greater than the set total recording memory!");
                return false;
            }

            // the difference in each element of the last stillnessFramesRequired rows must be under threshold
            for (int recordingIndex = recordingLength - stillnessFramesRequired; recordingIndex < recordingLength - 1; recordingIndex++)
            {
                if (movementRecord[recordingIndex] == null || movementRecord[recordingIndex + 1] == null)
                {
                    return false; //recording is incomplete
                }

                foreach (var landmark in movementRecord[recordingIndex].Keys.ToList()) //adjust hand origin
                {
                    if (movementRecord[recordingIndex].ContainsKey(landmark) && movementRecord[recordingIndex + 1].ContainsKey(landmark) && Vector3.Distance(movementRecord[recordingIndex][landmark], movementRecord[recordingIndex + 1][landmark]) > stillnessThreshold)
                    {
                        return false; // Difference exceeded the threshold
                    }

                }
            }

            if (OnStillnessDetected != null)
            {
                OnStillnessDetected.Invoke();
                return true;
            }
            else
            {
                OnStillnessDetected = new UnityEvent();
                return true;
            }
        }
    }
}
