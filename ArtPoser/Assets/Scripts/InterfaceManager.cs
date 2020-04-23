using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{

    public GameObject menuPanel, pose, room;
    public Slider poseSlider;
    public InputField x, y, z, pX, pY, pZ;
    public Camera camera;
    public Light theLight;
    public Mesh[] roomTypes;

    // Start is called before the first frame update
    void Start()
    {
        x.text = theLight.transform.position.x.ToString();
        y.text = theLight.transform.position.y.ToString();
        z.text = theLight.transform.position.z.ToString();

        pX.text = pose.transform.position.x.ToString();
        pY.text = pose.transform.position.y.ToString();
        pZ.text = pose.transform.position.z.ToString();

    }

    //toggles panel visibility
    public void ToggleMenu()
    {
        menuPanel.SetActive(!menuPanel.activeSelf);
    }

    //rotates the pose
    public void RotatePose()
    {
        pose.transform.eulerAngles = new Vector3(0, poseSlider.value, 0);
    }

    //updates light source position
    public void MoveLightSource()
    {
        if (x.text != "" && y.text != "" && z.text != "")
        theLight.transform.position = new Vector3(float.Parse(x.text), float.Parse(y.text), float.Parse(z.text));
    }

    //updates pose movement
    public void MovePose()
    {
        if (pX.text != "" && pY.text != "" && pZ.text != "")
            pose.transform.position = new Vector3(float.Parse(pX.text), float.Parse(pY.text), float.Parse(pZ.text));
    }

    //updates the camera zoom
    public void MoveCamera(float moveVal)
    {
        Vector3 pos = camera.transform.position;
        pos[2] += moveVal;
        camera.transform.position = pos;
    }

    //changes the environment
    public void ChangeEnv(Dropdown drop)
    {
        if (drop.value < roomTypes.Length)
        {
            room.SetActive(true);
            room.GetComponent<MeshFilter>().mesh = roomTypes[drop.value];
        }
        else
            room.SetActive(false);
            
    }

    //changes the light type
    public void ChangeLight(Dropdown drop)
    {
        theLight.type = (LightType)drop.value;
    }
}