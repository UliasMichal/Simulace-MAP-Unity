using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class DataManager : MonoBehaviour
{
    public string urlToRequest = $"https://ssd.jpl.nasa.gov/api/horizons.api?format=json&COMMAND=%27#OBJEKTTOREPLACE#%27&OBJ_DATA=%27YES%27&MAKE_EPHEM=%27YES%27&EPHEM_TYPE=%27VECTOR%27&CENTER=%27500@10%27&START_TIME=%272006-01-01%27&STOP_TIME=%272006-01-20%27&STEP_SIZE=%271%20d%27&QUANTITIES=%271,9,20,23,24,29%27";

    public enum FileSelectMode 
    {
        defaultFile,
        saveFile1,
        saveFile2,
        saveFile3,
    }

    enum ObjektySID
    {
        Sun = 10,
        Mercury = 199,
        Venus = 299,
        Earth = 399,
        Mars = 499,
        Jupiter = 599,
        Saturn = 699,
        Uranus = 799,
        Neptune = 899,
        Moon = 301,
        JovianSatalite1 = 501,
        JovianSatalite2 = 502,
        JovianSatalite3 = 503,
        JovianSatalite4 = 504,
        JovianSatalite5 = 505,
        JovianSatalite6 = 506,
        JovianSatalite7 = 507,
        JovianSatalite8 = 508,
        JovianSatalite9 = 509,
        JovianSatalite10 = 510,
        JovianSatalite11 = 511,
        JovianSatalite12 = 512,
        JovianSatalite13 = 513,
        JovianSatalite14 = 514,
        JovianSatalite15 = 515,
        JovianSatalite16 = 516,
        JovianSatalite17 = 517,
        JovianSatalite18 = 518,
        JovianSatalite19 = 519,
        JovianSatalite20 = 520,
        JovianSatalite21 = 521,
        JovianSatalite22 = 522,
        JovianSatalite23 = 523,
        JovianSatalite24 = 524,
        JovianSatalite25 = 525,
        JovianSatalite26 = 526,
        JovianSatalite27 = 527,
        JovianSatalite28 = 528,
        JovianSatalite29 = 529,
        JovianSatalite30 = 530,
        JovianSatalite31 = 531,
        JovianSatalite32 = 532,
        JovianSatalite33 = 533,
        JovianSatalite34 = 534,
        JovianSatalite35 = 535,
        JovianSatalite36 = 536,
        JovianSatalite37 = 537,
        JovianSatalite38 = 538,
        JovianSatalite39 = 539,
        JovianSatalite40 = 540,
        JovianSatalite41 = 541,
        JovianSatalite42 = 542,
        JovianSatalite43 = 543,
        JovianSatalite44 = 544,
        JovianSatalite45 = 545,
        JovianSatalite46 = 546,
        JovianSatalite47 = 547,
        JovianSatalite48 = 548,
        JovianSatalite49 = 549,
        JovianSatalite50 = 550,
        JovianSatalite51 = 551,
        JovianSatalite52 = 552,
        JovianSatalite53 = 553,
        JovianSatalite54 = 554,
        JovianSatalite55 = 555,
        JovianSatalite56 = 556,
        JovianSatalite57 = 557,
        JovianSatalite58 = 558,
        JovianSatalite59 = 559,
        JovianSatalite60 = 560,
        JovianSatalite61 = 561,
        JovianSatalite62 = 562,
        JovianSatalite63 = 563,
        JovianSatalite64 = 564,
        JovianSatalite65 = 565,
        JovianSatalite66 = 566,
        JovianSatalite67 = 567,
        JovianSatalite68 = 568,
        JovianSatalite69 = 569,
        JovianSatalite70 = 570,
        JovianSatalite71 = 571,
        JovianSatalite72 = 572,
        JovianSatalite73 = 55501,
        JovianSatalite74 = 55502,
        JovianSatalite75 = 55503,
        JovianSatalite76 = 55504,
        JovianSatalite77 = 55505,
        JovianSatalite78 = 55506,
        JovianSatalite79 = 55507,
        JovianSatalite80 = 55508,
        SaturnianSatalite1 = 601,
        SaturnianSatalite2 = 602,
        SaturnianSatalite3 = 603,
        SaturnianSatalite4 = 604,
        SaturnianSatalite5 = 605,
        SaturnianSatalite6 = 606,
        SaturnianSatalite7 = 607,
        SaturnianSatalite8 = 608,
        SaturnianSatalite9 = 609,
        SaturnianSatalite10 = 610,
        SaturnianSatalite11 = 611,
        SaturnianSatalite12 = 612,
        SaturnianSatalite13 = 613,
        SaturnianSatalite14 = 614,
        SaturnianSatalite15 = 615,
        SaturnianSatalite16 = 616,
        SaturnianSatalite17 = 617,
        SaturnianSatalite18 = 618,
        SaturnianSatalite19 = 619,
        SaturnianSatalite20 = 620,
        SaturnianSatalite21 = 621,
        SaturnianSatalite22 = 622,
        SaturnianSatalite23 = 623,
        SaturnianSatalite24 = 624,
        SaturnianSatalite25 = 625,
        SaturnianSatalite26 = 626,
        SaturnianSatalite27 = 627,
        SaturnianSatalite28 = 628,
        SaturnianSatalite29 = 629,
        SaturnianSatalite30 = 630,
        SaturnianSatalite31 = 631,
        SaturnianSatalite32 = 632,
        SaturnianSatalite33 = 633,
        SaturnianSatalite34 = 634,
        SaturnianSatalite35 = 635,
        SaturnianSatalite36 = 636,
        SaturnianSatalite37 = 637,
        SaturnianSatalite38 = 638,
        SaturnianSatalite39 = 639,
        SaturnianSatalite40 = 640,
        SaturnianSatalite41 = 641,
        SaturnianSatalite42 = 642,
        SaturnianSatalite43 = 643,
        SaturnianSatalite44 = 644,
        SaturnianSatalite45 = 645,
        SaturnianSatalite46 = 646,
        SaturnianSatalite47 = 647,
        SaturnianSatalite48 = 648,
        SaturnianSatalite49 = 649,
        SaturnianSatalite50 = 650,
        SaturnianSatalite51 = 651,
        SaturnianSatalite52 = 652,
        SaturnianSatalite53 = 653,
        SaturnianSatalite54 = 654,
        SaturnianSatalite55 = 655,
        SaturnianSatalite56 = 656,
        SaturnianSatalite57 = 657,
        SaturnianSatalite58 = 658,
        SaturnianSatalite59 = 659,
        SaturnianSatalite60 = 660,
        SaturnianSatalite61 = 661,
        SaturnianSatalite62 = 662,
        SaturnianSatalite63 = 663,
        SaturnianSatalite64 = 664,
        SaturnianSatalite65 = 665,
        SaturnianSatalite66 = 666,
        SaturnianSatalite67 = 65084,
        SaturnianSatalite68 = 65085,
        SaturnianSatalite69 = 65086,
        SaturnianSatalite70 = 65087,
        SaturnianSatalite71 = 65088,
        SaturnianSatalite72 = 65089,
        SaturnianSatalite73 = 65090,
        SaturnianSatalite74 = 65091,
        SaturnianSatalite75 = 65092,
        SaturnianSatalite76 = 65093,
        UranianSatalite1 = 701,
        UranianSatalite2 = 702,
        UranianSatalite3 = 703,
        UranianSatalite4 = 704,
        UranianSatalite5 = 705,
        UranianSatalite6 = 706,
        UranianSatalite7 = 707,
        UranianSatalite8 = 708,
        UranianSatalite9 = 709,
        UranianSatalite10 = 710,
        UranianSatalite11 = 711,
        UranianSatalite12 = 712,
        UranianSatalite13 = 713,
        UranianSatalite14 = 714,
        UranianSatalite15 = 715,
        UranianSatalite16 = 716,
        UranianSatalite17 = 717,
        UranianSatalite18 = 718,
        UranianSatalite19 = 719,
        UranianSatalite20 = 720,
        UranianSatalite21 = 721,
        UranianSatalite22 = 722,
        UranianSatalite23 = 723,
        UranianSatalite24 = 724,
        UranianSatalite25 = 725,
        UranianSatalite26 = 726,
        UranianSatalite27 = 727,
        NeptunianSatalite1 = 801,
        NeptunianSatalite2 = 802,
        NeptunianSatalite3 = 803,
        NeptunianSatalite4 = 804,
        NeptunianSatalite5 = 805,
        NeptunianSatalite6 = 806,
        NeptunianSatalite7 = 807,
        NeptunianSatalite8 = 808,
        NeptunianSatalite9 = 809,
        NeptunianSatalite10 = 810,
        NeptunianSatalite11 = 811,
        NeptunianSatalite12 = 812,
        NeptunianSatalite13 = 813,
        NeptunianSatalite14 = 814
    }

    async Task Start()
    {
        /*
        List<int> objekty = GetIDsObjektu();

        
        List<int> objekty = new List<int>();
        objekty.Add(10);
        objekty.Add(199);

        //int i = 0;
        foreach (int objektID in objekty)
        {

            string urlToRequestNow = urlToRequest.Replace($"#OBJEKTTOREPLACE#", objektID.ToString());
            await RequestNaApi(urlToRequestNow);
            //Debug.Log(i++);
            System.Threading.Thread.Sleep(10000);

        }
        */
    }

    static async Task RequestNaApi(string urlToRequest)
    {
        //Výsledek tohoto testu bylo, že API má neuvìøitelnì chaotické seøazení dat - i ve formátu JSON je formát OBJ_Data prakticky náhodný (úplnì jiná pozice stejných hodnot a nìkdy hodnoty mají i rùzné názvy).


        //string urlToRequest = $"https://ssd.jpl.nasa.gov/api/horizons.api?format=json&COMMAND=%27499%27&OBJ_DATA=%27YES%27&MAKE_EPHEM=%27YES%27&EPHEM_TYPE=%27OBSERVER%27&CENTER=%27500@399%27&START_TIME=%272006-01-01%27&STOP_TIME=%272006-01-20%27&STEP_SIZE=%271%20d%27&QUANTITIES=%271,9,20,23,24,29%27";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(urlToRequest))
        {
            webRequest.SendWebRequest();
            while (!webRequest.isDone)
            {
                await Task.Yield();
            }


            string responseBody = webRequest.downloadHandler.data.ToString();
            Debug.Log(responseBody);
            responseBody = Regex.Replace(responseBody, @"\s+", " ");
            string[] splitters = { "*******************************************************************************", "*******************************************" };
            string[] parts = responseBody.Split(splitters, StringSplitOptions.RemoveEmptyEntries);

            int indexMass = parts[1].IndexOf("Mass");
            int indexKonce = parts[1].IndexOf(' ', parts[1].IndexOf("= ", parts[1].IndexOf("Mass")) + 2);
            StringBuilder sb = new StringBuilder();
            for(indexMass = indexMass; indexMass < indexKonce; indexMass++) 
            {
                sb.Append(parts[1][indexMass]);
            }

            string[] separator = {"\\n"};


            string mass = sb.ToString(); //hmotnost objektu - formát se liší dle tìlesa
            string[] xyzPozice = parts[8].Split(separator, StringSplitOptions.RemoveEmptyEntries); //pozice X,Y,Z - poèet dle requestù

        }
    }
    

    static int[] GetIDsObjektu()
    {
        //Vrátí list základních objektù v simulaci

        //Enum ObjektySID obsahuje objekty, které je potøeba žádat spolu s ID pod kterými jsou vedeny v rámci HorizonsSystem API
        int[] hodnoty = (int[])Enum.GetValues(typeof(ObjektySID));

        return hodnoty;

    }

    public void MakeOutputFile() 
    {

    }

    public void LoadFromFile() 
    {

    }
}
