using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
            gameObject.GetComponent<MeshRenderer>().material = colorTraitsSO.matchingMaterial;
            Debug.Log("COLOR Start!");
        }


        #region PROPERTY_SPECIFIC_METHODS

        public ColorTraitsSO GetTraits() { return colorTraitsSO; }

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
