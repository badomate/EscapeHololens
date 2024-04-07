using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Items.Properties
{

    [Obsolete("[Property]Traits classes are deprecated, please use PropertySO and its subclasses instead.", error: true)]
    [CreateAssetMenu(fileName = "ColorTraitsSO", menuName = "Gameplay/Item Properties/Color")]
    public class ColorTraitsSO: ScriptableObject
    {
        // As per the HSV model
        public enum Id
        {
            NONE,
            RED,
            BLUE,
            GREEN,
            YELLOW
        }

        public Id id;
        public Color matchingColor;
        public Material matchingMaterial;
        public Material replaceableMaterial;
        public AnimationClip gesture;
        public bool Equals(ColorTraitsSO other) 
        { 
            return  id == other.id && 
                    replaceableMaterial == other.replaceableMaterial;
        }
    }
}