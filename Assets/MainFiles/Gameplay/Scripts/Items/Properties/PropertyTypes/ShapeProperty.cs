using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Items.Properties
{
    public class ShapeProperty : Property
    {
        public static readonly Dictionary<Format, GameObject> validShapes;

        public enum Format
        {
            NONE,
            SQUARE,
            CIRCLE,
            TRIANGLE
        }

        public Format format;
        private GameObject matchingShape;

        public ShapeProperty(Format formatValue):base(PropertyType.Shape)
        {
            format = formatValue;
            matchingShape = FindMatchingShape();
            Debug.Log("Shape created!");
        }

        #region PROPERTY_SPECIFIC_METHODS
        public Format GetFormat()
        {
            return format;
        }

        protected GameObject GetMatchingShape() { return matchingShape; }

        private GameObject FindMatchingShape()
        {
            if (validShapes.ContainsKey(format))
            {
                return validShapes.GetValueOrDefault(format);
            }
            else
            {
                return GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            }
        }

        #endregion

        #region EQUITABILITY
        public override bool EqualsValue(IProperty other)
        {
            ShapeProperty shapeProperty = other as ShapeProperty;
            return EqualsFormat(shapeProperty);
        }

        public bool EqualsFormat(ShapeProperty other)
        {
            return format.Equals(other.GetFormat());
        }
        #endregion
    }
}
