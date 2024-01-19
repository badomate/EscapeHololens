using UnityEngine;
using Utils;

namespace Gameplay.Items.Properties
{
    public class ShapeProperty : Property
    {
        [field: SerializeField]
        public ShapeTraitsSO ShapeTraitsSO {get; private set;}

        public ShapeProperty():base(PropertyType.Shape)
        {
        }

        public void Start()
        {
            // UpdateItemShape();
        }

        #region PROPERTY_SPECIFIC_METHODS

        public ShapeTraitsSO GetTraits() { return ShapeTraitsSO; }

        // Requires a shape holder with tag "Interactable"
        // Which is a gameobject that holds the shape components
        private void UpdateItemShape()
        {
            Transform shapeHolder = ObjectManipulation.FindFirstChildWithTag(gameObject, "Interactable");
            if (shapeHolder != null)
                Destroy(shapeHolder.gameObject);
            Instantiate(ShapeTraitsSO.representation3D, shapeHolder);
        }

        #endregion

        #region EQUITABILITY
        public override bool EqualsValue(IProperty other)
        {
            ShapeProperty shapeProperty = other as ShapeProperty;
            return ShapeTraitsSO.Equals(shapeProperty.GetTraits());
        }

        #endregion
    }
}
