using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbeMotory : MonoBehaviour
{
    public Vector3 Motory = new Vector3(0,0,0);
    public bool MotorX = false;
    public bool MotorY = false;
    public bool MotorZ = false;

    public void PosunDleMotoru()
    {
        Vector3 aktualniPozice = this.transform.position;
        
        TimeManager.CasNasobek casovyNasobekSimulace = GameObject.Find("TimeManager").GetComponent<TimeManager>().aktualniCasovyNasobek;

        float modifikator = (float)casovyNasobekSimulace / 100000f / 50f / 10f;

        if (MotorX)
        {
            aktualniPozice.x = Motory[0] * modifikator;
        }
        if (MotorY)
        {
            aktualniPozice.y = Motory[1] * modifikator;
        }
        if (MotorZ)
        {
            aktualniPozice.z = Motory[2] * modifikator;
        }
    }

    public void UpdateMotor(Vector3 toUpdate) 
    {
        Motory.x = toUpdate.x;
        Motory.y = toUpdate.y;
        Motory.z = toUpdate.z;
    }

    public void ZapnoutMotor(bool x, bool y, bool z)
    {
        MotorX = x;
        MotorY = y;
        MotorZ = z;
    }
}
