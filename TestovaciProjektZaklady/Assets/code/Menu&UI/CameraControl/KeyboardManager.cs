using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardManager : InputManager
{
    public static event MoveInputHandler OnMoveInput;
    public static event RotateInputHandler OnRotateInput;

    // Update is called once per frame
    void Update()
    {
        //Pohyby - X
        if (Input.GetKey(KeyCode.D))
        {
            OnMoveInput?.Invoke(Vector3.left);
        }
        if (Input.GetKey(KeyCode.A))
        {
            OnMoveInput?.Invoke(Vector3.right);
        }

        //Pohyby - Y
        if (Input.GetKey(KeyCode.W))
        {
            OnMoveInput?.Invoke(Vector3.forward);
        }
        if (Input.GetKey(KeyCode.S))
        {
            OnMoveInput?.Invoke(Vector3.back);
        }

        //Pohyby - Z
        if (Input.GetKey(KeyCode.R))
        {
            OnMoveInput?.Invoke(Vector3.up);
        }
        if (Input.GetKey(KeyCode.F))
        {
            OnMoveInput?.Invoke(Vector3.down);
        }

        //Rotace
        if (Input.GetKey(KeyCode.E))
        {
            OnRotateInput?.Invoke(-1f);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            OnRotateInput?.Invoke(1f);
        }
    }
}
