using Agent.Communication.Cognition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agent.Movement
{
    public class IKController : MonoBehaviour
    {
        Animator animator;
    
        bool active = false;
        Transform rightTargetOld;
        Transform leftTargetOld;
        Transform rightTarget;
        Transform leftTarget;

        public float interpolationTime = 1.0f;
        float interpolationIKValueRight = 0;
        float interpolationIKValueLeft = 0;
        float interpolationValueRight = 0;
        float interpolationValueLeft = 0;
        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        [ContextMenu("Set Idle")]
        public void SetIdle() {
            active = false;
        }

        public void SetTargetLeft(Transform ikTarget) {
            interpolationValueLeft = 0;
            leftTargetOld = leftTarget;
            leftTarget = ikTarget;
            active = true;
        }   

        public void SetTargetRight(Transform ikTarget) {
            interpolationValueRight = 0;
            rightTargetOld = rightTarget;
            rightTarget = ikTarget;
            active = true;
        }   

        public void SetTarget(Transform ikTarget) {
            if(Vector3.Dot(ikTarget.position - transform.position, transform.right) > 0) {
                SetTargetRight(ikTarget);
            }
            else {
                SetTargetLeft(ikTarget);
            }
        }

        public void SetWeightValue(float weight) {
            interpolationIKValueRight = weight;
            interpolationIKValueLeft = weight;
        }

        private void Update() {
            if(active) {
                if(rightTarget != null) interpolationIKValueRight += 1/interpolationTime * Time.deltaTime;
                if(leftTarget != null) interpolationIKValueLeft += 1/interpolationTime * Time.deltaTime;
                interpolationValueRight += 1/interpolationTime * Time.deltaTime;
                interpolationValueLeft += 1/interpolationTime * Time.deltaTime;
            }
            else {
                interpolationIKValueRight -= 1/interpolationTime * Time.deltaTime;
                interpolationIKValueLeft -= 1/interpolationTime * Time.deltaTime;
            }
        }

        private void OnAnimatorIK(int layerIndex) {
            if(active) {
                if(interpolationIKValueRight > 1) {
                    interpolationIKValueRight = 1;
                }
                if(interpolationIKValueLeft > 1) {
                    interpolationIKValueLeft = 1;
                }
                if(interpolationValueRight > 1) {
                    interpolationValueRight = 1;
                }
                if(interpolationValueLeft > 1) {
                    interpolationValueLeft = 1;
                }

                if(rightTarget != null) {
                    Vector3 target = rightTarget.position;
                    if(rightTargetOld != null) {
                        target = Vector3.Lerp(rightTargetOld.position, rightTarget.position, interpolationValueRight);
                    }
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, interpolationIKValueRight);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, target);
                    animator.SetLookAtPosition(target);
                }
                else {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand,interpolationIKValueRight);
                    animator.SetLookAtWeight(interpolationIKValueRight);
                }
                if(leftTarget != null) {
                    Vector3 target = leftTarget.position;
                    if(leftTargetOld != null) {
                        target = Vector3.Lerp(leftTargetOld.position, leftTarget.position, interpolationValueLeft);
                    }
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, interpolationIKValueLeft);
                    animator.SetIKPosition(AvatarIKGoal.LeftHand,target);
                    animator.SetLookAtPosition(target);
                }
                else {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand,interpolationIKValueLeft);
                    animator.SetLookAtWeight(interpolationIKValueLeft);
                }
            }
            else {          
                if(interpolationIKValueRight < 0) {
                    rightTarget = null;
                    interpolationIKValueRight = 0;
                }
                if(interpolationIKValueLeft < 0) {
                    leftTarget = null;
                    interpolationIKValueLeft = 0;
                }
                if(interpolationValueRight < 0) {
                    interpolationValueRight = 0;
                }
                if(interpolationValueLeft < 0) {
                    interpolationValueLeft = 0;
                }
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand,interpolationIKValueRight);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand,interpolationIKValueLeft);
                animator.SetLookAtWeight(Mathf.Max(interpolationIKValueRight, interpolationIKValueLeft));
            }
        }

    }
}