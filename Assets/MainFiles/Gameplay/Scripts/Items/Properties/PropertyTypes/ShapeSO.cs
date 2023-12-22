using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Items.Properties
{
    public class ShapeSO : PropertySO
    {
        [SerializeField]
        public static Dictionary<ValueType, GameObject> validShapes;

        public enum ValueType
        {
            NONE,
            SQUARE,
            CIRCLE
        }

        [SerializeField]
        protected ValueType valueType;

        public ShapeSO(Type type):base(type)
        {
            _type = Type.Shape;
            Value = GetShapeValue();
        }

        private GameObject GetShapeValue()
        {
            if (validShapes.ContainsKey(valueType))
            {
                return validShapes[valueType];
            }
            else
            {
                return GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            }
        }

        public bool EqualsValueType(ShapeSO other)
        {
            return valueType.Equals(other.GetValueType());
        }
        public ValueType GetValueType()
        {
            return valueType;
        }
    }
}
