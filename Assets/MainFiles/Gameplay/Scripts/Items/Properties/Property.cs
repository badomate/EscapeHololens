using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.Items.Properties
{
    [Serializable]
    public class Property: MonoBehaviour, IProperty
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
        public string FullName;

        public Property(PropertyType type)
        {
            Type = type;
            FullName = type.ToString();
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

        public virtual bool EqualsValue(IProperty other) { return false; }

        #endregion
    }
}
