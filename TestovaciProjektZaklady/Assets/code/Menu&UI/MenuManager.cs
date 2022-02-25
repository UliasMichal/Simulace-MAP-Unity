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
    public GameObject PlanetMoveToPU;
    public GameObject PlanetFollowPU;
    public GameObject CreateOrUpdateValuePU;
    public GameObject SelectToUpdatePU;
    public GameObject DeletePU;

    //Vlastnosti pop-upù
    #region Vlastnosti pop-upù
    //Planet options
    public Dropdown DropboxPlanetOptionsPU;
    public Toggle PopisekCM;
    public Toggle DrahyCM;
    public Toggle SilocaryCM;
    public Toggle NoGravCM;
    public Toggle NoMoveCM;

    //Planet info
    public Dropdown DropboxPlanetSelectPU;
    public Text InfoMass;
    public Text InfoPosition;
    public Text InfoRychlost;
    public Text InfoSilovePusobeni;

    //Planet move to
    public Dropdown DropboxMoveToPlanetPU;

    //Planet follow
    public Dropdown DropboxFollowPlanetPU;

    //Planet destroy
    public Dropdown DropboxDestroyPlanetPU;

    //Planet select update
    public Dropdown DropboxUpdatePlanetPU;

    //Planet create or update
    public Text CreateTitle;
    public Text UpdateTitle;
    public InputField CUNazevObjektu;
    public InputField CUMassObjektu;
    public InputField CUPolohaX;
    public InputField CUPolohaY;
    public InputField CUPolohaZ;
    public InputField CURychlostX;
    public InputField CURychlostY;
    public InputField CURychlostZ;
    public InputField CURed;
    public InputField CUGreen;
    public InputField CUBlue;
    public Image CUColorPalette;
    public InputField CUSize; 
    public Toggle CUNoGravCB;
    public Toggle CUNoMoveCB;
    public Toggle CUIsProbeCB;
    public GameObject CUCreateButton;
    public GameObject CUUpdateButton;

    #endregion

    //Activate
    private bool InfoActivated = false;
    private GameObject objectToUpdate;


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

        if (CreateOrUpdateValuePU.activeInHierarchy)
        {
            UpdatePalette();
        }
    }

    private void LoadPlanetInfo()
    {
        SpaceObject hledanaPlaneta = SpaceObject.GetChild(ObjektySimulace.transform, DropboxPlanetSelectPU.options[DropboxPlanetSelectPU.value].text).gameObject.GetComponent<SpaceObject>();
        InfoPosition.text = MenuManager.ParserVector3(hledanaPlaneta.transform.position * 100000, " km\n");
        InfoMass.text = hledanaPlaneta.mass.ToString();
        InfoRychlost.text = MenuManager.ParserVector3(hledanaPlaneta.rychlost, " km/s\n");
        InfoSilovePusobeni.text = MenuManager.ParserVector3(hledanaPlaneta.aktualniSilovePusobeni/hledanaPlaneta.mass, " N\n");
    }

    private void LoadPlanetSettings(bool loadFromDropbox)
    {
        if (loadFromDropbox)
        {
            SpaceObject hledanaPlaneta = objectToUpdate.GetComponent<SpaceObject>();
            CUNazevObjektu.text = hledanaPlaneta.name;
            CUMassObjektu.text = hledanaPlaneta.mass.ToString();
            CUPolohaX.text = hledanaPlaneta.transform.position.x.ToString();
            CUPolohaY.text = hledanaPlaneta.transform.position.y.ToString();
            CUPolohaZ.text = hledanaPlaneta.transform.position.z.ToString();
            CURychlostX.text = hledanaPlaneta.rychlost.x.ToString();
            CURychlostY.text = hledanaPlaneta.rychlost.y.ToString();
            CURychlostZ.text = hledanaPlaneta.rychlost.z.ToString();
            CURed.text = (hledanaPlaneta.GetComponent<MeshRenderer>().materials[0].color.r * 255).ToString();
            CUGreen.text = (hledanaPlaneta.GetComponent<MeshRenderer>().materials[0].color.g * 255).ToString();
            CUBlue.text = (hledanaPlaneta.GetComponent<MeshRenderer>().materials[0].color.b * 255).ToString();
            CUSize.text = hledanaPlaneta.transform.localScale.x.ToString();
            CUNoGravCB.isOn = hledanaPlaneta.noGravityEffect;
            CUNoMoveCB.isOn = hledanaPlaneta.noMovement;
            CUIsProbeCB.isOn = hledanaPlaneta.isProbe;
        }
        else 
        {
            CUNazevObjektu.text = "";
            CUMassObjektu.text = "";
            CUPolohaX.text = "";
            CUPolohaY.text = "";
            CUPolohaZ.text = "";
            CURychlostX.text = "";
            CURychlostY.text = "";
            CURychlostZ.text = "";
            CURed.text = "255";
            CUGreen.text = "255";
            CUBlue.text = "255";
            CUSize.text = "";
            CUNoGravCB.isOn = false;
            CUNoMoveCB.isOn = false;
            CUIsProbeCB.isOn = false;
        }
    }

    public void UpdatePalette()
    {
        if (!(CURed.text == "" || CUGreen.text == "" || CUBlue.text == ""))
        {
            CUColorPalette.color = new Color(ParserProOddelovace(CURed.text) / 255, ParserProOddelovace(CUGreen.text) / 255, ParserProOddelovace(CUBlue.text) / 255);
        }
    }

    public void OpenPlanetDeletePU()
    {
        SpeedPausePlay(true);
        CloseAllPopUps();
        UpdateDropboxDlePlanet(DropboxDestroyPlanetPU);
        DeletePU.SetActive(true);
    }

    public void OpenSelectPlanetUpdatePU()
    {
        SpeedPausePlay(true);
        CloseAllPopUps();
        UpdateDropboxDlePlanet(DropboxUpdatePlanetPU);
        SelectToUpdatePU.SetActive(true);
    }

    public void DeleteSelectedPlanet() 
    {
        SpaceObject hledanaPlaneta = SpaceObject.GetChild(ObjektySimulace.transform, DropboxDestroyPlanetPU.options[DropboxDestroyPlanetPU.value].text).gameObject.GetComponent<SpaceObject>();
        GameObject.Destroy(hledanaPlaneta.gameObject);
        CloseAllPopUps();
    }

    public void CreatePlanet() 
    {
        GameObject newObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        newObject.name = CUNazevObjektu.text;

        newObject.transform.parent = ObjektySimulace.transform;

        LoadSimulation.AddSpaceObject(newObject, GetSpaceObjectData());
        LoadSimulation.AddTrailRenderer(newObject);
        LoadSimulation.AddPopisek(newObject);
        LoadSimulation.AddParentSilocar(newObject);
    }

    public void SelectPlanetToUpdateAndOpenUpdate()
    {
        objectToUpdate = SpaceObject.GetChild(ObjektySimulace.transform, DropboxUpdatePlanetPU.options[DropboxUpdatePlanetPU.value].text).gameObject.GetComponent<SpaceObject>().gameObject;
        CloseAllPopUps();
        OpenPlanetCreateOrUpdatePU(false);
    }

    public void UpdatePlanet()
    {
        objectToUpdate.name = CUNazevObjektu.text;

        SpaceObjectData dataToLoad = GetSpaceObjectData();

        objectToUpdate.GetComponent<SpaceObject>().LoadFromData(dataToLoad);

        CloseAllPopUps();
    }

    private SpaceObjectData GetSpaceObjectData() 
    {
        SpaceObjectData spaceObjectDataInput = new SpaceObjectData();

        spaceObjectDataInput.name = CUNazevObjektu.text;
        spaceObjectDataInput.mass = ParserProOddelovace(CUMassObjektu.text);

        spaceObjectDataInput.position = new float[3];
        spaceObjectDataInput.position[0] = ParserProOddelovace(CUPolohaX.text);
        spaceObjectDataInput.position[1] = ParserProOddelovace(CUPolohaY.text);
        spaceObjectDataInput.position[2] = ParserProOddelovace(CUPolohaZ.text);

        spaceObjectDataInput.currentSpeed = new float[3];
        spaceObjectDataInput.currentSpeed[0] = ParserProOddelovace(CURychlostX.text);
        spaceObjectDataInput.currentSpeed[1] = ParserProOddelovace(CURychlostY.text);
        spaceObjectDataInput.currentSpeed[2] = ParserProOddelovace(CURychlostZ.text);

        spaceObjectDataInput.colour = new float[3];
        spaceObjectDataInput.colour[0] = ParserProOddelovace(CURed.text);
        spaceObjectDataInput.colour[1] = ParserProOddelovace(CUGreen.text);
        spaceObjectDataInput.colour[2] = ParserProOddelovace(CUBlue.text);

        spaceObjectDataInput.scale = new float[3];
        spaceObjectDataInput.scale[0] = ParserProOddelovace(CUSize.text);
        spaceObjectDataInput.scale[1] = ParserProOddelovace(CUSize.text);
        spaceObjectDataInput.scale[2] = ParserProOddelovace(CUSize.text);

        spaceObjectDataInput.noGravityEffect = CUNoGravCB.isOn;
        spaceObjectDataInput.noMovement = CUNoMoveCB.isOn;
        spaceObjectDataInput.isProbe = CUIsProbeCB.isOn;
        
        return spaceObjectDataInput;
    }

    public void OpenPlanetCreateOrUpdatePU(bool create)
    {
        SpeedPausePlay(true);
        CloseAllPopUps();
        CreateOrUpdateValuePU.SetActive(true);
        LoadPlanetSettings(!create);
        if (create) 
        {
            CreateTitle.enabled = true;
            CUCreateButton.SetActive(true);
            UpdateTitle.enabled = false;
            CUUpdateButton.SetActive(false);
        }
        else
        {
            CreateTitle.enabled = false;
            CUCreateButton.SetActive(false);
            UpdateTitle.enabled = true;
            CUUpdateButton.SetActive(true);
        }
    }

    public static string ParserVector3(Vector3 toConvert, string endlineSJednotkou) 
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
        dbToUpdate.value = 0;
        dbToUpdate.RefreshShownValue();
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
        UpdateDropboxDlePlanet(DropboxPlanetSelectPU);
        SelectPlanetPU.SetActive(true);
        SetUpdatingInfoPlanet(true);
    }

    public void OpenMoveToPU()
    {
        CloseAllPopUps();
        UpdateDropboxDlePlanet(DropboxMoveToPlanetPU);
        PlanetMoveToPU.SetActive(true);
    }

    public void OpenFollowPU()
    {
        CloseAllPopUps();
        UpdateDropboxDlePlanet(DropboxFollowPlanetPU);
        PlanetFollowPU.SetActive(true);
    }

    public void SetPoziceKameryToPlanet() 
    {
        Vector3 pozicePlanety = SpaceObject.GetChild(ObjektySimulace.transform, DropboxMoveToPlanetPU.options[DropboxMoveToPlanetPU.value].text).position;
        KameraASvetla.GetComponent<FullMoveCamScript>().Deactivate(); 
        KameraASvetla.GetComponent<FullMoveCamScript>().OffsetPos = pozicePlanety;
    }

    public void SetDefaultPozici()
    {
        Vector3 defaultPozice = new Vector3(0f, 0f, -350f);

        KameraASvetla.GetComponent<FullMoveCamScript>().Deactivate();
        KameraASvetla.GetComponent<FullMoveCamScript>().OffsetPos = defaultPozice;

        Quaternion defaultRotace = new Quaternion();
        defaultRotace.eulerAngles = new Vector3(0f, 0f, 0f);
        KameraASvetla.transform.rotation = defaultRotace;
        
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

    public void FollowPlanet()
    {
        GameObject objectToFollow = SpaceObject.GetChild(ObjektySimulace.transform, DropboxFollowPlanetPU.options[DropboxFollowPlanetPU.value].text).gameObject;
        KameraASvetla.GetComponent<FullMoveCamScript>().Activate(objectToFollow);
        KameraASvetla.GetComponent<FullMoveCamScript>().OffsetPos = new Vector3(0f, 0f, 0f);
        CloseAllPopUps();
    }

    private float ParserProOddelovace(string input) 
    {
        if(float.TryParse(input.Replace(',', '.'), out float teckaVal)) 
        {
            return teckaVal;
        }
        if (float.TryParse(input.Replace('.', ','), out float carkaVal)) 
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
        PlanetMoveToPU.SetActive(false);
        PlanetFollowPU.SetActive(false);
        DeletePU.SetActive(false);  
        SelectToUpdatePU.SetActive(false);
        CreateOrUpdateValuePU.SetActive(false);
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
