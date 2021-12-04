using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour
{
    // Start is called at the start of simulation
    void Start()
    {

        
    }

    void FixedUpdate()
    {
        TimeManager.CasNasobek a = GameObject.Find("TimeManager").GetComponent<TimeManager>().aktualniCasovyNasobek;
        for (int i = 0; i < ((int)a); i++)
        {
            //Debug.LogWarning(i);
            SpaceObject[] objekty = (SpaceObject[])Resources.FindObjectsOfTypeAll(typeof(SpaceObject));
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
        //Metoda vezme všechny vesmírné objekty a vytvoøí mezi nimi silová pùsobení

        foreach (SpaceObject sO in objekty)
        {
            sO.vsechnaSilovaPusobeni.Clear();
        }

        for (int i = 0; i < objekty.Length; i++)
        {
            for (int y = 0; y < objekty.Length; y++)
            {
                if (i != y) //a zároveò jsem již tuto gravitaci nespoèetl!!! DODAT
                {
                    Vector3[] silaMeziObjekty = GravityMethod(objekty[i], objekty[y]);

                    objekty[i].vsechnaSilovaPusobeni.Add(silaMeziObjekty[0]);
                    //objekty[y].vsechnaSilovaPusobeni.Add(silaMeziObjekty / objekty[y].weight);
                }
            }
        }
    }

    Vector3[] GravityMethod(SpaceObject sO, SpaceObject sO2)
    {

        const float gravityConstant = 6.67E-11f;

        Vector3 distanceInVector = (sO2.transform.position - sO.transform.position).normalized;
        float distance = Vector3.Distance(sO.transform.position, sO2.transform.position) * 1000000;

        //Debug.Log(distance / 1000000);
        if(distance < 10000000) 
        {
            distance = 10000000;
            Debug.LogWarning("NEBEZPEÈNÁ VZDÁLENOST");
        }

        float gravityNonVectorPart = (gravityConstant * sO.weight * sO2.weight / distance / distance)/1000000;


        Vector3 gravityVector0 = new Vector3(gravityNonVectorPart * distanceInVector.x / sO.weight, gravityNonVectorPart * distanceInVector.y / sO.weight, gravityNonVectorPart * distanceInVector.z / sO.weight);
        Vector3 gravityVector1 = new Vector3(gravityNonVectorPart * distanceInVector.x / sO2.weight, gravityNonVectorPart * distanceInVector.y / sO2.weight, gravityNonVectorPart * distanceInVector.z / sO2.weight);

        Vector3[] gravityVectors = { gravityVector0, gravityVector1 * -1 };

        return gravityVectors;
    }
}
