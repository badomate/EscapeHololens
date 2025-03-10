using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using System.Diagnostics;
using Pose = Agent.Communication.Gestures.Pose;

namespace SensorHub
{
    [CreateAssetMenu(fileName = "CameraStreamPS", menuName = "Pose Solver/Camera Stream", order = 0)]
    public class CameraStreamPoseSolver : PoseSolver {
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        [Serializable]
        private class BodyContainer
        {
            public List<BodyData> body;
            public HandContainer hands;
        }

        [Serializable]
        private class HandContainer
        {
            public List<BodyData> left;
            public List<BodyData> right;
            public List<BodyData> left_world;
            public List<BodyData> right_world;
        }

        [Serializable]
        private class BodyData
        {
            public int id;
            public List<float> data;
            public string landmarkName;
        }

        public static Pose playerPose = new Pose();
        public Vector3 centerLandmarkOffset = new Vector3(); //coordinates of that center
        public long animationFPS = 0;

        // Process and handle the received landmarks data
        void ProcessLandmarksData(string jsonData)
        {
            jsonData = jsonData.Substring(jsonData.IndexOf('{'));

            //UnityEngine.Debug.Log(jsonData);
            // Deserialize the JSON string into an array of Vector3Data objects
            BodyContainer dataContainer = JsonUtility.FromJson<BodyContainer>(jsonData);

            Pose.Landmark identifiedLandmark = Pose.Landmark.LEFT_WRIST;
            foreach (BodyData body in dataContainer.body) //the amount of landmarks seems to always be 33 no matter how obscured the person is
            {
                bool included = true; //whether we are going to use it, whether it appears in the switch case somewhere
                switch (body.landmarkName)

                {
                    case "Left hip":
                        identifiedLandmark = Pose.Landmark.LEFT_HIP;
                        break;
                    case "Right hip":
                        identifiedLandmark = Pose.Landmark.RIGHT_HIP;
                        break;
                    case "Left wrist":
                        identifiedLandmark = Pose.Landmark.LEFT_WRIST;
                        break;
                    case "Right wrist":
                        identifiedLandmark = Pose.Landmark.RIGHT_WRIST;
                        break;
                    case "Left ankle":
                        identifiedLandmark = Pose.Landmark.LEFT_FOOT;
                        break;
                    case "Right ankle":
                        identifiedLandmark = Pose.Landmark.RIGHT_FOOT;
                        break;
                    case "Right elbow":
                        identifiedLandmark = Pose.Landmark.RIGHT_ELBOW;
                        break;
                    case "Left elbow":
                        identifiedLandmark = Pose.Landmark.LEFT_ELBOW;
                        break;
                    case "Left shoulder":
                        identifiedLandmark = Pose.Landmark.LEFT_SHOULDER;
                        break;
                    case "Right shoulder":
                        identifiedLandmark = Pose.Landmark.RIGHT_SHOULDER;
                        break;
                    case "Right ear":
                        identifiedLandmark = Pose.Landmark.RIGHT_EAR;
                        break;
                    case "Left ear":
                        identifiedLandmark = Pose.Landmark.LEFT_EAR;
                        break;
                    case "Nose":
                        identifiedLandmark = Pose.Landmark.NOSE;
                        break;
                    case "Right knee":
                        identifiedLandmark = Pose.Landmark.RIGHT_KNEE;
                        break;
                    case "Left knee":
                        identifiedLandmark = Pose.Landmark.LEFT_KNEE;
                        break;
                    default:
                        included = false; //if it didn't match anything we need, don't modify the Pose
                        break;
                }
                if (included)
                {
                    processLandmark(identifiedLandmark, new Vector3(body.data[0], body.data[1], body.data[2]));
                }
            }

            //TODO: there's probably a way to collapse this to a function instead of repeating lines
            foreach (BodyData body in dataContainer.hands?.right_world)
            {
                bool included = true;
                switch (body.landmarkName)
                {
                    case "Index-4(fingertip)":
                        identifiedLandmark = Pose.Landmark.LEFT_INDEX;
                        break;
                    case "Thumb-4(fingertip)":
                        identifiedLandmark = Pose.Landmark.LEFT_THUMB;
                        break;
                    case "Middle-4(fingertip)":
                        identifiedLandmark = Pose.Landmark.LEFT_MIDDLE;
                        break;
                    case "Ring-4(fingertip)":
                        identifiedLandmark = Pose.Landmark.LEFT_RING;
                        break;
                    case "Pinky-4(fingertip)":
                        identifiedLandmark = Pose.Landmark.LEFT_PINKY;
                        break;

                    case "Index-2(knuckle)":
                        identifiedLandmark = Pose.Landmark.LEFT_INDEX_KNUCKLE;
                        break;
                    case "Thumb-2(knuckle)":
                        identifiedLandmark = Pose.Landmark.LEFT_THUMB_KNUCKLE;
                        break;
                    case "Middle-2(knuckle)":
                        identifiedLandmark = Pose.Landmark.LEFT_MIDDLE_KNUCKLE;
                        break;
                    case "Ring-2(knuckle)":
                        identifiedLandmark = Pose.Landmark.LEFT_RING_KNUCKLE;
                        break;
                    case "Pinky-2(knuckle)":
                        identifiedLandmark = Pose.Landmark.LEFT_PINKY_KNUCKLE;
                        break;


                    case "Wrist":
                        identifiedLandmark = Pose.Landmark.LEFT_WRIST_ROOT;
                        break;

                    case "Index-1(base)":
                        identifiedLandmark = Pose.Landmark.LEFT_INDEX_BASE;
                        break;
                    case "Thumb-1(base)":
                        identifiedLandmark = Pose.Landmark.LEFT_THUMB_BASE;
                        break;
                    case "Middle-1(base)":
                        identifiedLandmark = Pose.Landmark.LEFT_MIDDLE_BASE;
                        break;
                    case "Ring-1(base)":
                        identifiedLandmark = Pose.Landmark.LEFT_RING_BASE;
                        break;
                    case "Pinky-1(base)":
                        identifiedLandmark = Pose.Landmark.LEFT_PINKY_BASE;
                        break;

                    default:
                        included = false;
                        break;
                }
                if (included)
                {
                    processLandmark(identifiedLandmark, new Vector3(body.data[0], body.data[1], body.data[2]));
                }
            }


            foreach (BodyData body in dataContainer.hands?.left_world)
            {
                bool included = true;
                switch (body.landmarkName)
                {
                    case "Index-4(fingertip)":
                        identifiedLandmark = Pose.Landmark.RIGHT_INDEX;
                        break;
                    case "Thumb-4(fingertip)":
                        identifiedLandmark = Pose.Landmark.RIGHT_THUMB;
                        break;
                    case "Middle-4(fingertip)":
                        identifiedLandmark = Pose.Landmark.RIGHT_MIDDLE;
                        break;
                    case "Ring-4(fingertip)":
                        identifiedLandmark = Pose.Landmark.RIGHT_RING;
                        break;
                    case "Pinky-4(fingertip)":
                        identifiedLandmark = Pose.Landmark.RIGHT_PINKY;
                        break;


                    case "Index-2(knuckle)":
                        identifiedLandmark = Pose.Landmark.RIGHT_INDEX_KNUCKLE;
                        break;
                    case "Thumb-2(knuckle)":
                        identifiedLandmark = Pose.Landmark.RIGHT_THUMB_KNUCKLE;
                        break;
                    case "Middle-2(knuckle)":
                        identifiedLandmark = Pose.Landmark.RIGHT_MIDDLE_KNUCKLE;
                        break;
                    case "Ring-2(knuckle)":
                        identifiedLandmark = Pose.Landmark.RIGHT_RING_KNUCKLE;
                        break;
                    case "Pinky-2(knuckle)":
                        identifiedLandmark = Pose.Landmark.RIGHT_PINKY_KNUCKLE;
                        break;


                    case "Wrist":
                        identifiedLandmark = Pose.Landmark.RIGHT_WRIST_ROOT;
                        break;

                    case "Index-1(base)":
                        identifiedLandmark = Pose.Landmark.RIGHT_INDEX_BASE;
                        break;
                    case "Thumb-1(base)":
                        identifiedLandmark = Pose.Landmark.RIGHT_THUMB_BASE;
                        break;
                    case "Middle-1(base)":
                        identifiedLandmark = Pose.Landmark.RIGHT_MIDDLE_BASE;
                        break;
                    case "Ring-1(base)":
                        identifiedLandmark = Pose.Landmark.RIGHT_RING_BASE;
                        break;
                    case "Pinky-1(base)":
                        identifiedLandmark = Pose.Landmark.RIGHT_PINKY_BASE;
                        break;

                    default:
                        included = false;
                        break;
                }
                if (included)
                {
                    processLandmark(identifiedLandmark, new Vector3(body.data[0], body.data[1], body.data[2]));
                }

            }
        }

        void processLandmark(Pose.Landmark identifiedLandmark, Vector3 data)
        {

            Vector3 adjustedVector3 = Vector3.Scale(data, new Vector3(-1, -1, -1));

            if (!playerPose._landmarkArrangement.ContainsKey(identifiedLandmark))
            {
                playerPose._landmarkArrangement.Add(identifiedLandmark, adjustedVector3);
            }
            else
            {
                playerPose._landmarkArrangement[identifiedLandmark] = adjustedVector3;
            }
        }

        // Asynchronously stream pose landmarks data from the Flask API
        private async Task StreamLandmarksAsync(CancellationToken cancellationToken, 
                                               bool staticImageMode = false,
                                               int modelComplexity = 1,
                                               double minDetectionConfidence = 0.5,
                                               double minTrackingConfidence = 0.5,
                                               string outputVideo = "output.mp4",
                                               int displayFrames = 0,
                                               int useHandPose = 1,
                                               int drawBodyPose = 1,
                                               int drawHandPose = 1,
                                               int UseIndependentHands = 1
                                               )
        {
            var baseUrl = "http://localhost:5000";
            var apiUrl = "/landmarks";  

            using (var client = new HttpClient())
            {
                // Build the query parameters for the API request
                var queryParams = $"?" +
                                  $"&model_complexity={modelComplexity}" +
                                  $"&min_detection_confidence={minDetectionConfidence}" +
                                  $"&min_tracking_confidence={minTrackingConfidence}" +
                                  $"&output_video={outputVideo}" +
                                  $"&display_frames={displayFrames}" +
                                  $"&use_hand_pose={useHandPose}" +
                                  $"&draw_body_pose={drawBodyPose}" +
                                  $"&draw_hand_pose={drawHandPose}" +
                                  $"&use_independent_hands_estimation={UseIndependentHands}";

                // Send the GET request to the API
                var responseStream = await client.GetStreamAsync(baseUrl + apiUrl + queryParams);
            
                // Read the response stream and process each line of data
                using (var reader = new System.IO.StreamReader(responseStream))
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    while (!reader.EndOfStream)
                    {
                        if (cancellationToken.IsCancellationRequested) //If requested, exit task
                        {
                            // Clean up resources?
                            break;
                        }
                        var line = await reader.ReadLineAsync();

                        stopwatch.Stop();

                        long elapsed = stopwatch.ElapsedMilliseconds;
                        //long hz = 1000 / elapsed;
                        if (elapsed > 0)
                        {
                            animationFPS = 1000 / elapsed;
                            //UnityEngine.Debug.Log("Animation FPS: " + 1000 / elapsed);
                        }

                        if (!string.IsNullOrEmpty(line))
                        {
                            // Process each line of landmarks data
                            ProcessLandmarksData(line);
                        }

                        stopwatch.Reset();
                        stopwatch.Start();
                        //await Task.Yield();
                    }
                }
            }
        }
        private Task myGet;

        public override void Init()
        {
            //Debug.Log("Connecting...");
            //string myJson = "data : {\"bodies\": [{\"id\": 0, \"landmarkName\": \"Nose\", \"data\": [0.017760001122951508, -0.5622981190681458, -0.2617194652557373, 0.501261830329895, 0.6356117129325867, -0.5766324400901794]}, {\"id\": 1, \"landmarkName\": \"Left eye inner\", \"data\": [0.02411283180117607, -0.6019347906112671, -0.24616317451000214, 0.5239912867546082, 0.5869146585464478, -0.5480535626411438]}]}";
            //BodyContainer dataContainer = JsonUtility.FromJson<BodyContainer>(myJson);
            //Debug.Log(dataContainer.bodies[0].data.Count);

            // Call the asynchronous method to stream pose landmarks data
            myGet = Task.Run(() => StreamLandmarksAsync(cancellationTokenSource.Token,
                                 staticImageMode: false,
                                 modelComplexity: 1,
                                 minDetectionConfidence: 0.5,
                                 minTrackingConfidence: 0.5,
                                 outputVideo: "null", //at the moment this still creates an empty file with the name "null", but it won't take up space
                                 displayFrames: 1,
                                 useHandPose: 1,
                                 drawBodyPose: 1,
                                 drawHandPose: 1,
                                 UseIndependentHands: 1
                                 ));

            /*string jsonString = "{\"body\": [{\"id\": 0, \"landmarkName\": \"Nose\", \"data\": [0.0683145523071289, -0.5272032618522644, -0.2814151346683502, 0.534261167049408, 0.5520623326301575, -0.8420344591140747]}, {\"id\": 1, \"landmarkName\": \"Left eye inner\", \"data\": [0.06900090724229813, -0.5620826482772827, -0.2750793397426605, 0.542637050151825, 0.48907387256622314, -0.7762095928192139]}]}";

            BodyContainer body = JsonUtility.FromJson<BodyContainer>(jsonString);
              foreach (var landmark in body.body)
              {
                  UnityEngine.Debug.Log("Landmark ID: " + landmark.id);
                  UnityEngine.Debug.Log("Landmark Name: " + landmark.landmarkName);
                  UnityEngine.Debug.Log("Landmark Data: " + string.Join(", ", landmark.data));
              }
            */
        }

        public override void Process()
        {
    
        }

        public override void Close()
        {
            // Request cancellation of the task
            cancellationTokenSource.Cancel();
        }

        public override Pose GetPose()
        {
            return playerPose;
        }
    }
}