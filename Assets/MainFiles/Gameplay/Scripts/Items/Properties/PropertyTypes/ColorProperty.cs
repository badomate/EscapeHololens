using System;
using UnityEngine;

namespace Gameplay.Items.Properties
{
    // May add more color traits (saturation, luminosity, ...)

    [Obsolete("Property class is deprecated, please use PropertySO and its subclasses instead.", error:true)]
    public class ColorProperty : Property
    {
        [field: SerializeField]
        public ColorTraitsSO ColorTraitsSO { get; private set;}

        public ColorProperty() : base(PropertyType.Color)
        {
        }

        public void Start()
        {
            UpdateItemColor();
        }


        #region PROPERTY_SPECIFIC_METHODS

        private void UpdateItemColor()
        {
            MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

            for (int i = 0; i < meshRenderers.Length; i++)
            {
                MeshRenderer meshRenderer = meshRenderers[i];
                if (meshRenderer.sharedMaterial.Equals(ColorTraitsSO.replaceableMaterial))
                {
                    meshRenderer.material = ColorTraitsSO.matchingMaterial;
                }
            }
        }

        #endregion


        #region EQUITABILITY
        public override bool EqualsValue(IProperty other)
        {
            ColorProperty colorProperty = other as ColorProperty;
            return ColorTraitsSO.Equals(colorProperty.ColorTraitsSO);
        }

        #endregion
    }
}
