using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Items.Properties
{
    [CreateAssetMenu(fileName = "ShapeTraitsSO", menuName = "Gameplay/Item Properties/Shape")]
    public class ShapeTraitsSO : ScriptableObject
    {
        public enum Id
        {
            NONE,
            SQUARE,
            CIRCLE,
            TRIANGLE
        }

        public Id id;

        public Sprite representation2D;

        public GameObject representation3D;

        public AnimationClip gesture;
    }
}