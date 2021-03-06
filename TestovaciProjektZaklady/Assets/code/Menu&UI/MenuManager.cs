using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    //Ovl?dac? objekty
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
    public GameObject TextProbe;
    public GameObject OvladaniProbe;
    public Dropdown DropboxSelectProbe;
    public InputField MotorX;
    public InputField MotorY;
    public InputField MotorZ;
    public Toggle MotorXCB;
    public Toggle MotorYCB;
    public Toggle MotorZCB;
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

    //Vlastnosti pop-up?
    #region Vlastnosti pop-up?
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
    public Scrollbar ScrollbarCU;
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
        ScrollbarCU.value = 1;
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
        float mass = ParserProOddelovace(CUMassObjektu.text);

        if (float.IsNaN(mass)) 
        {
            OpenErrorPU("Hodnota hmotnosti mus? b?t validn? ??slo");
            return;
        }

        if (DoesItCauseNameConflict(CUNazevObjektu.text)) 
        {
            OpenErrorPU("N?zev mus? b?t unik?tn? - pouze jeden objekt m??e m?t dan? n?zev");
        }


        GameObject newObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        newObject.name = CUNazevObjektu.text;

        newObject.transform.parent = ObjektySimulace.transform;

        LoadSimulation.AddSpaceObject(newObject, GetSpaceObjectData());
        LoadSimulation.AddTrailRenderer(newObject);
        LoadSimulation.AddPopisek(newObject);
        LoadSimulation.AddParentSilocar(newObject);
        CloseAllPopUps();
    }

    public bool DoesItCauseNameConflict(string nameToCheck, bool isUpdate=false) 
    {
        foreach (Transform simulationObject in ObjektySimulace.transform)
        {
            if(simulationObject.name == nameToCheck) 
            {
                if (!isUpdate)
                {
                    return true;
                }
                else 
                {
                    isUpdate = false;
                }
            }
        }
        return false;
    }

    public void SelectPlanetToUpdateAndOpenUpdate()
    {
        objectToUpdate = SpaceObject.GetChild(ObjektySimulace.transform, DropboxUpdatePlanetPU.options[DropboxUpdatePlanetPU.value].text).gameObject.GetComponent<SpaceObject>().gameObject;
        CloseAllPopUps();
        OpenPlanetCreateOrUpdatePU(false);
    }

    public void UpdatePlanet()
    {
        float mass = ParserProOddelovace(CUMassObjektu.text);
        if (float.IsNaN(mass) || mass <= 0)
        {
            OpenErrorPU("Hodnota hmotnosti mus? b?t validn? ??slo\nV intervalu (0; 3.4E+38)");
            return;
        }

        if (DoesItCauseNameConflict(CUNazevObjektu.text))
        {
            OpenErrorPU("N?zev mus? b?t unik?tn? - pouze jeden objekt m??e m?t dan? n?zev");
        }

        objectToUpdate.name = CUNazevObjektu.text;

        SpaceObjectData dataToLoad = GetSpaceObjectData();

        objectToUpdate.GetComponent<SpaceObject>().LoadFromData(dataToLoad);

        CloseAllPopUps();
    }

    public void UpdateProbe() 
    {
        ProbeMotory pmToUpdate = SpaceObject.GetChild(ObjektySimulace.transform, DropboxSelectProbe.options[DropboxSelectProbe.value].text).gameObject.GetComponent<ProbeMotory>();
        UpdateProbeValues(pmToUpdate);
    }

    private void UpdateProbeValues(ProbeMotory toUpdate) 
    {
        toUpdate.Motory.x = ParserProOddelovace(MotorX.text);
        toUpdate.Motory.y = ParserProOddelovace(MotorY.text);
        toUpdate.Motory.z = ParserProOddelovace(MotorZ.text);

        toUpdate.MotorX = MotorXCB.isOn;
        toUpdate.MotorY = MotorYCB.isOn;
        toUpdate.MotorZ = MotorZCB.isOn;
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
        //Unity v k?du zpracov?v? RGB ve form?tu floatu od 0.0 do 1.0
        spaceObjectDataInput.colour[0] = ParserProOddelovace(CURed.text)/255;
        spaceObjectDataInput.colour[1] = ParserProOddelovace(CUGreen.text)/255;
        spaceObjectDataInput.colour[2] = ParserProOddelovace(CUBlue.text)/255;

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
        SpaceObject hledanaPlaneta = SpaceObject.GetChild(ObjektySimulace.transform, DropboxPlanetOptionsPU.options[DropboxPlanetOptionsPU.value].text).gameObject.GetComponent<SpaceObject>();

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

    public void UpdateProbeCP()
    {
        DropboxSelectProbe.options.Clear();
        foreach (Transform t in ObjektySimulace.transform)
        {
            if (t.GetComponent<SpaceObject>().isProbe)
            {
                DropboxSelectProbe.options.Add(new Dropdown.OptionData() { text = t.name.ToString() });
            }
        }
        
        TextProbe.SetActive(DropboxSelectProbe.options.Count == 0);
        OvladaniProbe.SetActive(DropboxSelectProbe.options.Count != 0);
        if (DropboxSelectProbe.options.Count != 0)
        {
            DropboxSelectProbe.value = 0;
            DropboxSelectProbe.RefreshShownValue();
            LoadProbeValues();
        }
    }

    public void LoadProbeValues() 
    {
        ProbeMotory hledanaProbe = SpaceObject.GetChild(ObjektySimulace.transform, DropboxSelectProbe.options[DropboxSelectProbe.value].text).gameObject.GetComponent<ProbeMotory>();

        MotorX.text = hledanaProbe.Motory.x.ToString();
        MotorY.text = hledanaProbe.Motory.y.ToString();
        MotorZ.text = hledanaProbe.Motory.z.ToString();

        MotorXCB.isOn = hledanaProbe.MotorX;
        MotorYCB.isOn = hledanaProbe.MotorY;
        MotorZCB.isOn = hledanaProbe.MotorZ;
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
        Vector3 aktualniOffset = KameraASvetla.GetComponent<FullMoveCamScript>().OffsetPos;
        float xPos = ParserProOddelovace(PoziceX.text);
        if (xPos == float.NaN) 
        {
            OpenErrorPU(xPos + " - nen? validn? ??slo pro pozici");
        }
        if(PoziceX.text == "") 
        {
            xPos = aktualniOffset.x;
        }

        float yPos = ParserProOddelovace(PoziceY.text);
        if (yPos == float.NaN)
        {
            OpenErrorPU(yPos + " - nen? validn? ??slo pro pozici");
        }
        if (PoziceY.text == "")
        {
            yPos = aktualniOffset.y;
        }

        float zPos = ParserProOddelovace(PoziceZ.text);
        if (zPos == float.NaN)
        {
            OpenErrorPU(zPos + " - nen? validn? ??slo pro pozici");
        }
        if (PoziceZ.text == "")
        {
            zPos = aktualniOffset.z;
        }

        if(RotaceX.text == "" || RotaceX.text == "" || RotaceX.text == "") 
        {
            OpenErrorPU("Rotaci lze zm?nit pouze pro v?echny 3 osy. Vypln?te v?echna pole rotace.");
        }

        float xRot = ParserProOddelovace(RotaceX.text);
        if (xRot == float.NaN)
        {
            OpenErrorPU(xRot + " - nen? validn? ??slo pro rotaci");
        }

        float yRot = ParserProOddelovace(RotaceY.text);
        if (yRot == float.NaN)
        {
            OpenErrorPU(yRot + " - nen? validn? ??slo pro rotaci");
        }

        float zRot = ParserProOddelovace(RotaceZ.text);
        if (zRot == float.NaN)
        {
            OpenErrorPU(zRot + " - nen? validn? ??slo pro rotaci");
        }
        

        KameraASvetla.GetComponent<FullMoveCamScript>().Deactivate();
        KameraASvetla.GetComponent<FullMoveCamScript>().OffsetPos = new Vector3(xPos, yPos, zPos);

        KameraASvetla.transform.localRotation = Quaternion.Euler(new Vector3(xRot, yRot, zRot));
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
            UpdateProbeCP();
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
