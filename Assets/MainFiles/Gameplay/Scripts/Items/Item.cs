using System.Collections.Generic;
using UnityEngine;
using Gameplay.Items.Properties;

namespace Gameplay.Items
{
    public class Item : MonoBehaviour
    {
        [SerializeField]
        private Dictionary<PropertySO.PropertyType, PropertySO> Properties;

        public PropertySO GetProperty(PropertySO.PropertyType propertyType)
        {
            Properties.TryGetValue(propertyType, out PropertySO property);
            return property;
        }

        [SerializeField]
        private int Id { get; set; }

        public int GetId()
        {
            return Id;
        }
    }
}
