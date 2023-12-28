using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Items.Properties
{
    public class ShapeSO : PropertySO
    {
        public static readonly Dictionary<ValueType, GameObject> validShapes;

        public enum ValueType
        {
            NONE,
            SQUARE,
            CIRCLE
        }

        public ValueType ShapeType { get; protected set; }

        public ShapeSO(PropertyType type, ValueType shapeType):base(type)
        {
            Type = PropertyType.Shape;
            ShapeType = shapeType;
            Value = GetShapeValue();
        }

        private GameObject GetShapeValue()
        {
            if (validShapes.ContainsKey(ShapeType))
            {
                return validShapes.GetValueOrDefault(ShapeType);
            }
            else
            {
                return GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            }
        }

        public bool EqualsValueType(ShapeSO other)
        {
            return ShapeType.Equals(other.GetValueType());
        }
        public ValueType GetValueType()
        {
            return ShapeType;
        }
    }
}
