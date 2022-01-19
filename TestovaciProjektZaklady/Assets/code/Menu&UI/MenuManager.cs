using UnityEngine;
using System.IO;
using UnityEngine.UI;

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
    public GameObject CloseWindow;
    public GameObject AsteroidMenu;


    public enum ControlPanelModes 
    {
        allHidden,
        cameraCP,
        objectCP,
        probeCP,
        optionsCP
    }
        

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

    public void SpeedPausePlay(bool isPause)
    {
        if (isPause)
        {
            PlayButton.SetActive(true);
            Debug.Log("Simulation paused");
            PauseButton.SetActive(false);
        }
        else
        {
            PauseButton.SetActive(true);
            Debug.Log("Simulation resumed");
            PlayButton.SetActive(false);
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

    public void SaveToFile(int fileNumber) 
    {
        
        string path = Application.persistentDataPath + "/LoadFile" + fileNumber.ToString() + ".slf";

        SimulationData dataToSave = new SimulationData(ObjektySimulace, TimeManager.GetComponent<TimeManager>());
        string json = JsonUtility.ToJson(dataToSave, true);
        //JsonUtility.FromJson(json, SimulationData);

        StreamWriter sw = new StreamWriter(path);

        sw.Write(json);

        Debug.Log(path);
        sw.Close();
    }
}
