using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject startMenuObj;
    public GameObject fileLoadObj;

    public void StartByApi()
    {
        //pøidat load by API
        Debug.Log("SUCCESS");
    }

    public void StartByDefaultFile()
    {
        Debug.Log("Default file");
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
        //Metoda, která naète ze souboru
    }
}
