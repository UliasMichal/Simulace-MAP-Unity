using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    //TimeManager programu urèuje rychlost pohybu èasu - dùležité hlavnì pøi výpoètech gravitace

    public GameObject timeUI;
        
    public enum CasNasobek 
    {
        pauza = 0,
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
        AktualizovatCas();
    }

    public void AktualizovatCas() 
    {
        casSimulace = DateTime.Now;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //FixedUpdate se volá každých 20 milisekund
        int milisecondsToAdd = 20 * ((int)aktualniCasovyNasobek);
        
        casSimulace += new TimeSpan(0,0,0,0, milisecondsToAdd);
        
        //Propíše èas do UI
        timeUI.transform.GetChild(0).GetComponent<Text>().text = casSimulace.Date.ToString("dd:MM:yyyy"); //0 = datum
        timeUI.transform.GetChild(1).GetComponent<Text>().text = casSimulace.TimeOfDay.ToString(@"hh\:mm\:ss"); //1 = èas
    }

}
