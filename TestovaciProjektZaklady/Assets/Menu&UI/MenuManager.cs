using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject TimeMenu;
    public GameObject MainOptionsMenu;
    public GameObject ControlPanelCamera;
    public GameObject ControlPanelObject;
    public GameObject ControlPanelProbe;
    public GameObject ControlPanelOptions;

    public void SpeedChange(bool zvysit)
    {
        if (zvysit)
        {
            Debug.Log("Speed increased");
        }
        else
        {
            Debug.Log("Speed decreased");
        }
    }

    public void SpeedPause() 
    {
        Debug.Log("Simulation paused");
    }

    public void SpeedResume() 
    {
        Debug.Log("Simulation resumed");
    }

    
}
