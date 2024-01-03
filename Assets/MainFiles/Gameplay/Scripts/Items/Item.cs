using System.Collections.Generic;
using UnityEngine;
using Gameplay.Items.Properties;

namespace Gameplay.Items
{
    public class Item : MonoBehaviour
    {
        // This is so the Unity Editor can recognize Properties
        [SerializeField]
        private Property[] PropertyList;

        private Dictionary<Property.PropertyType, Property> Properties;

        public Item()
        {
            Properties = new Dictionary<Property.PropertyType, Property>();
        }

        public void Start()
        {
            PropertyList = gameObject.GetComponents<Property>();
            for (int i = 0; i < PropertyList.Length; i++)
            {
                
                Property property = PropertyList[i];

                // No duplicate properties allowed
                Properties.TryAdd(property.Type, property); 
            }
        }

        public Property GetProperty(Property.PropertyType propertyType)
        {
            Properties.TryGetValue(propertyType, out Property property);
            return property;
        }

        [SerializeField]
        private int Id = -1;

        public int GetId()
        {
            return Id;
        }

        public bool EqualsId(int id) { return Id == id; }
    }
}
