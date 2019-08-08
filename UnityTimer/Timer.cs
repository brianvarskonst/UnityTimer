using System;
using System.Collections.Generic;

using UnityEngine;
using Object = UnityEngine.Object;

using UnityTimer.Core;

namespace UnityTimer
{
    /**
     * Barebone
     *
     * Callback Timer
     */
    [Serializable]
    public class Timer
    {
        [SerializeField]
        private string name;
        public string Name { get; }
        
        [SerializeField] 
        private float duration;
        
        [SerializeField]
        private Action action;

        [SerializeField]
        private GameObject gameObject;

        [SerializeField]
        private bool active;
        
        [SerializeField]
        private bool useUnscaledDeltaTime;
        
        [SerializeField]
        private bool isDestroyed = false;

        /**
         * Class Constructor
         */
        private Timer(Action _action, float _duration, GameObject _gameObject, string _name, bool _useUnscaledDeltaTime)
        {
            action = _action;
            duration = _duration;
            gameObject = _gameObject;

            name = _name;
            useUnscaledDeltaTime = _useUnscaledDeltaTime;
            
            isDestroyed = false;
        }

        private static List<Timer> activeTimers;
        private static GameObject initGameObject;

        private static void Init()
        {
            if (initGameObject == null)
            {
                initGameObject = new GameObject("Timer_Init");
                activeTimers = new List<Timer>();
            }
        }

        public static Timer Create(Action action, float timer)
        {
            // Set name from Action Method Name
            string name = action.Method.Name;
            
            return Create(action, timer, name, false, false);
        }
        
        public static Timer Create(Action action, float timer, string name)
        {
            return Create(action, timer, name, false, false);
        }
        
        public static Timer Create(Action action, float timer, string name, bool useUnscaledDeltaTime)
        {
            return Create(action, timer, name, useUnscaledDeltaTime, false);
        }
        

        private static Timer Create(Action action, float duration, string name, bool useUnscaledDeltaTime, bool stopAllWithSameName)
        {
            Init();

            if (stopAllWithSameName)
            {
                StopAllWithName(name);
            }

            Debug.Log(name); 

            name = name.FirstCharToUpper();
            
            Debug.Log(name);
            
            GameObject TimerObject = new GameObject("Timer" + name, typeof(MonoBehaviourHook));

            Timer timer = new Timer(action, duration, TimerObject, name, useUnscaledDeltaTime);
            
            TimerObject.GetComponent<MonoBehaviourHook>().onUpdate = timer.Update;
            
            activeTimers.Add(timer);

            return timer;
        }

        public static void Remove(Timer timer)
        {
            Init();

            activeTimers.Remove(timer);
        }

        public static void Stop(string name)
        {
            for (int i = 0; i < activeTimers.Count; i++)
            {
                if (activeTimers[i].name == name)
                {
                    activeTimers[i].DestroyTimer();
                    i--;
                }
            }
        }


        private static void StopAllWithName(string name)
        {
            Init();

            for (int i = 0; i < activeTimers.Count; i++)
            {
                if (activeTimers[i].name == name)
                {
                    activeTimers[i].DestroyTimer();
                    i--;
                }
            }
        }

        private static void StopFirstWithName(string name)
        {
            Init();

            for (int i = 0; i < activeTimers.Count; i++)
            {
                if (activeTimers[i].name == name)
                {
                    activeTimers[i].DestroyTimer();
                    return;
                }
            }
        }

        public void Update()
        {
            if (!isDestroyed)
            {
                if (useUnscaledDeltaTime)
                {
                    duration -= Time.unscaledDeltaTime;
                }
                else
                {
                    duration -= Time.deltaTime;
                }                

                if (duration <= 0)
                {
                    action();
                    DestroySelf();
                }
            }
            else
            {
                DestroyTimer();
            }
        }

        private void DestroySelf()
        {
            isDestroyed = true;
        }

        private void DestroyTimer()
        {
           Object.Destroy(gameObject);
           Remove(this);
        }

        public void Destroy()
        {
            DestroySelf();
            DestroyTimer();
        }

        public static TimerObject CreateObject(Action callback, float duration)
        {
            return new TimerObject(callback, duration);
        }
    }
}