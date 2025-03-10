using System.Collections;

namespace Agent.Communication.Gestures.ContentGenerators.StarterGestures
{

    /// <summary> Gesture known since the start of the game. </summary>
    public class StarterGesture : Gesture
    {
        public string gestureId;
        public StarterGesture(GestureID id) : base()
        {
            gestureId = id.ToString();
        }
    }
}