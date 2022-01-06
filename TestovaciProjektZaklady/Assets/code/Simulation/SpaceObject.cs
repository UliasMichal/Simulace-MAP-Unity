using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceObject : MonoBehaviour
{
    //SpaceObject je t��da, kterou m� ka�d� vesm�rn� objekt ovlivn�n gravitac�

    public float mass;
    public List<Vector3> vsechnaSilovaPusobeni;

    //ud�v�ny jako: UnityJednotka za 1s
    public Vector3 rychlost;

    public bool zobrazitSilocary;
    public bool zobrazitDrahy;

    public bool isProbe;

    public override string ToString()
    {
        return name + " " + mass;
    }

    #region DataRegion
    // Z�sk�v�n� dat a generace dat pro na��t�n�/ukl�d�n�

    public SpaceObjectData GetData()
    {
        return new SpaceObjectData(this);
    }

    public void LoadFromData(SpaceObjectData dataToLoad) 
    {
        this.name = dataToLoad.name;
        mass = dataToLoad.mass;

        zobrazitSilocary = dataToLoad.zobrazitSilocary;
        zobrazitDrahy = dataToLoad.zobrazitDrahy;

        isProbe = dataToLoad.isProbe;

        //Obarv� objekt dle ulo�en�ch RGB hodnot
        this.GetComponent<Renderer>().material.color = new Color(dataToLoad.colour[0], dataToLoad.colour[1], dataToLoad.colour[2]); 

        //Nastav� pozici dle ulo�en�ch sou�adnic XYZ
        this.transform.position = new Vector3(dataToLoad.position[0], dataToLoad.position[1], dataToLoad.position[2]);

        //Nastav� aktu�ln� rychlost dle ulo�en�ch sou�adnic XYZ
        this.rychlost  = new Vector3(dataToLoad.currentSpeed[0], dataToLoad.currentSpeed[1], dataToLoad.currentSpeed[2]);
    }
    #endregion

    public void OperaceObjektu()
    {
        //Metoda popisuj�c� pohybov� p�soben� objektu 
        Vector3 celkoveGravitaceZrychleni = CelkoveSilovePusobeniGravitace(vsechnaSilovaPusobeni);

        rychlost += celkoveGravitaceZrychleni;

        //Debug.LogWarning(rychlost);

        MoveBy(rychlost);
    }

    void MoveBy(Vector3 celkGravitace)
    {
        //Metoda pohne objektem dle gravita�n�ho p�soben� a vlastn� rychlosti
        Vector3 vysledniceSil = celkGravitace; 

        //TimeManager.CasNasobek a = GameObject.Find("TimeManager").GetComponent<TimeManager>().aktualniCasovyNasobek; //p�ipraveno a� bude t�eba �k�lovat dle vy���, �i ni��� frekvence

        this.transform.position += (vysledniceSil / 50);

        //Detekuje vzd�lenost mezi nejbli���mi vesm�rn�mi objekty a p��padn� dojde k jejich zni�en�
        //NicitelBlizkychObjektu(0.000001f);

    }

    void NicitelBlizkychObjektu(float distance) 
    {
        //Metoda zni�� objekt na ur�itou vzd�lenost (vzd�lenost v UnityJednotk�ch)
        foreach (SpaceObject sO in (SpaceObject[])Resources.FindObjectsOfTypeAll(typeof(SpaceObject)))
        {
            //if this != sO
            if (Mathf.Abs(sO.transform.position.magnitude - this.transform.position.magnitude) < distance && sO != this)
            {
                if (sO.mass < this.mass)
                {
                    Destroy(sO.gameObject);
                }
                if (sO.mass > this.mass)
                {
                    Destroy(this.gameObject);
                }
                if (sO.mass == this.mass)
                {
                    Destroy(sO.gameObject);
                    Destroy(this.gameObject);
                }
            }
        }
    }

    Vector3 CelkoveSilovePusobeniGravitace(List<Vector3> vektoryIn)
    {
        //Vypo��t� aktu�ln� gravita�n� p�soben� - se�te ve�ker� gravita�n� p�soben� do jednoho vektoru
        Vector3 vOut = new Vector3(0, 0, 0);

        foreach (Vector3 vIn in vektoryIn)
        {
            vOut += vIn;
        }

        return vOut; 
        //Pozn.: zde ji� m�me rychlost, proto�e tabulka je pro gravita�n� rychlost (kv�li d�len� hmotnost�)
    }
}
