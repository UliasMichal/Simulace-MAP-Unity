using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cam;

    Vector3 frameMove;

    // Update is called once per frame
    private void OnEnable()
    {
        KeyboardManager.OnMoveInput += UpdateFrameMove;
        //KeyboardManager.OnRotateInput += UpdateFrameRotate;
    }
    private void OnDisable()
    {
        //Kv�li Unity - kdy� se zni�� kamera, tak by vznikal error
        KeyboardManager.OnMoveInput -= UpdateFrameMove;
        //KeyboardManager.OnRotateInput -= UpdateFrameRotate;
    }

    private void UpdateFrameMove(Vector3 pohybKamery) 
    {
        frameMove += pohybKamery;
    }

    /*private void UpdateFrameRotate(float rotate)
    {
    }*/

    private void LateUpdate()
    {
        if(frameMove != Vector3.zero) 
        {
            Debug.Log("funguje");
            Vector3 rychlostZaFrame = new Vector3(frameMove.x, frameMove.y, frameMove.z);
            transform.position += transform.TransformDirection(rychlostZaFrame); //p�evede na sm�r, kter�m m��� u�ivatel
            frameMove = Vector3.zero;
        }
    }
}
