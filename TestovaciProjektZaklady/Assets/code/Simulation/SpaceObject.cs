using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceObject : MonoBehaviour
{
    //SpaceObject je tøída, kterou má každý vesmírný objekt ovlivnìn gravitací

    public float mass;
    public List<Vector3> vsechnaSilovaPusobeni;

    //udávány jako: UnityJednotka za 1s
    public Vector3 rychlost;
    public Vector3 aktualniSilovePusobeni;

    public bool zobrazitSilocary;
    public bool zobrazitDrahy;
    public bool zobrazitPopisek = false;

    public bool isProbe;

    public bool noGravityEffect = false;
    public bool noMovement = false;

    public override string ToString()
    {
        return name + " " + mass;
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

        zobrazitSilocary = dataToLoad.zobrazitSilocary;
        zobrazitDrahy = dataToLoad.zobrazitDrahy;

        isProbe = dataToLoad.isProbe;

        noGravityEffect = dataToLoad.noGravityEffect;
        noMovement = dataToLoad.noMovement;

        //Pøidá materiál typu planeta a obarví objekt dle uložených RGB hodnot
        Material planetMat = Resources.Load<Material>("Materials/Planet");
        Material[] matArr = { planetMat };

        Color planetColor = new Color(dataToLoad.colour[0], dataToLoad.colour[1], dataToLoad.colour[2]);
        this.GetComponent<MeshRenderer>().materials = matArr;

        this.GetComponent<MeshRenderer>().materials[0].color = planetColor;
        
        //Nastaví pozici dle uložených souøadnic XYZ
        this.transform.position = new Vector3(dataToLoad.position[0], dataToLoad.position[1], dataToLoad.position[2]);

        //Nastaví aktuální rychlost dle uložených souøadnic XYZ
        this.rychlost  = new Vector3(dataToLoad.currentSpeed[0], dataToLoad.currentSpeed[1], dataToLoad.currentSpeed[2]);
    }
    #endregion

    public void OperaceObjektu()
    {
        //Metoda popisující pohybové pùsobení objektu 
        aktualniSilovePusobeni = CelkoveSilovePusobeniGravitace(vsechnaSilovaPusobeni);

        if (!noMovement)
        {
            rychlost += aktualniSilovePusobeni;
            MoveBy(rychlost); 
        }

        //OvladaniTrailRendereru();
        //OvladaniPopisku();
        //OvladaniLineRendereru();
    }

    void OvladaniTrailRendereru() 
    {
        //Metoda umožòuje skrýt/odkrýt dráhu planety

        if (zobrazitDrahy)
        {
            Material defaultLine = Resources.Load<Material>("Materials/Default-Line");
            Color toChange = this.GetComponent<MeshRenderer>().materials[0].color;
            toChange.a = 1;
            Material[] matArr = { defaultLine };
            this.GetComponent<TrailRenderer>().materials = matArr;
            this.GetComponent<TrailRenderer>().materials[0] = defaultLine;
            this.GetComponent<TrailRenderer>().material.SetColor("_TintColor", toChange);
        }
        else
        {
            Material invis = Resources.Load<Material>("Materials/Invis");
            Material[] matArr = { invis };
            this.GetComponent<TrailRenderer>().materials = matArr;
        }
        
    }

    void OvladaniPopisku()
    {
        //Metoda umožòuje skrýt/odkrýt název planety
        
        TextMesh popisekObjektu = this.GetComponentInChildren<TextMesh>();
        if (zobrazitPopisek) 
        {
            popisekObjektu.fontSize = (int)(GameObject.Find("CameraAndLights").transform.position.magnitude / 7);
            popisekObjektu.transform.rotation = GameObject.Find("CameraAndLights").transform.rotation;
            popisekObjektu.transform.position = this.transform.position;
            popisekObjektu.text = this.name;
        }
        else 
        {
            popisekObjektu.text = "";
        }
    }

    void OvladaniLineRendereru() 
    {
        //Metoda umožòuje skrýt/odkrýt silové pùsobení na planetu
        Transform parentSilocar = SpaceObject.GetChild(this.transform, "ParentSilocar");
        foreach (Transform child in parentSilocar)
        {
            GameObject.Destroy(child.gameObject);
        }
        if (zobrazitSilocary) 
        {
            GameObject hlavniSilocara = TvorbaObjektuSilocary(this.transform.position, aktualniSilovePusobeni, this.transform.localScale);
            hlavniSilocara.transform.SetParent(parentSilocar);
            Debug.Log(vsechnaSilovaPusobeni[0].x);
            foreach (Vector3 silocara in vsechnaSilovaPusobeni) 
            {
                GameObject go = TvorbaObjektuSilocary(this.transform.position, silocara, this.transform.localScale);
                go.transform.SetParent(parentSilocar);
            }
        }
    }

    GameObject TvorbaObjektuSilocary(Vector3 poziceObjektu, Vector3 silocara, Vector3 scale) 
    {
        GameObject gameObjectToReturn = new GameObject();
        LineRenderer createdLR = gameObjectToReturn.AddComponent<LineRenderer>();

        Material defaultLine = Resources.Load<Material>("Materials/Default-Line");
        Color toChange = this.GetComponent<MeshRenderer>().materials[0].color;
        toChange.a = 1;

        createdLR.material = defaultLine;
        createdLR.material.SetColor("_TintColor", toChange);
        createdLR.widthMultiplier = 0.05f;

        createdLR.SetPosition(0, poziceObjektu);
        createdLR.SetPosition(1, poziceObjektu + 200*silocara);

        return gameObjectToReturn;
    }

    public static Transform GetChild(Transform hlavni, string childToFind) 
    {
        foreach(Transform t in hlavni) 
        {
            if(t.name == childToFind) 
            {
                return t;
            }
        }
        throw new System.ArgumentException("ERROR: Child of a given parent has not been found!");
    }

    void MoveBy(Vector3 rychlostObjektu)
    {
        //Metoda pohne objektem dle gravitaèního pùsobení a vlastní rychlosti
        TimeManager.CasNasobek a = GameObject.Find("TimeManager").GetComponent<TimeManager>().aktualniCasovyNasobek;

        this.transform.position += (rychlostObjektu / 100000 / 4750 * (float)a);
        //this.transform.position += (rychlostObjektu / 100000 / 50 * (float)a);

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
                    // pøidat podmínku rychlosti?
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
