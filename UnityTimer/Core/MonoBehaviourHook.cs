using System;
using UnityEngine;

namespace UnityTimer.Core
{
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
}