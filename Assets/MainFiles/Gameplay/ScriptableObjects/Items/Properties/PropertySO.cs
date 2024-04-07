using System;
using UnityEngine;

namespace Gameplay.Items.Properties
{
    public class PropertySO: ScriptableObject, IEquatable<PropertySO>, IItemUpdatable
    {
        public enum Type
        {
            NONE,
            Color,
            Shape,
            Location
        }

        public Type type;
        public string fullName;

        public PropertySO(Type type)
        {
            this.type = type;
            UpdateName();
        }

        private void OnValidate()
        {
            UpdateName();
        }

        public override string ToString()
        {
            return base.ToString() + ":\n" + fullName;
        }

        public virtual bool Equals(PropertySO other)
        {
            return EqualsType(other) && EqualsSubType(other);
        }

        public virtual bool EqualsType(PropertySO other)
        {
            return type.Equals(other.type);
        }

        public virtual bool EqualsSubType(PropertySO other)
        {
            return true;
        }

        public virtual void UpdateItem(GameObject item = null)
        {
            return;
        }

        public virtual void UpdateName(string suffix = "")
        {
            if (suffix != "")
                fullName = type.ToString() + "." + suffix;
            else
                fullName = type.ToString();
        }
    }
}