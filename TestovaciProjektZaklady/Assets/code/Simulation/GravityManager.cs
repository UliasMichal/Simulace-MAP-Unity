using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour
{
    // GravityManager spravuje gravita�n� v�po�ty 

    //Konstanty gravitace a vzd�lenost m���tka
    public const float gravityConstant = 6.67E-11f;
    public const float meritko = 1000000;

    bool canSimulate = true;

    

    void FixedUpdate()
    {
        //FixedUpdate je metoda volan� fixn�m �asem 
        if (TransferSimulationDataBetweenScenes.HasDataToTransfer)
        {
            LoadSimulation.LoadSimulationFromData(this.gameObject);
            return;
        }

        TimeManager.CasNasobek a = GameObject.Find("TimeManager").GetComponent<TimeManager>().aktualniCasovyNasobek;

        if (canSimulate && a != TimeManager.CasNasobek.pauza)
        {
            SpaceObject[] objekty = this.GetComponentsInChildren<SpaceObject>();

            GravityOfAllObjects(objekty);

            foreach (SpaceObject sO in objekty)
            {
                if (sO.vsechnaSilovaPusobeni.Count != 0)
                {
                    sO.OperaceObjektu();
                }
            }
        }
    }

    void GravityOfAllObjects(SpaceObject[] objekty)
    {
        //Metoda vezme v�echny vesm�rn� objekty a vytvo�� mezi nimi silov� p�soben�
        
        //Vyma�e aktu�ln� gravita�n� p�soben�
        foreach (SpaceObject sO in objekty)
        {
            sO.vsechnaSilovaPusobeni.Clear();
        }

        //Vypo��t� gravitaci mezi objekty a to p�i�ad� k objekt�m
        for (int i = 0; i < objekty.Length; i++)
        {
            for (int y = 0; y < objekty.Length; y++)
            {
                if (i < y) 
                {
                    Vector3[] silaMeziObjekty = GravityMethod(objekty[i], objekty[y]);

                    objekty[i].vsechnaSilovaPusobeni.Add(silaMeziObjekty[0]);
                    objekty[y].vsechnaSilovaPusobeni.Add(silaMeziObjekty[1]);
                }
            }
        }
    }
    Vector3[] GravityMethod(SpaceObject sO, SpaceObject sO2)
    {
        //Upraven� verze GravityMethod
        if (sO.noGravityEffect || sO2.noGravityEffect) 
        {
            Vector3[] zeroVectors = { new Vector3(0, 0, 0), new Vector3(0, 0, 0) };
            return zeroVectors;
        }

        float distance = Vector3.Distance(sO.transform.position, sO2.transform.position) * meritko;
        Vector3 direction = Vector3.Normalize(sO2.transform.position - sO.transform.position);

        float nonVectorPart = gravityConstant / distance / distance;

        //Gravita�n� zrychlen� upraveno d�len�m hmotnost� pro lep�� manipulaci (tato hmotnost by se stejn� d�lila p�i p�ed�v�n� s�li)
        Vector3 gravityVector1 = direction * nonVectorPart * sO2.mass / meritko;
        Vector3 gravityVector2 = direction * nonVectorPart * sO.mass / meritko;


        //Vektory jsou spojeny do 2 prvkov�ho pole - jedno m� opa�n� sm�r kv�li tomu, �e se ob� p�itahuj� k sob�
        Vector3[] gravityVectors = { gravityVector1, -1 * gravityVector2 };

        if (float.IsNaN(gravityVectors[0].x)) 
        {
            Debug.Log(sO.name);
            Debug.Log(sO2.name);
        }

        return gravityVectors;
    }
}
