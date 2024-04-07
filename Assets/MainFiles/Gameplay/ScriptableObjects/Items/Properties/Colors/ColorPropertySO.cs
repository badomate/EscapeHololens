using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Items.Properties
{
    [CreateAssetMenu(fileName = "ColorPropertySO", menuName = "Gameplay/Item Properties/Color")]
    public class ColorPropertySO : PropertySO
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

        public ColorPropertySO() : base(Type.Color)
        {
            UpdateName(id.ToString());
        }

        private void OnValidate()
        {
            UpdateName(id.ToString());
        }

        public override bool EqualsSubType(PropertySO other) 
        { 
            ColorPropertySO otherColorProperty = other as ColorPropertySO;

            if (otherColorProperty == null) return false;
            
            return  id == otherColorProperty.id && 
                    replaceableMaterial == otherColorProperty.replaceableMaterial;
        }

        public override void UpdateItem(GameObject item = null)
        {
            MeshRenderer[] meshRenderers = item.GetComponentsInChildren<MeshRenderer>();

            for (int i = 0; i < meshRenderers.Length; i++)
            {
                MeshRenderer meshRenderer = meshRenderers[i];
                if (meshRenderer.sharedMaterial.Equals(replaceableMaterial))
                {
                    meshRenderer.material = matchingMaterial;
                }
            }
        }
    }
}