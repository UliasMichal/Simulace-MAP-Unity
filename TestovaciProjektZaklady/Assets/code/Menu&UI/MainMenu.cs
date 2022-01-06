using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject startMenuObj;
    public GameObject fileLoadObj;

    public void StartByApi()
    {
        //pøidat load by API
        Debug.Log("SUCCESS");
        //Unsuccessful
    }

    public void StartByDefaultFile()
    {
        string path = Application.persistentDataPath + "/LoadFile" + 1 + ".slf";

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
        Debug.Log("Save file");
    }

    public void ExitApp() 
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void BackToStartMenu() 
    {
        Debug.Log("Back");
        fileLoadObj.SetActive(false);
        startMenuObj.SetActive(true);
    }

    void StartByFile(string pathToFile)
    {
        //JsonUtility.FromJson(json, SimulationData);

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
}
