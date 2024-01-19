using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class ObjectManipulation
    {
        public static Transform FindFirstChildWithTag(GameObject parent, string tag)
        {
            foreach (Transform transform in parent.transform)
            {
                if (transform.CompareTag(tag))
                {
                    return transform;
                }
            }
            return null;
        }
    }

}
