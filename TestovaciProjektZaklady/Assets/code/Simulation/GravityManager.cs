using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour
{
    // GravityManager spravuje gravita�n� v�po�ty 

    //Konstanty gravitace a vzd�lenost m���tka
    public const float gravityConstant = 6.67E-11f;
    public const float meritko = 1000000;

    void FixedUpdate()
    {
        //FixedUpdate je metoda volan� fixn�m �asem - bude p�epracov�na na Update v r�mci dal��ho v�stupu pro lep�� v�kon syst�mu

        TimeManager.CasNasobek a = GameObject.Find("TimeManager").GetComponent<TimeManager>().aktualniCasovyNasobek;
        for (int i = 0; i < ((int)a); i++)
        {
            //Debug.LogWarning(i);
            SpaceObject[] objekty = (SpaceObject[])Resources.FindObjectsOfTypeAll(typeof(SpaceObject));
            GravityOfAllObjects(objekty);
            
            foreach (SpaceObject sO in objekty)
            {
                if (sO.vsechnaSilovaPusobeni.Count != 0 && sO.name != "Sun")
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

    Vector3[] GravityMethodOld(SpaceObject sO, SpaceObject sO2)
    {
        //Metoda po��taj�c� gravita�n� p�soben� mezi 2 objekty - vrac� 2-prvkov� pole 
        //Z d�vodu velikosti je vektorov� a nevektorov� ��st rozd�lena

        //Vektorov� ��st - normalizovan� vektor ukazuj�c� sm�r mezi 2 objekty
        Vector3 distanceInVector = (sO2.transform.position - sO.transform.position).normalized;
        

        float distance = Vector3.Distance(sO.transform.position, sO2.transform.position) * meritko;

        //Debug.Log(distance / 1000000);
        
        if(distance < meritko * 10) 
        {
            distance = meritko * 10;
            Debug.LogWarning("NEBEZPE�N� VZD�LENOST");
        }

        //Nevekterov� ��st - pomoc� Newtonova gravita�n�ho z�konu 
        float gravityNonVectorPart = (gravityConstant * sO.mass * sO2.mass / distance / distance)/ meritko;

        //Gravita�n� zrychlen� upraveno d�len�m hmotnost� pro lep�� manipulaci (tato hmotnost by se stejn� d�lila p�i p�ed�v�n� s�li)
        Vector3 gravityVector0 = new Vector3(gravityNonVectorPart * distanceInVector.x / sO.mass, gravityNonVectorPart * distanceInVector.y / sO.mass, gravityNonVectorPart * distanceInVector.z / sO.mass);
        Vector3 gravityVector1 = new Vector3(gravityNonVectorPart * distanceInVector.x / sO2.mass, gravityNonVectorPart * distanceInVector.y / sO2.mass, gravityNonVectorPart * distanceInVector.z / sO2.mass);


       //Vektory jsou spojeny do 2 prvkov�ho pole - jedno m� opa�n� sm�r kv�li tomu, �e se ob� p�itahuj� k sob�
       Vector3[] gravityVectors = { gravityVector0, gravityVector1 * -1 };

        return gravityVectors;
    }

    Vector3[] GravityMethod(SpaceObject sO, SpaceObject sO2)
    {
        //Upraven� verze GravityMethod - dle doporu�en� konzultanta
        
        float rx = (sO2.transform.position.x - sO.transform.position.x) * meritko;
        float ry = (sO2.transform.position.y - sO.transform.position.y) * meritko;
        float rz = (sO2.transform.position.y - sO.transform.position.z) * meritko;
        


        float distance = Vector3.Distance(sO.transform.position, sO2.transform.position) * meritko;

        //Debug.Log(distance);


        float axMassObou = ((gravityConstant * rx * sO.mass * sO2.mass) / Mathf.Pow(distance, 3));
        float ayMassObou = ((gravityConstant * ry * sO.mass * sO2.mass) / Mathf.Pow(distance, 3));
        float azMassObou = ((gravityConstant * rz * sO.mass * sO2.mass) / Mathf.Pow(distance, 3));



        //Gravita�n� zrychlen� upraveno d�len�m hmotnost� pro lep�� manipulaci (tato hmotnost by se stejn� d�lila p�i p�ed�v�n� s�li)
        Vector3 gravityVector1 = new Vector3(axMassObou / sO.mass, ayMassObou / sO.mass, azMassObou / sO.mass) / meritko / 50;
        Vector3 gravityVector2 = new Vector3(axMassObou / sO2.mass, ayMassObou / sO2.mass, azMassObou / sO2.mass) / meritko / 50;



        //Vektory jsou spojeny do 2 prvkov�ho pole - jedno m� opa�n� sm�r kv�li tomu, �e se ob� p�itahuj� k sob�
        Vector3[] gravityVectors = { gravityVector1/50, -1 * gravityVector2 /50};

        return gravityVectors;
    }
}
