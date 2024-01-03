using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Items
{
    [CreateAssetMenu(fileName = "ColorTraits", menuName = "Gameplay/Properties")]
    public class ColorTraitsSO : ScriptableObject
    {
        // As per the HSL model
        public enum Id
        {
            NONE,
            RED,
            BLUE
        }

        public Id id;
        public Color matchingColor;
        public Material matchingMaterial;

        public bool Equals(ColorTraitsSO other) { return id == other.id; }
    }
}