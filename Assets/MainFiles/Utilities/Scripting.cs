using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils { 
    public class Scripting : MonoBehaviour
    {
        public enum DebugMode
        {
            Buttons,
            AgentBehavior,
            AgentMovement,
            Streaming
        }

        public static Dictionary<DebugMode, bool> debugModes = new Dictionary<DebugMode, bool>()
        {
            { DebugMode.Buttons, false },
            { DebugMode.AgentBehavior, false },
            { DebugMode.AgentMovement, false },
            { DebugMode.Streaming, false }
        };
        public static void Log(List<DebugMode> modes, string msg, bool alwaysRun = false)
        {
            if (alwaysRun)
            {
                Debug.Log(msg);
            }
            else
            {
                foreach (DebugMode mode in modes)
                {
                    if (debugModes[mode])
                    {
                        Debug.Log(msg);
                        break;
                    }
                }
            }                
        }
    }
}
