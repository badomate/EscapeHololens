using UnityEngine;

namespace Gameplay.Items.Properties
{
    // Likely to be unused, but still
    // Maybe this property can calculate relative location?
    public class LocationSO : PropertySO
    {
        // reference object location (the player's, in this case)
        [SerializeField]
        private Transform referencePos;
        public LocationSO(Type type):base(type)
        {
            _type = Type.Location;
            Value = GetLocationValue();
        }

        private Vector3 GetLocationValue()
        {
            // Maybe return game object attached loc instead, and calculate relative position
            return referencePos.position;
        }
    }
}
