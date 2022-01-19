using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject startMenuObj;
    public GameObject fileLoadObj;

    public GameObject popUpErrorObj;
    public Text textOfPopUpPath;

    public void StartByDefaultFile()
    {
        string path = Application.dataPath + "/LoadFileDefault.slf";

        StartByFile(path);
    }

    public void LoadFileSelect()
    {
        Debug.Log("File select");
        fileLoadObj.SetActive(true);
        startMenuObj.SetActive(false);
    }

    public void StartBySaveFile(int indexSouboru)
    {
        string path = Application.persistentDataPath + "/LoadFile" + indexSouboru + ".slf";

        StartByFile(path);
    }

    public void ExitApp() 
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void BackToStartMenu() 
    {
        fileLoadObj.SetActive(false);
        startMenuObj.SetActive(true);
    }
    public void HidePopUp()
    {
        popUpErrorObj.SetActive(false);
    }

    void StartByFile(string pathToFile)
    {
        //JsonUtility.FromJson(json, SimulationData);

        if (File.Exists(pathToFile))
        {

            StreamReader sr = new StreamReader(pathToFile);

            string json = sr.ReadToEnd();

            SimulationData dataFromFile = JsonUtility.FromJson<SimulationData>(json);

            sr.Close();

            TransferSimulationDataBetweenScenes.DataToTransfer = dataFromFile;
            TransferSimulationDataBetweenScenes.HasDataToTransfer = true;

            //C:\Users\Michal\AppData\LocalLow\DefaultCompany\TestovaciProjektZaklady\soubor.slf
            //Debug.Log(pathToFile);

            SceneManager.LoadScene(sceneName: "MainScene");
        }
        else
        {
            popUpErrorObj.SetActive(true);
            textOfPopUpPath.text = pathToFile;
        }
    }
}
