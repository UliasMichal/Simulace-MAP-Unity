using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpaceObjectData
{
    public string name;
    public float mass;
    public int[] colour;

    public float[] position;
    public float[] currentSpeed;

    public bool isProbe;

    public SpaceObjectData(SpaceObject sO) 
    {
        name = sO.name;
        mass = sO.mass;
        //colour = sO.
        
        position = new float[3];
        position[0] = sO.transform.position.x;
        position[1] = sO.transform.position.y;
        position[2] = sO.transform.position.z;

        currentSpeed = new float[3];
        currentSpeed[0] = sO.rychlost.x;
        currentSpeed[1] = sO.rychlost.y;
        currentSpeed[2] = sO.rychlost.z;


    }
}
