using System;
using System.Collections.Generic;

using UnityEngine;

using UnityTimer.Core;

namespace UnityTimer
{
    public class TimerManager : Singleton<TimerManager>
    {
        [SerializeField]
        private static List<Timer> timers = new List<Timer>();

        private void Start()
        {}

        private void Update()
        {}

        public static void Add(Action action, float duration, string name)
        {
            Timer timer = Timer.Create(action, duration, name);
            
            timers.Add(timer);
        }

        public static void Remove(string name)
        {
            for (int i = 0; i < timers.Count; i++)
            {
                if (timers[i].Name == name)
                {
                    timers.Remove(timers[i]);
                    
                    timers[i].Destroy();
                    
                    return;
                }
            }
        }

//        public static void Stop()
//        {
//            
//        }
//
//        public static void StopAll()
//        {
//            
//        }

        public static bool Reset()
        {
            timers.Clear();
            
            return (timers.Count <= 0); 
        }
    }
}