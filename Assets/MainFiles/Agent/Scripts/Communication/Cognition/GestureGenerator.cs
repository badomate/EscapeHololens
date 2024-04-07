using System.Collections.Generic;
using UnityEngine;
using Agent.Communication.Gestures;
using System;

namespace Agent.Communication.Cognition.Startup {

    [Obsolete("This class is deprecated while complex gestures are not present.")]
    public class GestureGenerator
    {
        [SerializeField]
        public static List<Animation> metaGestureAnimations;

        [SerializeField]
        public static List<string> metaGestures;

        [SerializeField]
        public static List<Animation> baseGestureAnimations;

        [SerializeField]
        public static List<string> baseGestures;

        public static void GenerateStarterGestures(ResponseManager responseManager) {
            GenerateMetaGestures(responseManager);
            GenerateBasicGestures(responseManager);
        }

        /// <summary> Generates known metagestures </summary>
        public static void GenerateMetaGestures(ResponseManager responseManager) 
        {
            GenerateGestures(responseManager, metaGestureAnimations, metaGestures, true);
        }

        /// <summary> Generates basic known gestures </summary>
        public static void GenerateBasicGestures(ResponseManager responseManager)
        {
            GenerateGestures(responseManager, baseGestureAnimations, baseGestures);
        }

        public static void GenerateGestures(ResponseManager responseManager, 
            List<Animation> animations, List<string> gestureTitles, bool isMeta= false)
        {
            for (int i = 0; i < animations.Count; i++)
            {
                Gesture gesture = new Gesture();
                gesture.animation = animations[i];
                responseManager.AddGesture(gesture, gestureTitles[i], isMeta);
            }
        }
    }
}
