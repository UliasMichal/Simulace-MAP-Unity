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
        if (isProbe) 
        {
            this.gameObject.AddComponent<ProbeMotory>();
        }

        noGravityEffect = dataToLoad.noGravityEffect;
        noMovement = dataToLoad.noMovement;

        //P�id� materi�l typu planeta a obarv� objekt dle ulo�en�ch RGB hodnot
        Material planetMat = Resources.Load<Material>("Materials/Planet");
        Material[] matArr = { planetMat };

        Color planetColor = new Color(dataToLoad.colour[0], dataToLoad.colour[1], dataToLoad.colour[2]);
        this.GetComponent<MeshRenderer>().materials = matArr;

        this.GetComponent<MeshRenderer>().materials[0].color = planetColor;
        
        //Nastav� pozici dle ulo�en�ch sou�adnic XYZ
        this.transform.position = new Vector3(dataToLoad.position[0], dataToLoad.position[1], dataToLoad.position[2]);

        //Nastav� aktu�ln� rychlost dle ulo�en�ch sou�adnic XYZ
        this.rychlost = new Vector3(dataToLoad.currentSpeed[0], dataToLoad.currentSpeed[1], dataToLoad.currentSpeed[2]);

        //Nastav� scale objektu
        this.transform.localScale = new Vector3(dataToLoad.scale[0], dataToLoad.scale[1], dataToLoad.scale[2]);
    }
    #endregion

    public void OperaceObjektu()
    {
        //Metoda popisuj�c� pohybov� p�soben� objektu 
        TimeManager.CasNasobek casovyNasobekSimulace = GameObject.Find("TimeManager").GetComponent<TimeManager>().aktualniCasovyNasobek;
        aktualniSilovePusobeni = CelkoveSilovePusobeniGravitace(vsechnaSilovaPusobeni) / 50 / 1100 * (float)casovyNasobekSimulace;

        if (!noMovement)
        {
            rychlost += aktualniSilovePusobeni;
            MoveBy(rychlost, casovyNasobekSimulace); 
        }
        
        OvladaniTrailRendereru();
        OvladaniPopisku();
        OvladaniLineRendereru();

        if (isProbe)
        {
            if (this.GetComponent<ProbeMotory>() != null)
            {
                this.GetComponent<ProbeMotory>().PosunDleMotoru();
            }
        }
    }

    void OvladaniTrailRendereru() 
    {
        //Metoda umo��uje skr�t/odkr�t dr�hu planety

        if (zobrazitDrahy)
        {
            Material defaultLine = Resources.Load<Material>("Materials/Default-Line");
            Color toChange = this.GetComponent<MeshRenderer>().materials[0].color;
            toChange.a = 1;
            Material[] matArr = { defaultLine };
            this.GetComponent<TrailRenderer>().materials = matArr;
            this.GetComponent<TrailRenderer>().materials[0] = defaultLine;
            this.GetComponent<TrailRenderer>().material.SetColor("_TintColor", toChange);
            this.GetComponent<TrailRenderer>().time = float.PositiveInfinity;
        }
        else
        {
            Material invis = Resources.Load<Material>("Materials/Invis");
            Material[] matArr = { invis };
            this.GetComponent<TrailRenderer>().materials = matArr;
            this.GetComponent<TrailRenderer>().time = 0f;
        }
        
    }

    void OvladaniPopisku()
    {
        //Metoda umo��uje skr�t/odkr�t n�zev planety
        
        TextMesh popisekObjektu = this.GetComponentInChildren<TextMesh>();
        if (zobrazitPopisek)
        {
            //popisekObjektu.characterSize = (float)(GameObject.Find("CameraAndLights").transform.position.magnitude);
            popisekObjektu.characterSize = Vector3.Distance(GameObject.Find("CameraAndLights").transform.position, this.transform.position)/750f;
            popisekObjektu.fontSize = 255;
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
        //Metoda umo��uje skr�t/odkr�t silov� p�soben� na planetu
        Transform parentSilocar = SpaceObject.GetChild(this.transform, "ParentSilocar");
        foreach (Transform child in parentSilocar)
        {
            GameObject.Destroy(child.gameObject);
        }
        if (zobrazitSilocary) 
        {
            GameObject hlavniSilocara = TvorbaObjektuSilocary(this.transform.position, aktualniSilovePusobeni);
            hlavniSilocara.transform.SetParent(parentSilocar);
            Debug.Log(vsechnaSilovaPusobeni[0].x);
            foreach (Vector3 silocara in vsechnaSilovaPusobeni) 
            {
                GameObject go = TvorbaObjektuSilocary(this.transform.position, silocara);
                go.transform.SetParent(parentSilocar);
            }
        }
    }

    GameObject TvorbaObjektuSilocary(Vector3 poziceObjektu, Vector3 silocara) 
    {
        //Metoda tvo�� silo��ry objektu
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

    void MoveBy(Vector3 rychlostObjektu, TimeManager.CasNasobek casovyNasobekSimulace)
    {
        //Metoda pohne objektem dle gravita�n�ho p�soben� a vlastn� rychlosti

        this.transform.position += (rychlostObjektu / 100000 / 50 / 10 * (float)casovyNasobekSimulace);

        //Detekuje vzd�lenost mezi nejbli���mi vesm�rn�mi objekty a p��padn� dojde k jejich zni�en�
        NicitelBlizkychObjektu(0.0000000000001f);
    }

    void NicitelBlizkychObjektu(float distance) 
    {
        //Metoda zni�� objekt na ur�itou vzd�lenost (vzd�lenost v UnityJednotk�ch)
        foreach (SpaceObject sO in (SpaceObject[])Resources.FindObjectsOfTypeAll(typeof(SpaceObject)))
        {
            if (Mathf.Abs(sO.transform.position.magnitude - this.transform.position.magnitude) < distance && sO != this)
            {
                if (sO.mass < this.mass)
                {
                    Debug.Log("Destroyed");
                    Destroy(sO.gameObject);
                }
                if (sO.mass > this.mass)
                {
                    Debug.Log("Destroyed");
                    Destroy(this.gameObject);
                }
                if (sO.mass == this.mass)
                {
                    Debug.Log("Destroyed");
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
