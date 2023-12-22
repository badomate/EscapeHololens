using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Items.Properties
{
    public class ColorSO : PropertySO
    {
        static Dictionary<ValueType, Color> validColors =
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

        [SerializeField]
        protected ValueType valueType;

        public ColorSO(Type type):base(type)
        {
            _type = Type.Color;
            Value = GetColorValue();
        }

        private Color GetColorValue()
        {
            if (validColors.ContainsKey(valueType))
            {
                return validColors[valueType];
            }
            else
            {
                return Color.white;
            }
        }

        public bool EqualsValueType(ColorSO other)
        {
            return valueType.Equals(other.GetValueType());
        }

        public override bool EqualsValue(PropertySO property)
        {
            return GetValue().Equals(property.GetValue());
        }

        public ValueType GetValueType()
        {
            return valueType;
        }
    }
}
