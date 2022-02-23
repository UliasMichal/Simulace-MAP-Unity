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
    public GameObject KameraASvetla;

    //Time
    public GameObject TimeMenu;
    public GameObject PauseButton;
    public GameObject PlayButton;

    //ControlPanel
    public Dropdown CPSelect;
    public GameObject ControlPanelCamera;
    public InputField PoziceX;
    public InputField PoziceY;
    public InputField PoziceZ;
    public InputField RotaceX;
    public InputField RotaceY;
    public InputField RotaceZ;
    public GameObject ControlPanelObject;
    public GameObject ControlPanelProbe;
    public GameObject ControlPanelOptions;

    //Pop-ups
    public GameObject ClosePU;
    public GameObject FilePU;
    public GameObject ErrorPU;
    public GameObject SelectPlanetPU;
    public GameObject PlanetOptionsPU;
    public GameObject CreatePU;
    public GameObject DeletePU;
    public GameObject UpdatePU;

    //Vlastnosti pop-upù
    public Dropdown DropboxPlanetOptionsPU;
    public Toggle PopisekCM;
    public Toggle DrahyCM;
    public Toggle SilocaryCM;
    public Toggle NoGravCM;
    public Toggle NoMoveCM;
    public Dropdown DropboxPlanetSelectPU;
    public Text InfoMass;
    public Text InfoPosition;
    public Text InfoRychlost;
    public Text InfoSilovePusobeni;

    //Activate
    private bool InfoActivated = false;


    public enum ControlPanelModes 
    {
        allHidden = 0,
        cameraCP = 1,
        objectCP = 2,
        probeCP = 3,
        optionsCP = 4
    }

    public enum OptionsPlanetCheckmarks 
    {
        popisekCM = 0,
        drahyCM = 1,
        silocaryCM = 2,
        noGravCM = 3,
        noMoveCM = 4,
    }

    private void FixedUpdate()
    {
        if (Screen.width < 450 || Screen.height < 400)
        {
            Screen.SetResolution(450, 400, false);
        }
        if (InfoActivated) 
        {
            LoadPlanetInfo();
        }
    }

    private void LoadPlanetInfo()
    {
        SpaceObject hledanaPlaneta = SpaceObject.GetChild(ObjektySimulace.transform, DropboxPlanetSelectPU.options[DropboxPlanetSelectPU.value].text).gameObject.GetComponent<SpaceObject>();
        InfoPosition.text = ParserVector3(hledanaPlaneta.transform.position * 100000, " km\n");
        InfoMass.text = hledanaPlaneta.mass.ToString();
        InfoRychlost.text = ParserVector3(hledanaPlaneta.rychlost, " km/s\n");
        InfoSilovePusobeni.text = ParserVector3(hledanaPlaneta.aktualniSilovePusobeni, " km/s\n");
    }

    private string ParserVector3(Vector3 toConvert, string endlineSJednotkou) 
    {
        string s = "x: " + toConvert.x + endlineSJednotkou + "y: " + toConvert.y + endlineSJednotkou + "z: " + toConvert.z + endlineSJednotkou;
        return s;
    }

    public void SetUpdatingInfoPlanet(bool toChange) 
    {
        InfoActivated = toChange;
    }

    public void LoadSettingsPlanet() 
    {
        SpaceObject hledanaPlaneta = SpaceObject.GetChild(ObjektySimulace.transform, DropboxPlanetOptionsPU.options[DropboxPlanetOptionsPU.value].text).gameObject.GetComponent<SpaceObject>();
        PopisekCM.isOn = hledanaPlaneta.zobrazitPopisek;
        DrahyCM.isOn = hledanaPlaneta.zobrazitDrahy;
        SilocaryCM.isOn = hledanaPlaneta.zobrazitSilocary;

        NoGravCM.isOn = hledanaPlaneta.noGravityEffect;
        NoMoveCM.isOn = hledanaPlaneta.noMovement;
    }

    public void ChangeOfCM(int infoCM) 
    {
        SpaceObject hledanaPlaneta = SpaceObject.GetChild(ObjektySimulace.transform, DropboxPlanetOptionsPU.options[DropboxPlanetSelectPU.value].text).gameObject.GetComponent<SpaceObject>();

        if (infoCM == (int)OptionsPlanetCheckmarks.popisekCM) 
        {
            hledanaPlaneta.zobrazitPopisek = PopisekCM.isOn;
        }
        if (infoCM == (int)OptionsPlanetCheckmarks.drahyCM)
        {
            hledanaPlaneta.zobrazitDrahy = DrahyCM.isOn;
        }
        if (infoCM == (int)OptionsPlanetCheckmarks.silocaryCM)
        {
            hledanaPlaneta.zobrazitSilocary = SilocaryCM.isOn;
        }

        if (infoCM == (int)OptionsPlanetCheckmarks.noGravCM)
        {
            hledanaPlaneta.noGravityEffect = NoGravCM.isOn;
        }
        if (infoCM == (int)OptionsPlanetCheckmarks.noMoveCM)
        {
            hledanaPlaneta.noMovement = NoMoveCM.isOn;
        }
    }

    public void UpdateDropboxDlePlanet(Dropdown dbToUpdate)
    {
        dbToUpdate.options.Clear();
        foreach (Transform t in ObjektySimulace.transform) 
        {
            dbToUpdate.options.Add(new Dropdown.OptionData() { text = t.name.ToString()});
        }
    }

    public void OpenPlanetOptionsPU()
    {
        CloseAllPopUps();
        UpdateDropboxDlePlanet(DropboxPlanetOptionsPU);
        LoadSettingsPlanet();
        PlanetOptionsPU.SetActive(true);
    }
    
    public void OpenSelectPlanetPU()
    {
        CloseAllPopUps();
        SetUpdatingInfoPlanet(false);
        UpdateDropboxDlePlanet(DropboxPlanetSelectPU);
        SelectPlanetPU.SetActive(true);
        SetUpdatingInfoPlanet(true);
    }

    public void SetPoziceKamery()
    {
        float xPos = ParserProOddelovace(PoziceX.text);
        if (xPos == float.NaN) 
        {
            OpenErrorPU(xPos + " - není validní èíslo pro pozici");
        }

        float yPos = ParserProOddelovace(PoziceY.text);
        if (yPos == float.NaN)
        {
            OpenErrorPU(yPos + " - není validní èíslo pro pozici");
        }

        float zPos = ParserProOddelovace(PoziceZ.text);
        if (zPos == float.NaN)
        {
            OpenErrorPU(zPos + " - není validní èíslo pro pozici");
        }

        float xRot = ParserProOddelovace(RotaceX.text);
        if (xRot == float.NaN)
        {
            OpenErrorPU(xRot + " - není validní èíslo pro rotaci");
        }

        float yRot = ParserProOddelovace(RotaceY.text);
        if (yRot == float.NaN)
        {
            OpenErrorPU(yRot + " - není validní èíslo pro rotaci");
        }

        float zRot = ParserProOddelovace(RotaceZ.text);
        if (zRot == float.NaN)
        {
            OpenErrorPU(zRot + " - není validní èíslo pro rotaci");
        }

        KameraASvetla.transform.position = new Vector3(xPos, yPos, zPos);

        Quaternion qRotace = new Quaternion();
        qRotace.eulerAngles = new Vector3(xRot, yRot, zRot);
        KameraASvetla.transform.rotation = qRotace;
    }

    private float ParserProOddelovace(string input) 
    {
        if(float.TryParse(input.Replace(',', '.'), out float teckaVal)) 
        {
            return teckaVal;
        }
        if (float.TryParse(input.Replace(',', '.'), out float carkaVal)) 
        {
            return carkaVal;
        }
        return float.NaN;
    }

    public void CloseAllPopUps() 
    {
        ClosePU.SetActive(false);
        FilePU.SetActive(false);
        ErrorPU.SetActive(false);
        SelectPlanetPU.SetActive(false);
        PlanetOptionsPU.SetActive(false);
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
