using System.Collections.Generic;
using UnityEngine;
using Gameplay.Items.Properties;

namespace Gameplay.Items
{
    public class Item : MonoBehaviour
    {
        [SerializeField]
        private Dictionary<Property.PropertyType, Property> Properties;

        public Property GetProperty(Property.PropertyType propertyType)
        {
            Properties.TryGetValue(propertyType, out Property property);
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
