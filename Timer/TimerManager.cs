using System;
using UnityEngine;

namespace Timer
{
    public class TimerManager : MonoBehaviour
    {
        private void Start()
        {
            Timer.Create(TestingAction, 3f, "Timer");
            
            Timer.Create(StopTestingAction, 6f, "StopTimer");
            
//            Timer.Stop("Timer");
        }

        private void TestingAction()
        {
            Debug.Log("Testing");
        }
        
        private void StopTestingAction()
        {
            Debug.Log("Stop Testing");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Timer.Create(() => TestingAction(), 1f);
            }
        }
    }
}