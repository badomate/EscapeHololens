using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.Items.Properties
{
    [Serializable]
    public abstract class Property: MonoBehaviour, IProperty
    {
        // To add a new property, add a new type
        // and create its corresponding class
        // (inherits from "Property")
        public enum PropertyType
        {
            NONE,
            Color,
            Shape,
            Location
        }

        [SerializeField]
        public PropertyType Type { get; protected set; }

        public Property(PropertyType type)
        {
            Type = type;
        }

        public List<PropertyType> GetAllTypes()
        {
            return Enum.GetValues(typeof(PropertyType))
                        .Cast<PropertyType>()
                        .ToList();
        }

        #region EQUITABILITY
        public bool Equals(IProperty other)
        {
            Property property = other as Property;
            return EqualsType(property) && EqualsValue(property);
        }

        public bool EqualsType(IProperty property)
        {
            return Type.Equals(property.GetType());
        }

        public abstract bool EqualsValue(IProperty other);

        #endregion
    }
}
