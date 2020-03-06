using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pose : MonoBehaviour
{
    public string poseName;
    public UnityEvent poseEvent;
    
    
    // Start is called before the first frame update
    void Start()
    {
      
    }
    public void RegisterGestures()
    {
        CustomGestureManager.customGestureManager.AddCustomGesture(poseName, poseEvent);//Adds the gesture to the list of poses to look for and sets the function to call when the pose is triggered
    }
    public string GetGestureName()
    {
        return poseName;
    }
    public UnityEvent GetEvent()
    {
        return poseEvent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
