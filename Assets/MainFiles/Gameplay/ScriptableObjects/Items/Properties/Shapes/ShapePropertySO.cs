using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Gameplay.Items.Properties
{
    [CreateAssetMenu(fileName = "ShapePropertySO", menuName = "Gameplay/Item Properties/Shape")]
    public class ShapePropertySO : PropertySO
    {
        public enum Id
        {
            NONE,
            SQUARE,
            CIRCLE,
            TRIANGLE
        }

        public Id id;

        public Sprite representation2D;
        public GameObject representation3D;

        public ShapePropertySO() : base(Type.Shape) {
            UpdateName(id.ToString());
        }

        private void OnValidate()
        {
            UpdateName(id.ToString());
        }

        /*
        public override void UpdateItem(GameObject item)
        {
            Transform shapeHolder = ObjectManipulation.FindFirstChildWithTag(item, "Interactable");
            if (shapeHolder != null)
                Destroy(shapeHolder.gameObject);
            Instantiate(representation3D, shapeHolder);
        }*/

        public override bool EqualsSubType(PropertySO other) 
        { 
            ShapePropertySO otherShapeProperty = other as ShapePropertySO;

            if (otherShapeProperty == null) return false;

            return id == otherShapeProperty.id;
        }
    }
}