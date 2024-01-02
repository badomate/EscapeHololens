using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Items.Properties
{
    // May add more color traits (saturation, luminosity, ...)
    public class ColorProperty : Property
    {
        static readonly Dictionary<Hue, Color> validColors =
            new Dictionary<Hue, Color>() {
                { Hue.BLUE, Color.blue},
                { Hue.RED,Color.red }
            };

        // As per the HSL model
        public enum Hue
        {
            NONE,
            RED,
            BLUE
        }

        private Hue hue;
        private Color matchingColor;

        public ColorProperty(PropertyType type, Hue colorValue) :base(type)
        {
            Type = PropertyType.Color;
            hue = colorValue;
            matchingColor = FindMatchingColor();
        }

        // Automatically updates object color based on specified hue
        public void Start()
        {
            gameObject.GetComponent<Material>().color = matchingColor;
        }

        #region PROPERTY_SPECIFIC_METHODS

        protected Color GetMatchingColor() { return matchingColor; }

        private Color FindMatchingColor()
        {
            if (validColors.ContainsKey(hue))
            {
                return validColors[hue];
            }
            else
            {
                return default(Color);
            }
        }
        #endregion

        #region EQUITABILITY
        public override bool EqualsValue(IProperty other)
        {
            ColorProperty colorProperty = other as ColorProperty;
            return EqualsHue(colorProperty);
        }

        public bool EqualsHue(ColorProperty other)
        {
            return hue.Equals(other.hue);
        }

        #endregion
    }
}
