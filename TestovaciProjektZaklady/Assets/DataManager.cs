using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class DataManager : MonoBehaviour
{
    public string urlToRequest = $"https://ssd.jpl.nasa.gov/api/horizons.api?format=json&COMMAND=%27#OBJEKTTOREPLACE#%27&OBJ_DATA=%27YES%27&MAKE_EPHEM=%27YES%27&EPHEM_TYPE=%27VECTOR%27&CENTER=%27500@10%27&START_TIME=%272006-01-01%27&STOP_TIME=%272006-01-20%27&STEP_SIZE=%271%20d%27&QUANTITIES=%271,9,20,23,24,29%27";

    public enum FileSelectMode 
    {
        defaultFile,
        saveFile1,
        saveFile2,
        saveFile3,
    }


    async Task Start()
    {
        /*
        List<int> objekty = GetIDsObjektu();

        
        List<int> objekty = new List<int>();
        objekty.Add(10);
        objekty.Add(199);

        //int i = 0;
        foreach (int objektID in objekty)
        {

            string urlToRequestNow = urlToRequest.Replace($"#OBJEKTTOREPLACE#", objektID.ToString());
            await RequestNaApi(urlToRequestNow);
            //Debug.Log(i++);
            System.Threading.Thread.Sleep(10000);

        }
        */
    }

    static async Task RequestNaApi(string urlToRequest)
    {
        //Výsledek tohoto testu bylo, že API má neuvìøitelnì chaotické seøazení dat - i ve formátu JSON je formát OBJ_Data prakticky náhodný (úplnì jiná pozice stejných hodnot a nìkdy hodnoty mají i rùzné názvy).


        //string urlToRequest = $"https://ssd.jpl.nasa.gov/api/horizons.api?format=json&COMMAND=%27499%27&OBJ_DATA=%27YES%27&MAKE_EPHEM=%27YES%27&EPHEM_TYPE=%27OBSERVER%27&CENTER=%27500@399%27&START_TIME=%272006-01-01%27&STOP_TIME=%272006-01-20%27&STEP_SIZE=%271%20d%27&QUANTITIES=%271,9,20,23,24,29%27";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(urlToRequest))
        {
            webRequest.SendWebRequest();
            while (!webRequest.isDone)
            {
                await Task.Yield();
            }


            string responseBody = webRequest.downloadHandler.data.ToString();
            Debug.Log(responseBody);
            responseBody = Regex.Replace(responseBody, @"\s+", " ");
            string[] splitters = { "*******************************************************************************", "*******************************************" };
            string[] parts = responseBody.Split(splitters, StringSplitOptions.RemoveEmptyEntries);

            int indexMass = parts[1].IndexOf("Mass");
            int indexKonce = parts[1].IndexOf(' ', parts[1].IndexOf("= ", parts[1].IndexOf("Mass")) + 2);
            StringBuilder sb = new StringBuilder();
            for(indexMass = indexMass; indexMass < indexKonce; indexMass++) 
            {
                sb.Append(parts[1][indexMass]);
            }

            string[] separator = {"\\n"};


            string mass = sb.ToString(); //hmotnost objektu - formát se liší dle tìlesa
            string[] xyzPozice = parts[8].Split(separator, StringSplitOptions.RemoveEmptyEntries); //pozice X,Y,Z - poèet dle requestù

            /*
            foreach (string part in parts)
            {
                //Debug.Log("Sekce " + (Array.IndexOf(parts, part) + 1));
                string[] linesOfPart = part.Split("\\n", StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in linesOfPart)
                {
                    Debug.Log(line);
                }
            }*/

            //NÌJAK VRA TEN STRING!!!
        }
    }
    

    static List<int> GetIDsObjektu()
    {
        //Vrátí list základních objektù v simulaci
        int[] zakladni = { 10, 199, 299, 399, 499, 599, 699, 799, 899, 301, 401, 402, 901, 902, 903, 904, 905 }; // + 501-572 + 55501-55508 + 601-666 + 65067 + 65070 + 65077 + 65079 + 65081 + 65082 + 65084-65093 + 701-727 + 801-814
        List<int> veci = new List<int>(zakladni);

        for (int i = 501; i <= 572; i++)
        {
            veci.Add(i);
        }
        for (int i = 55501; i <= 55508; i++)
        {
            veci.Add(i);
        }
        for (int i = 601; i <= 666; i++)
        {
            veci.Add(i);
        }
        for (int i = 65084; i <= 65093; i++)
        {
            veci.Add(i);
        }
        for (int i = 701; i <= 727; i++)
        {
            veci.Add(i);
        }
        for (int i = 801; i <= 814; i++)
        {
            veci.Add(i);
        }

        int[] dodatky = { 65067, 65070, 65077, 65079, 65081, 65082 };
        veci.AddRange(dodatky);
        return veci;
    }
    
    public void MakeOutputFile() 
    {

    }

    public void LoadFromFile() 
    {

    }
}
