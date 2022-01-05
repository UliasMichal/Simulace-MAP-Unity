using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour
{
    // GravityManager spravuje gravita�n� v�po�ty 

    //Konstanty gravitace a vzd�lenost m���tka
    public const float gravityConstant = 6.67E-11f;
    public const float meritko = 1000000;

    bool canSimulate = false;

    void Start()
    {
        if (TransferSimulationDataBetweenScenes.HasDataToTransfer)
        {
            List<SpaceObjectData> sodList = TransferSimulationDataBetweenScenes.DataToTransfer.Telesa;

            foreach (Transform child in this.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (SpaceObjectData sod in sodList)
            {
                GameObject newObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);

                newObject.transform.parent = this.transform;

                newObject.AddComponent<SpaceObject>();
                AddSpaceObject(newObject, sod);

                Debug.Log(newObject.name);
                Debug.Log(newObject.transform.position.x);
                Debug.Log(newObject.transform.position.y);
                Debug.Log(newObject.transform.position.z);
            }
        }
        canSimulate = true;
    }

    void AddSpaceObject(GameObject toAdd, SpaceObjectData sod) 
    {
        toAdd.name = sod.name;
        toAdd.transform.position = FloatArrToVector3(sod.position);
        toAdd.transform.localScale = FloatArrToVector3(sod.scale);

        toAdd.GetComponent<Renderer>().material.color = new Color(sod.colour[0], sod.colour[1], sod.colour[2]);
        toAdd.GetComponent<SpaceObject>().mass = sod.mass;
        toAdd.GetComponent<SpaceObject>().rychlost = FloatArrToVector3(sod.currentSpeed);
        toAdd.GetComponent<SpaceObject>().zakladniRychlostObjektu = FloatArrToVector3(sod.baseSpeed);
        toAdd.GetComponent<SpaceObject>().zobrazitSilocary = sod.zobrazitSilocary;
        toAdd.GetComponent<SpaceObject>().zobrazitDrahy = sod.zobrazitDrahy;
        toAdd.GetComponent<SpaceObject>().isProbe = sod.isProbe;

        toAdd.GetComponent<SpaceObject>().vsechnaSilovaPusobeni = new List<Vector3>();
    }

    Vector3 FloatArrToVector3(float[] vectorSaved) 
    {
        if(vectorSaved.Length != 3) 
        {
            Debug.LogError("VECTOR LOADED IN INVALID FORMAT - it needs to be 3 float array");
        }
        return new Vector3(vectorSaved[0], vectorSaved[1], vectorSaved[2]);
    }

    void FixedUpdate()
    {
        //FixedUpdate je metoda volan� fixn�m �asem - bude p�epracov�na na Update v r�mci dal��ho v�stupu pro lep�� v�kon syst�mu
        if (canSimulate)
        {
            TimeManager.CasNasobek a = GameObject.Find("TimeManager").GetComponent<TimeManager>().aktualniCasovyNasobek;
            for (int i = 0; i < ((int)a); i++)
            {
                SpaceObject[] objekty = this.GetComponentsInChildren<SpaceObject>();

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

        //Debug.Log(gravityVector2);

        //Vektory jsou spojeny do 2 prvkov�ho pole - jedno m� opa�n� sm�r kv�li tomu, �e se ob� p�itahuj� k sob�
        Vector3[] gravityVectors = { gravityVector1/50, -1 * gravityVector2 /50};

        return gravityVectors;
    }
}
