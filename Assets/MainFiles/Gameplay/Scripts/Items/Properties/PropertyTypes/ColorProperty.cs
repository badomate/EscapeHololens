using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Items.Properties
{
    public class ColorProperty : Property
    {
        static readonly Dictionary<Hue, Color> validColors =
            new Dictionary<Hue, Color>() {
                { Hue.BLUE, Color.blue},
                { Hue.RED,Color.red }
            };

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
