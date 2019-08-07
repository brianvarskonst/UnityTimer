using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityTimer
{
    /**
     * Barebone
     *
     * Callback Timer
     */
    public class Timer
    {
        // Hook Class to have access to MonoBehaviour Methods
        public class MonoBehaviourHook : MonoBehaviour
        {
            public Action onUpdate = null;

            private void Update()
            {
                if (onUpdate != null)
                {
                    onUpdate();
                }
            }
        }
        
        private Action action;
        private float duration;
        private string name;
        
        private GameObject gameObject;

        private bool active;
        private bool useUnscaledDeltaTime;
        
        private bool isDestroyed = false;

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
            return Create(action, timer, "", false, false);
        }
        
        public static Timer Create(Action action, float timer, string name)
        {
            return Create(action, timer, name, false, false);
        }
        
        public static Timer Create(Action action, float timer, string name, bool useUnscaledDeltaTime)
        {
            return Create(action, timer, name, useUnscaledDeltaTime, false);
        }
        

        private static Timer Create(Action _action, float _duration, string _name, bool useUnscaledDeltaTime, bool stopAllWithSameName)
        {
            Init();

            if (stopAllWithSameName)
            {
                StopAllWithName(_name);
            }
            
            GameObject TimerObject = new GameObject("Timer " + _name, typeof(MonoBehaviourHook));

            Timer timer = new Timer(_action, _duration, TimerObject, _name, useUnscaledDeltaTime);
            
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

        public class TimerObject
        {
            private float duration;
            private Action callback;

            public TimerObject(Action _callback, float _duration)
            {
                callback = _callback;
                duration = _duration;
            }

            public void Update()
            {
                Update(Time.deltaTime);
            }

            public void Update(float time)
            {
                duration -= time;

                if (duration <= 0)
                {
                    callback();
                }
            }
        }

        public static TimerObject CreateObject(Action callback, float duration)
        {
            return new TimerObject(callback, duration);
        }
    }
}