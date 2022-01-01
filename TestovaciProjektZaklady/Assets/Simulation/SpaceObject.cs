using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceObject : MonoBehaviour
{
    //SpaceObject je tøída, kterou má každý vesmírný objekt ovlivnìn gravitací

    public float mass;
    public List<Vector3> vsechnaSilovaPusobeni;

    public Vector3 zakladniRychlostObjektu;

    //udávány jako: UnityJednotka za 1s
    public Vector3 rychlost;

    public bool isProbe;

    public override string ToString()
    {
        return name + " " + mass;
    }

    
    void Start()
    {
        // Bude smazáno - debug only
        
        vsechnaSilovaPusobeni = new List<Vector3>();
        rychlost = new Vector3(0, 0, 0);

        Color c = new Color(1, 1, 0);
        this.GetComponent<Renderer>().material.color = c;
    }

    #region DataRegion
    // Získávání dat a generace dat pro naèítání/ukládání

    public SpaceObjectData GetData()
    {
        return new SpaceObjectData(this);
    }

    public void LoadFromData(SpaceObjectData dataToLoad) 
    {
        this.name = dataToLoad.name;
        mass = dataToLoad.mass;
        isProbe = dataToLoad.isProbe;

        //Obarví objekt dle uložených RGB hodnot
        this.GetComponent<Renderer>().material.color = new Color(dataToLoad.colour[0], dataToLoad.colour[1], dataToLoad.colour[2]); 

        //Nastaví pozici dle uložených souøadnic XYZ
        this.transform.position = new Vector3(dataToLoad.position[0], dataToLoad.position[1], dataToLoad.position[2]);

        //Nastaví základní rychlost dle uložených souøadnic XYZ
        this.rychlost = new Vector3(dataToLoad.baseSpeed[0], dataToLoad.baseSpeed[1], dataToLoad.baseSpeed[2]);

        //Nastaví aktuální rychlost dle uložených souøadnic XYZ
        this.rychlost  = new Vector3(dataToLoad.currentSpeed[0], dataToLoad.currentSpeed[1], dataToLoad.currentSpeed[2]);
    }
    #endregion

    public void OperaceObjektu()
    {
        //Metoda popisující pohybové pùsobení objektu 
        Vector3 celkoveGravitaceZrychleni = CelkoveSilovePusobeniGravitace(vsechnaSilovaPusobeni);

        Debug.Log(rychlost.x);
        //Debug.Log(vlastniRychlost.x);

        //Debug.Log(celkoveGravitaceZrychleni.x);

        rychlost += celkoveGravitaceZrychleni;


        MoveBy(rychlost, zakladniRychlostObjektu);
    }

    void MoveBy(Vector3 celkGravitace, Vector3 vlastniRychlost)
    {
        //Metoda pohne objektem dle gravitaèního pùsobení a vlastní rychlosti
        Vector3 vysledniceSil = celkGravitace + vlastniRychlost; 

        //TimeManager.CasNasobek a = GameObject.Find("TimeManager").GetComponent<TimeManager>().aktualniCasovyNasobek; //pøipraveno až bude tøeba škálovat dle vyšší, èi nižší frekvence

        this.transform.position += (vysledniceSil / 50);

        //Detekuje vzdálenost mezi nejbližšími vesmírnými objekty a pøípadnì dojde k jejich znièení
        //NicitelBlizkychObjektu(0.000001f);

    }

    void NicitelBlizkychObjektu(float distance) 
    {
        //Metoda znièí objekt na urèitou vzdálenost (vzdálenost v UnityJednotkách)
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
        //Vypoèítá aktuální gravitaèní pùsobení - seète veškeré gravitaèní pùsobení do jednoho vektoru
        Vector3 vOut = new Vector3(0, 0, 0);

        foreach (Vector3 vIn in vektoryIn)
        {
            vOut += vIn;
        }

        return vOut; 
        //Pozn.: zde již máme rychlost, protože tabulka je pro gravitaèní rychlost (kvùli dìlení hmotností)
    }
}
