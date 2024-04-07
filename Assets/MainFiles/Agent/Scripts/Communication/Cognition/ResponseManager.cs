using System.Collections.Generic;
using UnityEngine;
using Agent.Communication.Gestures;
using Agent.Communication.Cognition.Startup;
using AuxiliarContent;
using Utilities;
using System;

namespace Agent.Communication.Cognition {

    [Obsolete("This class is deprecated, use GestureRecognizer instead - for now")]
    public class ResponseManager
    {
        public class ResponseGesture
        {
            public Gesture gesture;
            public float confidence;
        }

        private TwoWayDictionary<string, Gesture> MetaGestureRegistry;
        private TwoWayDictionary<string, Gesture> GestureRegistry;

        public ResponseManager() {
            MetaGestureRegistry = new TwoWayDictionary<string, Gesture>();
            GestureRegistry = new TwoWayDictionary<string, Gesture>();

            GestureGenerator.GenerateStarterGestures(this);

            CustomDebug.LogGen("GTM:" + GestureRegistry.Keys.Count);
            CustomDebug.LogGen("MGTM:" + MetaGestureRegistry.Keys.Count);
        }

        public TwoWayDictionary<string, Gesture> GetGestureRegistry()
        {
            return GestureRegistry;
        }

        public TwoWayDictionary<string, Gesture> GetMetaGestureRegistry()
        {
            return MetaGestureRegistry;
        }

        /// <summary> Adds a gesture to registries of known gestures </summary>
        public void AddGesture(Gesture gesture, string meaning, bool isMeta = false) {
            if (isMeta) {
                MetaGestureRegistry.Add(meaning, gesture);
            }
            else
            {
                GestureRegistry.Add(meaning, gesture);
            }
        }

        /// <summary> Updates a gesture's meaning </summary>
        public void UpdateGestureMeaning(Gesture gesture, string meaning) {
            if (!GestureRegistry.ContainsKey(gesture)) {
                AddGesture(gesture, meaning);
            }
            else {
                GestureRegistry.Update(meaning, gesture);
            }
        }

        /// <summary> Returns the gesture with the requested meaning (if known). </summary>
        public Gesture GetGestureFromMeaning(string meaning) {
            bool containsKey = GestureRegistry.TryGetValue(meaning, out Gesture gesture);
            if (containsKey) return gesture;
            else return null;
        }

        /// <summary> Returns the most likely meaning for a given gesture, out of the ones known </summary>
        public string GetMeaningFromGesture(Gesture gesture) {
            float bestMatchVariance= Mathf.Infinity;
            string bestMatch = "";

            // Always check metagestures first
            foreach (KeyValuePair<string, Gesture> dictGesture in MetaGestureRegistry)
            {
                Gesture metaGesture = dictGesture.Value;
                float matchVariance = gesture.GetMatchVariance(metaGesture);

                if (
                    (matchVariance <= (metaGesture.matchThresholdPerPose * metaGesture.poseSequence.Count)) 
                    && bestMatchVariance > matchVariance)
                {
                    bestMatchVariance = matchVariance;
                    bestMatch = dictGesture.Key;
                }
            }

            if (!bestMatch.Equals(""))
                return bestMatch;

            foreach (KeyValuePair<string, Gesture> dictGesture in GestureRegistry) {
                float matchVariance = gesture.GetMatchVariance(dictGesture.Value);
                if (bestMatchVariance > matchVariance) {
                    bestMatchVariance = matchVariance;
                    bestMatch = dictGesture.Key;
                }
            }

            return bestMatch;
        }
    }
}

