using System.Collections.Generic;
using Unity.XR.CoreUtils.Datums;
using UnityEngine;

namespace Gameplay.Items.Properties
{
    public class LocationProperty : Property
    {
        public enum Direction
        {
            NONE,
            Front, Back,
            Left, Right,
            UpperLeft = Front & Left,
            UpperRight = Front & Right,
            LowerLeft = Back & Left,
            LowerRight = Back & Right
        }

        // Reference object's location (e.g.: the player's body)
        [SerializeField]
        private Transform referenceLocation;

        // This item's current location
        [SerializeField]
        private Transform currentLocation;

        // The item's direction relative to the reference object
        public Direction relativeDirection;


        public readonly List<Direction> directions = new List<Direction>() { 
            Direction.Front, Direction.Back,
            Direction.Left, Direction.Right,
            Direction.UpperLeft, Direction.UpperRight,
            Direction.LowerLeft, Direction.LowerRight
        };

        // How far away the item and reference object must be
        // for it to be considered a different position
        public static float locationUnitLength = 0.5f;

        public LocationProperty():base(PropertyType.Location)
        {
        }

        public void Start()
        {
            currentLocation = transform;
            UpdateRelativeDirection();
            Debug.Log("LOCATION Start!");
        }

        #region PROPERTY_SPECIFIC_METHODS

        public Direction GetRelativeDirection()
        {
            return relativeDirection;
        }

        public Vector3 GetDirectionVector()
        {
            return referenceLocation.position - currentLocation.position;
        }

        public Direction ComputeRelativeDirection()
        {
            Vector3 relativePosition = GetDirectionVector();
            relativePosition.y = 0;
            relativePosition.Normalize();
            Direction relativeDirection = Direction.NONE;

            if (Mathf.Abs(Vector3.Distance(relativePosition, Vector3.zero)) >= locationUnitLength)
            {
                Direction horizontalDirection = relativePosition.x > 0 ?
                                        Direction.Left : Direction.Right;
                Direction verticalDirection = relativePosition.y > 0 ?
                                        Direction.Front : Direction.Back;

                relativeDirection = horizontalDirection & verticalDirection;
            }

            return relativeDirection;
        }

        public Direction UpdateRelativeDirection()
        {
            relativeDirection = ComputeRelativeDirection();
            return relativeDirection;
        }

        #endregion

        #region EQUITABILITY
        public override bool EqualsValue(IProperty other)
        {
            LocationProperty locationProperty = other as LocationProperty;
            return EqualsDirection(locationProperty);
        }

        public bool EqualsDirection(LocationProperty other)
        {
            return relativeDirection.Equals(other.relativeDirection);
        }
        #endregion
    }
}