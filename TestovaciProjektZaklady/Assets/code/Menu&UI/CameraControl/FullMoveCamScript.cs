using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FullMoveCamScript : MonoBehaviour
{
    public GameObject CamWithLight;
    public Text KameraX;
    public Text KameraY;
    public Text KameraZ;
    public Text KameraRotX;
    public Text KameraRotY;
    public Text KameraRotZ;

    public GameObject ObjectOfCamera;
    public GameObject CenterObject;

    public Vector3 OffsetPos;

    public float MoveSpeedCam = 10f;
    public float bounds = 7500f;

    /*
    Tlaèítko na vypnutí sledování
    Informaèní text hodnoty
     */

    public void Activate(GameObject toFollow)
    {
        ObjectOfCamera = toFollow;
        OffsetPos = new Vector3(0f, 0f, 0f);
    }

    public void Deactivate()
    {
        OffsetPos = CamWithLight.transform.position;
        ObjectOfCamera = CenterObject;
    }

    private void FixedUpdate()
    {
        KameraX.text = CamWithLight.transform.position.x.ToString();
        KameraY.text = CamWithLight.transform.position.y.ToString();
        KameraZ.text = CamWithLight.transform.position.z.ToString();
        KameraRotX.text = CamWithLight.transform.localRotation.eulerAngles.x.ToString() + "°";
        KameraRotY.text = CamWithLight.transform.localRotation.eulerAngles.y.ToString() + "°";
        KameraRotZ.text = CamWithLight.transform.localRotation.eulerAngles.z.ToString() + "°";
    }


    void Update()
    {
        /*
        Ovládání kamery:
        WASDRF = pohyb v osách
        Tab + WASDRF = otáèení kolem os
        Shift + WASDRF = 100x rychlejší pohyb kolem os
        */
        //pøidat podmínku: pokud není FollowCamScript active

        if (!Input.GetKey(KeyCode.Tab))
        {
            //Zrychlení pomocí levého shiftu
            if (Input.GetKey(KeyCode.LeftShift))
            {
                MoveSpeedCam = 1f / 10f;
            }

            #region Pohyby
            //Pohyby - X
            if (Input.GetKey(KeyCode.A))
            {
                OffsetPos += (Vector3.left / MoveSpeedCam);
            }
            if (Input.GetKey(KeyCode.D))
            {
                OffsetPos += (Vector3.right / MoveSpeedCam);
            }

            //Pohyby - Y
            if (Input.GetKey(KeyCode.R))
            {
                OffsetPos += (Vector3.forward / MoveSpeedCam);
            }
            if (Input.GetKey(KeyCode.F))
            {
                OffsetPos += (Vector3.back / MoveSpeedCam);
            }

            //Pohyby - Z
            if (Input.GetKey(KeyCode.W))
            {
                OffsetPos += (Vector3.up / MoveSpeedCam);
            }
            if (Input.GetKey(KeyCode.S))
            {
                OffsetPos += (Vector3.down / MoveSpeedCam);
            }

            SetCameraToBounds();
            MoveSpeedCam = 10f;

            #endregion

        }
        else
        {
            #region Rotace
            //Rotace - X
            if (Input.GetKey(KeyCode.A))
            {
                CamWithLight.transform.Rotate(Vector3.left / MoveSpeedCam);
            }
            if (Input.GetKey(KeyCode.D))
            {
                CamWithLight.transform.Rotate(Vector3.right / MoveSpeedCam);
            }

            //Rotace - Y
            if (Input.GetKey(KeyCode.R))
            {
                CamWithLight.transform.Rotate(Vector3.forward / MoveSpeedCam);
            }
            if (Input.GetKey(KeyCode.F))
            {
                CamWithLight.transform.Rotate(Vector3.back / MoveSpeedCam);
            }

            //Rotace - Z
            if (Input.GetKey(KeyCode.W))
            {
                CamWithLight.transform.Rotate(Vector3.up / MoveSpeedCam);
            }
            if (Input.GetKey(KeyCode.S))
            {
                CamWithLight.transform.Rotate(Vector3.down / MoveSpeedCam);
            }
            #endregion
        }
        CamWithLight.transform.position = ObjectOfCamera.transform.position + OffsetPos;

    }

    void SetCameraToBounds()
    {
        float x = CamWithLight.transform.position.x;
        float y = CamWithLight.transform.position.y;
        float z = CamWithLight.transform.position.z;
        if (x > bounds)
        {
            x = bounds;
        }
        if (y > bounds)
        {
            y = bounds;
        }
        if (z > bounds)
        {
            z = bounds;
        }
        CamWithLight.transform.position = new Vector3(x, y, z);
    }
}

