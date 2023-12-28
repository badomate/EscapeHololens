using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.Items.Properties
{
    public abstract class PropertySO : ScriptableObject
    {
        public enum PropertyType
        {
            NONE,
            Color,
            Shape,
            Location
        }

        [SerializeField]
        public PropertyType Type { get; protected set; }
        public dynamic Value {get; protected set; }

        public PropertySO(PropertyType type)
        {
            Type = type;
        }

        public List<PropertyType> GetAllTypes()
        {
            return Enum.GetValues(typeof(PropertyType))
                        .Cast<PropertyType>()
                        .ToList();
        }

        public bool EqualsType(PropertySO property)
        {
            return Type.Equals(property.GetType());
        }

        #region SUBCLASS_DEFINED_VALUE_TYPE

        public bool Equals(PropertySO property)
        {
            return EqualsType(property) && EqualsValue(property);
        }

        public virtual bool EqualsValue(PropertySO property)
        {
            return GetValue().Equals(property.GetValue());
        }

        public virtual object GetValue()
        {
            return Value;
        }
        
        #endregion
    }
}
