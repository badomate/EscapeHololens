using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Items.Properties
{
    public class ColorSO : PropertySO
    {
        static readonly Dictionary<ValueType, Color> validColors =
            new Dictionary<ValueType, Color>() {
                { ValueType.BLUE, Color.blue},
                { ValueType.RED,Color.red }
            };

        public enum ValueType
        {
            NONE,
            RED,
            BLUE
        }

        public ValueType ColorType { get; protected set; }

        public ColorSO(PropertyType type, ValueType colorType) :base(type)
        {
            Type = PropertyType.Color;
            ColorType = colorType;
            Value = GetColorValue();
        }

        private Color GetColorValue()
        {
            if (validColors.ContainsKey(ColorType))
            {
                return validColors[ColorType];
            }
            else
            {
                return Color.white;
            }
        }

        public bool EqualsValueType(ColorSO other)
        {
            return ColorType.Equals(other.GetValueType());
        }

        public override bool EqualsValue(PropertySO property)
        {
            return GetValue().Equals(property.GetValue());
        }

        public ValueType GetValueType()
        {
            return ColorType;
        }
    }
}
