using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowCamScript : MonoBehaviour
{
    public GameObject CamWithLight;
    public GameObject GOToFollow;
    public Vector3 Offset;
    public Quaternion RotaceKamery;
    public bool IsActive = false;
    public float MoveSpeedCam = 10f;

    public Text NazevFollow;
    public Text MassFollow;
    public Text PoziceFollow;
    public Text RychlostFollow;
    public Text SilovePusFollow;

    public void Activate(GameObject toFollow) 
    {
        Offset = new Vector3(0f, 0f, 0f);
        CamWithLight.transform.position = GOToFollow.transform.position + Offset;
        RotaceKamery.eulerAngles = new Vector3(0f, 0f, 0f);
        CamWithLight.transform.rotation = RotaceKamery;
    }

    void Update()
    {
        if (IsActive)
        {
            //Pohyby - X
            if (Input.GetKey(KeyCode.A))
            {
                Offset += (Vector3.left / MoveSpeedCam);
            }
            if (Input.GetKey(KeyCode.D))
            {
                Offset += (Vector3.right / MoveSpeedCam);
            }

            //Pohyby - Y
            if (Input.GetKey(KeyCode.R))
            {
                Offset += (Vector3.forward / MoveSpeedCam);
            }
            if (Input.GetKey(KeyCode.F))
            {
                Offset += (Vector3.back / MoveSpeedCam);
            }

            //Pohyby - Z
            if (Input.GetKey(KeyCode.W))
            {
                Offset += (Vector3.up / MoveSpeedCam);
            }
            if (Input.GetKey(KeyCode.S))
            {
                Offset += (Vector3.down / MoveSpeedCam);
            }
            CamWithLight.transform.position = GOToFollow.transform.position + Offset;
        }
    }
}
