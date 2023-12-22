using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay.Items.Properties
{
    public abstract class PropertySO : ScriptableObject
    {
        public enum Type
        {
            NONE,
            Color,
            Shape,
            Location
        }

        [SerializeField]
        protected Type _type { get; set; }
        protected dynamic Value;

        public PropertySO(Type type)
        {
            _type = type;
        }

        public List<Type> GetAllTypes()
        {
            return Enum.GetValues(typeof(Type))
                        .Cast<Type>()
                        .ToList();
        }

        public bool EqualsType(PropertySO property)
        {
            return _type.Equals(property.GetType());
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
