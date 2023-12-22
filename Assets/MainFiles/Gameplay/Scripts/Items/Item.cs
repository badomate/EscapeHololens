using System.Collections.Generic;
using UnityEngine;
using Gameplay.Items.Properties;

namespace Gameplay.Items
{
    public class Item : MonoBehaviour
    {
        [SerializeField]
        private Dictionary<PropertySO.Type, PropertySO> _properties;

        public PropertySO GetProperty(PropertySO.Type propertyType)
        {
            return _properties[propertyType];
        }

        [SerializeField]
        private int _id { get; set; }

        public int GetId()
        {
            return _id;
        }
    }
}
