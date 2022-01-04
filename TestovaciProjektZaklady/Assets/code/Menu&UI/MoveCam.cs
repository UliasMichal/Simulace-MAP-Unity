using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody cam;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("left"))
        {
            Vector3 positionLeft = new Vector3(cam.position.x-0.1f, cam.position.y, cam.position.z);
            cam.MovePosition(positionLeft);
        }
        if (Input.GetKeyDown("right"))
        {
            Vector3 positionLeft = new Vector3(cam.position.x + 0.1f, cam.position.y, cam.position.z);
            cam.MovePosition(positionLeft);
        }
        if (Input.GetKeyDown("down"))
        {
            Vector3 positionLeft = new Vector3(cam.position.x, cam.position.y - 0.1f, cam.position.z);
            cam.MovePosition(positionLeft);
        }
        if (Input.GetKeyDown("up"))
        {
            Vector3 positionLeft = new Vector3(cam.position.x, cam.position.y + 0.1f, cam.position.z);
            cam.MovePosition(positionLeft);
        }
        //Vector3 positionRight = new Vector3(cam.position.x - 0.1f, cam.position.y, cam.position.z);
    }
}
