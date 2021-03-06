using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    //TimeManager programu ur?uje rychlost pohybu ?asu - d?le?it? hlavn? p?i v?po?tech gravitace

    public GameObject timeUI;
    public Text timeNasobekText;
    public Button timeSpeedDown;
    public Button timeSpeedUp;


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
        denZaS = 86400
        //tydenZaS = 604800 //ji? nen? p?esn? - muselo by se to ?e?it p?es v?cero v?po?t? -> sn??en? kvality zobrazen? 

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
        /*
        if(casSimulace >= new DateTime(2022,1,2,0,0,0)) 
        {
            ZastavitCas();
        }
        */

        /*
        if (casSimulace >= new DateTime(2022, 1, 31, 0, 0, 0))
        {
            ZastavitCas();
        }
        */

        /*
        if (casSimulace >= new DateTime(2022, 2, 28, 0, 0, 0))
        {
            ZastavitCas();
        }
        */

        //FixedUpdate se vol? ka?d?ch 20 milisekund
        int milisecondsToAdd = 20 * ((int)aktualniCasovyNasobek);
        
        casSimulace += new TimeSpan(0,0,0,0, milisecondsToAdd);
        
        //Prop??e ?as do UI
        timeUI.transform.GetChild(0).GetComponent<Text>().text = casSimulace.Date.ToString("dd:MM:yyyy"); //0 = datum
        timeUI.transform.GetChild(1).GetComponent<Text>().text = casSimulace.TimeOfDay.ToString(@"hh\:mm\:ss"); //1 = ?as
        timeNasobekText.text = ((int)aktualniCasovyNasobek).ToString();
    }

    public void ZmenaCasovehoNasobku(int zmena)
    {
        CasNasobek[] poleNasobku = (CasNasobek[])Enum.GetValues(aktualniCasovyNasobek.GetType());
        int j = Array.IndexOf<CasNasobek>(poleNasobku, aktualniCasovyNasobek) + zmena;

        aktualniCasovyNasobek = (poleNasobku.Length == j) ? poleNasobku[0] : poleNasobku[j];
        timeSpeedUp.interactable = !(aktualniCasovyNasobek == CasNasobek.denZaS);
        timeSpeedDown.interactable = !(aktualniCasovyNasobek == CasNasobek.jedna || aktualniCasovyNasobek == CasNasobek.pauza);
    }

    public void ZastavitCas()
    {
        aktualniCasovyNasobek = CasNasobek.pauza;
        timeSpeedUp.interactable = false;
        timeSpeedDown.interactable = false;
    }

    public void SpustitCas()
    {
        aktualniCasovyNasobek = CasNasobek.jedna;
        timeSpeedUp.interactable = true;
    }
}
