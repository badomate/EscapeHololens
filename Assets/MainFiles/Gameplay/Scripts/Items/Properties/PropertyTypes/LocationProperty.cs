using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Items.Properties
{

    [Obsolete("Property class is deprecated, please use PropertySO and its subclasses instead.", error: true)]
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

        [SerializeField]
        private static string defaultReferenceTag = "MainCamera";

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
            UpdateReferenceObject(defaultReferenceTag);
            UpdateRelativeDirection();
        }

        #region PROPERTY_SPECIFIC_METHODS

        public void UpdateReferenceObject(string tag)
        {
            if (referenceLocation == null)
            {
                Camera[] validCameras = FindObjectsOfType<Camera>();
                for (int i = 0; i < validCameras.Length; i++)
                {
                    Camera camera = validCameras[i];
                    if (camera.CompareTag(tag))
                    {
                        referenceLocation = camera.transform;
                        break;
                    }
                }
            }
        }

        public Direction GetRelativeDirection(Transform reference = null)
        {
            if (reference == null) return relativeDirection;
            else return ComputeRelativeDirection(reference);
        }

        public Vector3 GetDirectionVector(Transform reference = null)
        {
            if (reference == null)
                reference = referenceLocation;
            
            return reference.position - currentLocation.position;
        }

        public Direction ComputeRelativeDirection(Transform reference)
        {
            Vector3 relativePosition = GetDirectionVector(reference);
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
            relativeDirection = ComputeRelativeDirection(this.referenceLocation);
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