using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveCamSimple : MonoBehaviour
{
    public GameObject CamWithLight;
    public Text KameraX;
    public Text KameraY;
    public Text KameraZ;
    public Text KameraRotX;
    public Text KameraRotY;
    public Text KameraRotZ;

    public float MoveSpeedCam = 10f;
    public float bounds = 7500f;

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
                CamWithLight.transform.Translate(Vector3.left / MoveSpeedCam);
                SetCameraToBounds();
            }
            if (Input.GetKey(KeyCode.D))
            {
                CamWithLight.transform.Translate(Vector3.right / MoveSpeedCam);
                SetCameraToBounds();
            }

            //Pohyby - Y
            if (Input.GetKey(KeyCode.R))
            {
                CamWithLight.transform.Translate(Vector3.forward / MoveSpeedCam);
                SetCameraToBounds();
            }
            if (Input.GetKey(KeyCode.F))
            {
                CamWithLight.transform.Translate(Vector3.back / MoveSpeedCam);
                SetCameraToBounds();
            }

            //Pohyby - Z
            if (Input.GetKey(KeyCode.W))
            {
                CamWithLight.transform.Translate(Vector3.up / MoveSpeedCam);
                SetCameraToBounds();
            }
            if (Input.GetKey(KeyCode.S))
            {
                CamWithLight.transform.Translate(Vector3.down / MoveSpeedCam);
                SetCameraToBounds();
            }

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
}
