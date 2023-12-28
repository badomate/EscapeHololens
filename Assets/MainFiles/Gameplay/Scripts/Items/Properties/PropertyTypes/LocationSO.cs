using System.Collections.Generic;
using Unity.XR.CoreUtils.Datums;
using UnityEngine;

namespace Gameplay.Items.Properties
{
    // Likely to be unused, but still
    // Maybe this property can calculate relative location?
    public class LocationSO : PropertySO
    {
        // reference object location (the player's, in this case)
        [SerializeField]
        private Transform referenceItem;

        public Direction RelativeDirection;

        [SerializeField]
        private Transform originalItem;

        public readonly List<Direction> directions = new List<Direction>() { 
            Direction.FRONT, Direction.BACK,
            Direction.LEFT, Direction.RIGHT,
            Direction.UPPER_LEFT, Direction.UPPER_RIGHT,
            Direction.LOWER_LEFT, Direction.LOWER_RIGHT
        };

        public static float locationUnitLength = 0.5f;

        public LocationSO(PropertyType type):base(type)
        {
            Type = PropertyType.Location;
            Value = GetLocationValue();
        }

        public enum Direction
        {
            NONE,
            FRONT, BACK,
            LEFT, RIGHT,
            UPPER_LEFT = FRONT & LEFT,
            UPPER_RIGHT = FRONT & RIGHT,
            LOWER_LEFT = BACK & LEFT,
            LOWER_RIGHT = BACK & RIGHT
        }

        private Direction GetLocationValue()
        {
            return UpdateRelativeDirection();
        }

        public Vector3 GetDirectionVector()
        {
            return referenceItem.position - originalItem.position;
        }

        public Direction GetRelativeDirection()
        {
            Vector3 relativePosition = GetDirectionVector();
            relativePosition.y = 0;
            relativePosition.Normalize();
            Direction relativeDirection = Direction.NONE;

            if (Mathf.Abs(Vector3.Distance(relativePosition, Vector3.zero)) >= locationUnitLength)
            {
                Direction horizontalDirection = relativePosition.x > 0 ?
                                        Direction.LEFT : Direction.RIGHT;
                Direction verticalDirection = relativePosition.y > 0 ?
                                        Direction.FRONT : Direction.BACK;

                relativeDirection = horizontalDirection & verticalDirection;
            }

            return relativeDirection;
        }

        public Direction UpdateRelativeDirection()
        {
            RelativeDirection = GetRelativeDirection();
            return RelativeDirection;
        }
    }
}
