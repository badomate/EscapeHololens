using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Items
{
    [CreateAssetMenu(fileName = "ColorTraitsSO", menuName = "Gameplay/ItemsProperties")]
    public class ColorTraitsSO : ScriptableObject
    {
        // As per the HSV model
        public enum Id
        {
            NONE,
            RED,
            BLUE
        }

        public Id id;
        public Color matchingColor;
        public Material matchingMaterial;
        public Material replaceableMaterial;

        public bool Equals(ColorTraitsSO other) 
        { 
            return  id == other.id && 
                    replaceableMaterial == other.replaceableMaterial;
        }
    }
}