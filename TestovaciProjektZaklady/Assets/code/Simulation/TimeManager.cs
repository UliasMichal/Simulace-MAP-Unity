using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    //TimeManager programu ur�uje rychlost pohybu �asu - d�le�it� hlavn� p�i v�po�tech gravitace

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
        tydenZaS = 604800, //7 dn�
        mesicZaS = 2592000, //30 dn�
        rokZaS = 31557600 //368,25 dn�

    }

    public DateTime casSimulace;
    public CasNasobek aktualniCasovyNasobek;

    // Start is called before the first frame update
    void Start()
    {
        AktualizovatCas();

        if (TransferSimulationDataBetweenScenes.HasDataToTransfer)
        {

            int[] timeFromFile = TransferSimulationDataBetweenScenes.DataToTransfer.datumACasSimulace;

            casSimulace = new DateTime(timeFromFile[0], timeFromFile[1], timeFromFile[2], timeFromFile[3], timeFromFile[4], timeFromFile[5], timeFromFile[6]);
        }
    }

    public void AktualizovatCas() 
    {
        casSimulace = DateTime.Now;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //FixedUpdate se vol� ka�d�ch 20 milisekund
        int milisecondsToAdd = 20 * ((int)aktualniCasovyNasobek);
        
        casSimulace += new TimeSpan(0,0,0,0, milisecondsToAdd);
        
        //Prop�e �as do UI
        timeUI.transform.GetChild(0).GetComponent<Text>().text = casSimulace.Date.ToString("dd:MM:yyyy"); //0 = datum
        timeUI.transform.GetChild(1).GetComponent<Text>().text = casSimulace.TimeOfDay.ToString(@"hh\:mm\:ss"); //1 = �as
    }

    public void ZmenaCasovehoNasobku(int zmena)
    {
        CasNasobek[] Arr = (CasNasobek[])Enum.GetValues(aktualniCasovyNasobek.GetType());
        int j = Array.IndexOf<CasNasobek>(Arr, aktualniCasovyNasobek) + zmena;
        aktualniCasovyNasobek = (Arr.Length == j) ? Arr[0] : Arr[j];
    }

    public void ZastavitCas()
    {
        aktualniCasovyNasobek = CasNasobek.pauza;
    }
    public void SpustitCas()
    {
        aktualniCasovyNasobek = CasNasobek.jedna;
    }
}
