using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamSimple : MonoBehaviour
{
    public GameObject CamWithLight;

    public float MoveSpeedCam = 10f;

    // Update is called once per frame
    void Update()
    {
        //Pohyby - X
        if (Input.GetKey(KeyCode.A))
        {
            CamWithLight.transform.Translate(Vector3.left * Time.deltaTime * MoveSpeedCam);
            Debug.Log(Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            CamWithLight.transform.Translate(Vector3.right * MoveSpeedCam);
        }

        //Pohyby - Y
        if (Input.GetKey(KeyCode.R))
        {
            CamWithLight.transform.Translate(Vector3.forward * MoveSpeedCam);
        }
        if (Input.GetKey(KeyCode.F))
        {
            CamWithLight.transform.Translate(Vector3.back * MoveSpeedCam);
        }

        //Pohyby - Z
        if (Input.GetKey(KeyCode.W))
        {
            CamWithLight.transform.Translate(Vector3.up * MoveSpeedCam);
        }
        if (Input.GetKey(KeyCode.S))
        {
            CamWithLight.transform.Translate(Vector3.down * MoveSpeedCam);
        }
        
        //Rotace
        
        if (Input.GetKey(KeyCode.E))
        {
            CamWithLight.transform.Rotate(Vector3.up, 2f/MoveSpeedCam);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            CamWithLight.transform.Rotate(Vector3.down, 2f/MoveSpeedCam);
        }
        

    }
}
