using System.Collections.Generic;
using UnityEngine;
using Utilities;
using System;
using Agent.Communication.Gestures;

namespace Agent.Communication.Cognition {
    public class SimpleResponseManager
    {
        public List<Animation> KnownMetaGestures; // Rows
        public List<GestureRecognized> KnownMetaMeanings; // Columns
        private TwoWayDictionary<GestureRecognized, Animation> MetaGestureRegistry;
       
        private List<List<float>> GestureMeaningLikelyhoods;
        
        public List<Animation> KnownGestures;
        public List<GestureRecognized> KnownMeanings;
        private TwoWayDictionary<GestureRecognized, Animation> GestureRegistry;

        private Animation lastGesture;

        private static float changeFactor = 0.1f;
        Func<float, float> likelihood = x => Mathf.Log(Mathf.Exp(3f*x), -1f);


        public SimpleResponseManager() {
            KnownGestures ??= new List<Animation>();
            KnownMeanings ??= new List<GestureRecognized>();

            if (KnownGestures.Count != KnownMeanings.Count)
            {
                throw new Exception("KnownGestures and KnownMeanings must have the same length");
            }
            else
            {
                StartupMeaningLikelihoods();
            }
        }

        private void StartupMeaningLikelihoods()
        {
            GestureMeaningLikelyhoods = new List<List<float>>();

            for (int i = 0; i < KnownGestures.Count; i++)
            {
                GestureMeaningLikelyhoods.Add(new List<float>());
                for (int j = 0; j < KnownMeanings.Count; j++)
                {
                    GestureMeaningLikelyhoods[i].Add(0.3f);
                }

                GestureMeaningLikelyhoods[i][i] = 0.7f;
            }
        }



        public TwoWayDictionary<GestureRecognized, Animation> GetGestureRegistry()
        {
            return GestureRegistry;
        }

        public TwoWayDictionary<GestureRecognized, Animation> GetMetaGestureRegistry()
        {
            return MetaGestureRegistry;
        }

        public void UpdateLastGesture(Animation gesture)
        {
            lastGesture = gesture;
        }

        public void ManageRecognizedGesture(Animation gesture, GestureRecognized meaning)
        {
            
        }

        public void ManageRecognizedGesture(Animation gesture, GestureRecognized meaning)
        {
            MetaGestureRegistry.TryGetValue(meaning, out Animation metaGesture);
            if (metaGesture != null)
            {
                switch(meaning)
                {
                    case GestureRecognized.YES:
                        UpdateGestureMeaning(metaGesture, meaning, gesture == metaGesture);
                        break;
                    case GestureRecognized.NO:
                        UpdateGestureMeaning(metaGesture, meaning, gesture == metaGesture);
                        break;
                    case GestureRecognized.UNRECOGNIZED:
                        UpdateGestureMeaning(metaGesture, meaning, gesture == metaGesture);
                        break;
                }
                UpdateGestureMeaning(metaGesture, meaning, gesture == metaGesture);
            }
            else
            {
                GestureRegistry.TryGetValue(meaning, out Animation knownGesture);
                if (knownGesture != null)
                {
                    UpdateGestureMeaning(knownGesture, meaning, gesture == knownGesture);
                }
                
            }

        }


        /// <summary> Updates a gesture's meaning </summary>
        public void UpdateGestureMeaning(Animation gesture, GestureRecognized meaning, bool match) {
            int multiplier = match ? 1 : -1;

            

            GestureRegistry.Update(meaning, gesture);
            return;
        }

        /// <summary> Returns the gesture with the requested meaning (if known). </summary>
        public Animation GetGestureFromMeaning(GestureRecognized meaning) {
            bool containsKey = GestureRegistry.TryGetValue(meaning, out Animation gesture);
            if (containsKey) return gesture;
            else return null;
        }


        /// <summary> Returns the most likely meaning for a given gesture, out of the ones known </summary>
        public GestureRecognized GetMeaningFromGesture(Animation gesture) {
            GestureRegistry.TryGetValue(gesture, out GestureRecognized value);
            return value;
        }

        public void ManageLikelyGestures(GestureRecognized meaning, Animation gesture, bool checksOut)
        {

        }
    }
}

