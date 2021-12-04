using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceObject : MonoBehaviour
{
    public float weight;
    public List<Vector3> vsechnaSilovaPusobeni;

    public Vector3 smerRychlostiObjektu;
    public float velikostRychlostiObjektu;

    public Vector3 rychlost; //UnityJednotka za 1s

    public override string ToString()
    {
        return name + " " + weight;
    }

    // Start is called before the first frame update
    void Start()
    {
        vsechnaSilovaPusobeni = new List<Vector3>();
        //smerRychlostiObjektu = smerRychlostiObjektu.normalized;
        rychlost = new Vector3(0,0,0);


    }

    public void OperaceObjektu()
    {
        Vector3 celkovaGravitaceZrychleni = CelkoveSilovePusobeniGravitace(vsechnaSilovaPusobeni);
        //ToDo: dát pravou hodnotu do objectBaseVelocity 
        Vector3 vlastni = smerRychlostiObjektu.normalized * velikostRychlostiObjektu;
        rychlost += celkovaGravitaceZrychleni;

        if (this.name != "Sun")
        {
            MoveBy(rychlost, vlastni);
        }
        //rychlost = new Vector3(0, 0, 0);

        if (this.name == "Mercury")
        {
            //Debug.Log(celkovaGravitaceZrychleni.magnitude);
            Debug.Log(celkovaGravitaceZrychleni.magnitude);
            if (this.transform.position == new Vector3(12.75359f, 19.04679f, 1.010269f))
            {
                Debug.LogWarning("Same position!");
            }
        }
    }

    void MoveBy(Vector3 celkGravitace, Vector3 vlastni) 
    {
        Vector3 vysledniceSil = celkGravitace + vlastni;

        //Debug.LogWarning(vysledniceSil);

        //TimeManager.CasNasobek a = GameObject.Find("TimeManager").GetComponent<TimeManager>().aktualniCasovyNasobek;
        vysledniceSil /= 50;
        //Vector3 novaPozice = new Vector3(this.transform.position.x + vysledniceSil.x, this.transform.position.y + vysledniceSil.y, this.transform.position.z + vysledniceSil.z);
        //Vector3 novaPozice = this.transform.position + vysledniceSil;
        this.transform.position += vysledniceSil;

        //NicitelBlizkychObjektu(0.5f);

    }

    void NicitelBlizkychObjektu(float distance) 
    {

        foreach (SpaceObject sO in (SpaceObject[])Resources.FindObjectsOfTypeAll(typeof(SpaceObject)))
        {
            //if this != sO
            if (Mathf.Abs(sO.transform.position.magnitude - this.transform.position.magnitude) < distance && sO != this)
            {
                if (sO.weight < this.weight)
                {
                    Destroy(sO);
                }
                if (sO.weight > this.weight)
                {
                    Destroy(this);
                }
                if (sO.weight == this.weight)
                {
                    Destroy(sO);
                    Destroy(this);
                }
            }
        }
    }

    Vector3 UpravaSmeruOrbity(Vector3 smerOrbity, Vector3 celkoveGravitacniPusobeni) 
    {
        /*
        Debug.Log(smerOrbity.x);
        Debug.Log(smerOrbity.y);
        Debug.Log(celkoveGravitacniPusobeni.x);
        Debug.Log(celkoveGravitacniPusobeni.y);
        */

        Vector3 a = Vector3.Cross(smerOrbity, celkoveGravitacniPusobeni);
        return a.normalized;
    }

    Vector3 CelkoveSilovePusobeniGravitace(List<Vector3> vektoryIn)
    {
        Vector3 vOut = new Vector3(0, 0, 0);

        foreach (Vector3 vIn in vektoryIn)
        {
            vOut += vIn;
        }
        
        if(vOut.magnitude > 0.5f) 
        {
            //vOut = vOut.normalized * 0.5f;
            //Debug.LogWarning("pøekroèeno");
        }

        return vOut; //zde máme akceleraci nebo rychlost za 1s
    }
}
