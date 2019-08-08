using UnityEngine;

using UnityTimer;

public class TestTimer : MonoBehaviour
{
    private void Awake()
    {
    }

    private void Start()
    {
        Timer.Create(TestingAction, 3f, "testingAction");


//        TimerManager.Add(TestingAction, 3f, "testingTimer");
//        
//        TimerManager.Remove("testingTimer");

//        
//       Timer.Create(TestingAction, 3f, "Timer");
//            
//       Timer.Create(StopTestingAction, 6f, "StopTimer");

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
    {}
}