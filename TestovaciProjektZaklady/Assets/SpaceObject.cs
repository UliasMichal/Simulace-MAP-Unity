using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceObject : MonoBehaviour
{
    //SpaceObject je t��da, kterou m� ka�d� vesm�rn� objekt ovlivn�n gravitac�

    public float mass;
    public List<Vector3> vsechnaSilovaPusobeni;

    public Vector3 smerRychlostiObjektu;
    public float velikostRychlostiObjektu;

    public Vector3 rychlost; //zat�m ud�v�na jako: UnityJednotka za 1s

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
        //Metoda popisuj�c� pohybov� p�soben� objektu 
        Vector3 celkovaGravitaceZrychleni = CelkoveSilovePusobeniGravitace(vsechnaSilovaPusobeni);
        Vector3 vlastniRychlost = smerRychlostiObjektu.normalized * velikostRychlostiObjektu;


        rychlost += celkovaGravitaceZrychleni;

        
        MoveBy(rychlost, vlastniRychlost);
    }

    void MoveBy(Vector3 celkGravitace, Vector3 vlastniRychlost) 
    {
        //Metoda pohne objektem dle gravita�n�ho p�soben� a vlastn� rychlosti
        Vector3 vysledniceSil = celkGravitace + vlastniRychlost;

        //TimeManager.CasNasobek a = GameObject.Find("TimeManager").GetComponent<TimeManager>().aktualniCasovyNasobek; //p�ipraveno a� bude t�eba �k�lovat dle vy���, �i ni��� frekvence
        
        vysledniceSil /= 50; //jeliko� se s�la vypo��t� za 1s mus� b�t vyd�lena 50 kv�li metod� FixedUpdate, kter� se vol� 50x za s
        
        this.transform.position += vysledniceSil;

        //Detekuje vzd�lenost mezi nejbli���mi vesm�rn�mi objekty a p��padn� dojde k jejich zni�en�
        NicitelBlizkychObjektu(0.000001f);

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
