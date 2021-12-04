using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public enum CasNasobek 
    {
        jedna = 1,
        dva = 2,
        ctyri = 4,
        osm = 8,
        sestnact = 16,
        tricetDva = 32,
        sedesatCtyri = 64,
        stoDvacetOsm = 128,
        dvestePadesatSest = 256,
        denZaS = 86400,
        tydenZaS = 604800
    }

    public DateTime casSimulace;
    public CasNasobek aktualniCasovyNasobek;


    // Start is called before the first frame update
    void Start()
    {
        casSimulace = DateTime.Now;
        //aktualniCasovyNasobek = CasNasobek.jedna;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //FixedUpdate se vol� ka�d�ch 20 milisekund

        if (false) //p�idat podm�nku - pokud bude zm�n�n� hodnota v UI - zm�� ji i zde
        {
            //p�idat zm�nu �asov�ho n�sobku
        }

        int milisecondsToAdd = 20 * ((int)aktualniCasovyNasobek);
        
        casSimulace += new TimeSpan(0,0,0,0, milisecondsToAdd);
        //Debug.Log(casSimulace);
    }

}
