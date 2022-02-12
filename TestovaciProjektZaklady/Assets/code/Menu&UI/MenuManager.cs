using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    //Ovládací objekty
    public GameObject ObjektySimulace;
    public GameObject TimeManager;

    //Time
    public GameObject TimeMenu;
    public GameObject PauseButton;
    public GameObject PlayButton;

    //ControlPanel
    public Dropdown CPSelect;
    public GameObject ControlPanelCamera;
    public GameObject ControlPanelObject;
    public GameObject ControlPanelProbe;
    public GameObject ControlPanelOptions;

    //Pop-ups
    public GameObject ClosePU;
    public GameObject FilePU;
    public GameObject ErrorPU;
    public GameObject CreatePU;
    public GameObject DeletePU;
    public GameObject UpdatePU;

    //Vlastnosti pop-upù
    

    public enum ControlPanelModes 
    {
        allHidden = 0,
        cameraCP = 1,
        objectCP = 2,
        probeCP = 3,
        optionsCP = 4
    }
    private void FixedUpdate()
    {
        if (Screen.width < 450 || Screen.height < 400)
        {
            Screen.SetResolution(450, 400, false);
        }
    }

    public void CloseAllPopUps() 
    {
        ClosePU.SetActive(false);
        FilePU.SetActive(false);
        ErrorPU.SetActive(false);
    }

    public void OpenClosePU() 
    {
        SpeedPausePlay(true);
        CloseAllPopUps();
        ClosePU.SetActive(true);
    }

    public void OpenFilePU()
    {
        SpeedPausePlay(true);
        CloseAllPopUps();
        FilePU.SetActive(true);
    }

    public void OpenErrorPU(string errorText)
    {
        SpeedPausePlay(true);
        CloseAllPopUps();
        ErrorPU.transform.Find("PopUpText").GetComponent<Text>().text = errorText;
        ErrorPU.SetActive(true);
    }

    public void SpeedChange(bool zvysit)
    {
        if (zvysit)
        {
            Debug.Log("Speed increased");
            TimeManager.GetComponent<TimeManager>().ZmenaCasovehoNasobku(1);
        }
        else
        {
            Debug.Log("Speed decreased");
            TimeManager.GetComponent<TimeManager>().ZmenaCasovehoNasobku(-1);
        }
    }

    public void SpeedPausePlay(bool simulationToPause)
    {
        if (simulationToPause)
        {
            PlayButton.SetActive(true);
            Debug.Log("Simulation paused");
            PauseButton.SetActive(false);
            TimeManager.GetComponent<TimeManager>().ZastavitCas();
        }
        else
        {
            PauseButton.SetActive(true);
            Debug.Log("Simulation resumed");
            PlayButton.SetActive(false);
            TimeManager.GetComponent<TimeManager>().SpustitCas();
        }
    }

    public void ControlPanelChange()
    {
        if (CPSelect.value == (int)ControlPanelModes.allHidden)
        {
            ControlPanelCamera.SetActive(false);
            ControlPanelObject.SetActive(false);
            ControlPanelProbe.SetActive(false);
            ControlPanelOptions.SetActive(false);

            return;
        }
        if (CPSelect.value == (int)ControlPanelModes.cameraCP)
        {
            ControlPanelCamera.SetActive(true);

            ControlPanelObject.SetActive(false);
            ControlPanelProbe.SetActive(false);
            ControlPanelOptions.SetActive(false);

            return;
        }
        if (CPSelect.value == (int)ControlPanelModes.objectCP)
        {
            ControlPanelObject.SetActive(true);

            ControlPanelCamera.SetActive(false);
            ControlPanelProbe.SetActive(false);
            ControlPanelOptions.SetActive(false);

            return;
        }
        if (CPSelect.value == (int)ControlPanelModes.probeCP)
        {
            ControlPanelProbe.SetActive(true);

            ControlPanelCamera.SetActive(false);
            ControlPanelObject.SetActive(false);
            ControlPanelOptions.SetActive(false);

            return;
        }
        if (CPSelect.value == (int)ControlPanelModes.optionsCP)
        {
            ControlPanelOptions.SetActive(true);

            ControlPanelCamera.SetActive(false);
            ControlPanelObject.SetActive(false);
            ControlPanelProbe.SetActive(false);

            return;
        }
    }
    public void CreateSpaceObject()
    {
        SpaceObjectData sod = GetDataFromMain();

        GameObject newObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        newObject.transform.parent = this.transform;

        AddSpaceObject(newObject, sod);
        AddTrailRenderer(newObject);
    }

    SpaceObjectData GetDataFromMain()
    {
        SpaceObjectData sod = new SpaceObjectData();
        return sod;
    }

    void AddSpaceObject(GameObject toAdd, SpaceObjectData sod)
    {
        toAdd.AddComponent<SpaceObject>();

        toAdd.GetComponent<SpaceObject>().LoadFromData(sod);

        toAdd.GetComponent<SpaceObject>().vsechnaSilovaPusobeni = new List<Vector3>();
    }

    void AddTrailRenderer(GameObject toAdd)
    {
        toAdd.AddComponent<TrailRenderer>();
        toAdd.GetComponent<TrailRenderer>().widthMultiplier = 0.5f;
        toAdd.GetComponent<TrailRenderer>().time = float.PositiveInfinity;
        toAdd.GetComponent<TrailRenderer>().enabled = true;
        toAdd.GetComponent<TrailRenderer>().emitting = true;
    }

    public void SaveToFile(int fileNumber) 
    {
        
        string path = Application.persistentDataPath + "/LoadFile" + fileNumber.ToString() + ".slf";

        SimulationData dataToSave = new SimulationData(ObjektySimulace, TimeManager.GetComponent<TimeManager>());
        string json = JsonUtility.ToJson(dataToSave, true);

        StreamWriter sw = new StreamWriter(path);

        sw.Write(json);

        Debug.Log(path);
        sw.Close();
        ExitApp();
    }

    public void ExitApp()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
