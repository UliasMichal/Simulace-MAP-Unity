using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceObject : MonoBehaviour
{
    //SpaceObject je tøída, kterou má každý vesmírný objekt ovlivnìn gravitací

    public float mass;
    public List<Vector3> vsechnaSilovaPusobeni;

    public Vector3 smerRychlostiObjektu;
    public float velikostRychlostiObjektu;

    public Vector3 rychlost; //zatím udávána jako: UnityJednotka za 1s

    public override string ToString()
    {
        return name + " " + mass;
    }

    void Start()
    {
        vsechnaSilovaPusobeni = new List<Vector3>();
        rychlost = new Vector3(0,0,0);


    }

    public void OperaceObjektu()
    {
        //Metoda popisující pohybové pùsobení objektu 
        Vector3 celkovaGravitaceZrychleni = CelkoveSilovePusobeniGravitace(vsechnaSilovaPusobeni);
        Vector3 vlastniRychlost = smerRychlostiObjektu.normalized * velikostRychlostiObjektu;


        rychlost += celkovaGravitaceZrychleni;

        
        MoveBy(rychlost, vlastniRychlost);
    }

    void MoveBy(Vector3 celkGravitace, Vector3 vlastniRychlost) 
    {
        //Metoda pohne objektem dle gravitaèního pùsobení a vlastní rychlosti
        Vector3 vysledniceSil = celkGravitace + vlastniRychlost;

        //TimeManager.CasNasobek a = GameObject.Find("TimeManager").GetComponent<TimeManager>().aktualniCasovyNasobek; //pøipraveno až bude tøeba škálovat dle vyšší, èi nižší frekvence
        
        vysledniceSil /= 50; //jelikož se síla vypoèítá za 1s musí být vydìlena 50 kvùli metodì FixedUpdate, která se volá 50x za s
        
        this.transform.position += vysledniceSil;

        //Detekuje vzdálenost mezi nejbližšími vesmírnými objekty a pøípadnì dojde k jejich znièení
        NicitelBlizkychObjektu(0.000001f);

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
