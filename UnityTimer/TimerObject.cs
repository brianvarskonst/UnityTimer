using System;
using UnityEngine;

namespace UnityTimer
{
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
}