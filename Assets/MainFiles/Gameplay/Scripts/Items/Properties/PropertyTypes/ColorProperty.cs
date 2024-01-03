using UnityEngine;

namespace Gameplay.Items.Properties
{
    // May add more color traits (saturation, luminosity, ...)
    public class ColorProperty : Property
    {
        [SerializeField]
        private ColorTraitsSO colorTraitsSO;

        public ColorProperty() : base(PropertyType.Color)
        {
        }

        public void Start()
        {
            UpdateItemColor();
            Debug.Log("COLOR Start!");
        }


        #region PROPERTY_SPECIFIC_METHODS

        public ColorTraitsSO GetTraits() { return colorTraitsSO; }

        private void UpdateItemColor()
        {
            MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();

            for (int i = 0; i < meshRenderers.Length; i++)
            {
                MeshRenderer meshRenderer = meshRenderers[i];
                if (meshRenderer.sharedMaterial.Equals(colorTraitsSO.replaceableMaterial))
                {
                    meshRenderer.material = colorTraitsSO.matchingMaterial;
                }
            }
        }

        #endregion


        #region EQUITABILITY
        public override bool EqualsValue(IProperty other)
        {
            ColorProperty colorProperty = other as ColorProperty;
            return colorTraitsSO.Equals(colorProperty.GetTraits());
        }

        #endregion
    }
}
