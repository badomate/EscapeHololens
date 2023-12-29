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

        // reference object location (the player's, in this case)
        [SerializeField]
        private Transform referenceLocation;
        
        private readonly Transform currentLocation;

        // Relative direction of object
        private Direction relativeDirection;


        public readonly List<Direction> directions = new List<Direction>() { 
            Direction.Front, Direction.Back,
            Direction.Left, Direction.Right,
            Direction.UpperLeft, Direction.UpperRight,
            Direction.LowerLeft, Direction.LowerRight
        };

        public static float locationUnitLength = 0.5f;

        public LocationProperty(PropertyType type):base(type)
        {
            currentLocation = transform;
            Type = PropertyType.Location;
            UpdateRelativeDirection();
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