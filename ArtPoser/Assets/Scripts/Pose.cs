﻿using System.Collections;
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
        CustomGestureManager.customGestureManager.AddCustomGesture(poseName,poseEvent);//Adds the gesture to the list of poses to look for and sets the function to call when the pose is triggered
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
