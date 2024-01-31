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

        void SetProperties() {
            Properties.Clear();
            PropertyList = gameObject.GetComponents<Property>();
            for (int i = 0; i < PropertyList.Length; i++)
            {
                
                Property property = PropertyList[i];

                // No duplicate properties allowed
                Properties.TryAdd(property.Type, property); 
            }
        }

        public void Start()
        {
            SetProperties();
        }

        private void OnValidate() {
            SetProperties();
        }

        public T GetProperty<T>() where T : Property
        {
            foreach (Property item in Properties.Values)
            {
                if (item is T t) return t;
            }
            return null;
        }

        public Property GetProperty(Property.PropertyType propertyType)
        {
            Properties.TryGetValue(propertyType, out Property property);
            return property;
        }

        [SerializeField]
        public int Id = -1;

        public int GetId()
        {
            return Id;
        }

        public bool EqualsId(int id) { return Id == id; }
    }
}
