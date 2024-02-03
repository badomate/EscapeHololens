using UnityEngine;

namespace Agent.Movement
{
	public class IKDebugTarget : MonoBehaviour {
		public IKController iKController;

		[ContextMenu("Set This as Target")]
		public void SetAsTarget() {
			iKController.SetTarget(this.transform);
		}
	
		[ContextMenu("Set This as Left Target")]
		public void SetAsLeftTarget() {
			iKController.SetTargetLeft(this.transform);
		}

		[ContextMenu("Set This as Right Target")]
		public void SetAsRightTarget() {
			iKController.SetTargetRight(this.transform);
		}
	}
}