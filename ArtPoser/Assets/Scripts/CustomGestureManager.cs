using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Windows.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;

public struct CustomGesture
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

    public void SetTrackingID(ulong id)
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
                if (frame.DiscreteGestureResults.Count > 0)
                {
                    for (int i = 0; i < gestures.Count; i++)
                    {
                        result = frame.DiscreteGestureResults[gestures[i].gesture];
                        if (result.Detected == true)
                        {
                            gestures[i].trigger.Invoke();
                        }
                    }


                }



            }


        }
    }



    // Start is called before the first frame update
    void Start()
    {
        if (customGestureManager != null)
        {
            Destroy(this);
        }
        customGestureManager = this;

        kinect = KinectSensor.GetDefault();

        gestureDatabase = VisualGestureBuilderDatabase.Create(Application.streamingAssetsPath + "/" + databaseName + ".gbd");
        gestureFrameSource = VisualGestureBuilderFrameSource.Create(kinect, 0);
        foreach (var gesture in gestureDatabase.AvailableGestures)
        {
            gestureFrameSource.AddGesture(gesture);
        }
        gestureFrameReader = gestureFrameSource.OpenReader();
        gestureFrameReader.IsPaused = true;

        }

   

    public void AddCustomGesture(string name, UnityEvent e)
    {
        foreach (var gesture in gestureDatabase.AvailableGestures)
        {
            if (gesture.Name == name)
            {
                CustomGesture tempGesture;
                tempGesture.gesture = gesture;
                tempGesture.trigger = e;
                gestures.Add(tempGesture);
            }
        }
    }
}
