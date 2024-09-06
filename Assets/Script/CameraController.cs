using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject arCamera;
    bool scene2;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (scene2)
        {
            this.transform.position = arCamera.transform.position;
            this.transform.rotation = arCamera.transform.rotation;
        }
    }

    public void FollowARCamera()
    {
        arCamera = GameObject.Find("ARCamera");
        scene2 = true;
    }
}