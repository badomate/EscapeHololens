using UnityEngine;
using Pose = Agent.Communication.Gestures.Pose;

namespace SensorHub
{
	public abstract class PoseSolver : ScriptableObject {
		public abstract void Init();
		public abstract void Process();
		public abstract void Close();
		public abstract Pose GetPose();
	}
}