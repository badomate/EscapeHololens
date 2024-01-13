using UnityEngine;


public abstract class PoseSolver : ScriptableObject {
	public abstract void Init();
	public abstract void Process();
	public abstract void Close();
	public abstract Pose GetPose();
}