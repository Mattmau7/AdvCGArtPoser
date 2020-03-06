using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Windows.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;

public struct CustomGesture//structure containing both a gesture and the function to call when its detected
{
    public Gesture gesture;
    public UnityEvent trigger;
};

public class CustomGestureManager : MonoBehaviour
{
    public static CustomGestureManager customGestureManager;


    VisualGestureBuilderDatabase gestureDatabase;
    VisualGestureBuilderFrameSource gestureFrameSource;
    VisualGestureBuilderFrameReader gestureFrameReader;
    KinectSensor kinect;
    
    List<CustomGesture> gestures = new List<CustomGesture>();

    [SerializeField]
    string databaseName;

    public void SetTrackingID(ulong id)//set the tracking id
    {
        gestureFrameReader.IsPaused = false;
        gestureFrameSource.TrackingId = id;
        gestureFrameReader.FrameArrived += FrameArrived;
    }

    void FrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs e)
    {
        VisualGestureBuilderFrameReference frameReference = e.FrameReference;
        using (VisualGestureBuilderFrame frame = frameReference.AcquireFrame())
        {
            if (frame != null && frame.DiscreteGestureResults != null)
            {
                DiscreteGestureResult result = null;
                if (frame.DiscreteGestureResults.Count > 0)//if there is one or more potential gesture being detected by the kinect
                {
                    for (int i = 0; i < gestures.Count; i++)//for each gesture in the database
                    {
                        result = frame.DiscreteGestureResults[gestures[i].gesture];//get the result of gesture i
                        if (result.Detected == true)//if gesture i was detected
                        {
                            gestures[i].trigger.Invoke();//call the unityevent attacked to the gesture
                        }
                    }


                }



            }


        }
    }



    // Start is called before the first frame update
    void Start()
    {
        if (customGestureManager != null)//if an instance of custom gesture manager already exists
        {
            Destroy(this);//destroy this script
        }
        customGestureManager = this;//set the singleton

        kinect = KinectSensor.GetDefault();//get the kinect

        gestureDatabase = VisualGestureBuilderDatabase.Create(Application.streamingAssetsPath + "/" + databaseName + ".gbd");//get the database
        gestureFrameSource = VisualGestureBuilderFrameSource.Create(kinect, 0);//create the frame source
        foreach (var gesture in gestureDatabase.AvailableGestures)
        {
            gestureFrameSource.AddGesture(gesture);//add all the available gestures in the database to the frame source
        }
        gestureFrameReader = gestureFrameSource.OpenReader();
        gestureFrameReader.IsPaused = true;

        }

   

    public void AddCustomGesture(string name, UnityEvent e)//Adds the gesture to the list of poses to look for and sets the function to call when the pose is triggered
    {
        foreach (var gesture in gestureDatabase.AvailableGestures)// loop for all gestures available in the database
        {
            if (gesture.Name == name)//if the gesture is found
            {
                CustomGesture tempGesture;//create the new struct
                tempGesture.gesture = gesture;
                tempGesture.trigger = e;
                gestures.Add(tempGesture);//add it to the list
            }
        }
    }



   


}
