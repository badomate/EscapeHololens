using System.Collections.Generic;
using UnityEngine;
using Gameplay.Items.Properties;
using System;

namespace Gameplay.Items
{
    public class Item : MonoBehaviour
    {
        // This is so the Unity Editor can recognize Properties
        [SerializeField]
        private List<PropertySO> PropertyList;

        private Dictionary<PropertySO.Type, PropertySO> Properties;

        public Item()
        {
            Properties = new Dictionary<PropertySO.Type, PropertySO>();
        }

        public void SetProperties() {
            Properties.Clear();

            for (int i = 0; i < PropertyList.Count; i++)
            {
                PropertySO property = PropertyList[i];

                // No duplicate properties allowed
                if (Properties.TryAdd(property.type, property))
                    property.UpdateItem(gameObject);
            }

        }

        public void Start()
        {
            SetProperties();
        }

        public T GetProperty<T>() where T : PropertySO
        {
            foreach (PropertySO item in Properties.Values)
            {
                if (item is T t) return t;
            }
            return null;
        }

        public PropertySO GetProperty(PropertySO.Type propertyType)
        {
            Properties.TryGetValue(propertyType, out PropertySO property);
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
