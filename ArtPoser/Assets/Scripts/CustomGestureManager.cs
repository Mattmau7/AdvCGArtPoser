using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Windows.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;



public class CustomGestureManager : MonoBehaviour
{
    VisualGestureBuilderDatabase gestureDatabase;
    VisualGestureBuilderFrameSource gestureFrameSource;
    VisualGestureBuilderFrameReader gestureFrameReader;
    KinectSensor kinect;
    
    List<Gesture> gestures = new List<Gesture>();

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
                        result = frame.DiscreteGestureResults[gestures[i]];
                        if (result.Detected == true)
                        {
                            Debug.Log("Recognized gesture: " + gestures[i].Name);
                        }
                    }


                }



            }


        }
    }



    // Start is called before the first frame update
    void Start()
    {
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

   

    public void AddCustomGesture(string name)
    {
        foreach (var gesture in gestureDatabase.AvailableGestures)
        {
            if (gesture.Name == name)
            {
                gestures.Add(gesture);
            }
        }
    }
}
