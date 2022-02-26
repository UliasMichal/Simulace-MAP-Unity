using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class MainMenu : MonoBehaviour
{
    public GameObject startMenuObj;
    public GameObject fileLoadObj;

    public GameObject popUpErrorObj;

    public Text textOfPopUpInfo;
    public Text textOfPopUpPath;

    private void FixedUpdate()
    {
        if (Screen.width < 450 || Screen.height < 400)
        {
            Screen.SetResolution(450, 400, false);
        }
    }

    public void StartByDefaultFile()
    {
        TextAsset file = Resources.Load<TextAsset>("Text/LoadFileDefault");

        StartByDefaultFileAsset(file);
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
        if (File.Exists(pathToFile))
        {
            using (StreamReader sr = new StreamReader(pathToFile))
            {
                string json = sr.ReadToEnd();

                SimulationData dataFromFile;
                TransferSimulationDataBetweenScenes.HasDataToTransfer = true;

                if (!json.Contains("\"Telesa\""))
                {
                    string errorMessageJSON = "Soubor nemá validní JSON strukturu: ";
                    textOfPopUpInfo.text = errorMessageJSON;
                    textOfPopUpPath.text = pathToFile;
                    popUpErrorObj.SetActive(true);
                    sr.Close();
                    return;
                }

                try
                {
                    dataFromFile = JsonUtility.FromJson<SimulationData>(json);
                    foreach (var so in dataFromFile.Telesa)
                    {
                        //Kontroluje validitu objektù - JsonUtility naèítá prázdné objekty, které by neodchycené dìlaly problémy
                        //Debug.Log se nevypíše po kompilaci, ale slouží to, aby otestoval, zda objekty nejsou prázdné odkazy
                        Debug.Log(so);
                    }
                }

                catch (Exception)
                {
                    string errorMessageJSON = "Soubor nemá validní JSON strukturu: ";
                    textOfPopUpInfo.text = errorMessageJSON;
                    textOfPopUpPath.text = pathToFile;
                    popUpErrorObj.SetActive(true);
                    sr.Close();
                    return;
                }

                sr.Close();

                TransferSimulationDataBetweenScenes.DataToTransfer = dataFromFile;

                SceneManager.LoadScene(sceneName: "MainScene");
            }
        }
        else
        {
            string errorMessagePath = "Cesta k souboru není validní: ";
            textOfPopUpInfo.text = errorMessagePath;
            textOfPopUpPath.text = pathToFile;
            popUpErrorObj.SetActive(true);
        }
    }

    void StartByDefaultFileAsset(TextAsset defaultFile)
    {
        string json = defaultFile.text;
        Debug.Log(defaultFile.text);

        SimulationData dataFromFile;
        TransferSimulationDataBetweenScenes.HasDataToTransfer = true;
        dataFromFile = JsonUtility.FromJson<SimulationData>(json);


        TransferSimulationDataBetweenScenes.DataToTransfer = dataFromFile;

        SceneManager.LoadScene(sceneName: "MainScene");
    }
}
