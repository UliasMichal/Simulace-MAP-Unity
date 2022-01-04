using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SimulationData
{
    public List<SpaceObjectData> Telesa;
    public float[] datumACasSimulace; //7 prvkù rok, mìsíc, ..., sekunda, milisekunda

    public SimulationData(GameObject souborTeles, TimeManager tmSimulace) 
    {
        Telesa = new List<SpaceObjectData>();
        foreach(SpaceObject sO in souborTeles.GetComponentsInChildren<SpaceObject>(true)) 
        {
            Telesa.Add(new SpaceObjectData(sO));
        }

        datumACasSimulace = new float[7];
        datumACasSimulace[0] = tmSimulace.casSimulace.Year;
        datumACasSimulace[1] = tmSimulace.casSimulace.Month;
        datumACasSimulace[2] = tmSimulace.casSimulace.Day;
        datumACasSimulace[3] = tmSimulace.casSimulace.Hour;
        datumACasSimulace[4] = tmSimulace.casSimulace.Minute;
        datumACasSimulace[5] = tmSimulace.casSimulace.Second;
        datumACasSimulace[6] = tmSimulace.casSimulace.Millisecond;
    }
}
