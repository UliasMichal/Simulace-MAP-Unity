using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpaceObjectData
{
    public string name;
    public float mass;
    public float[] colour;

    public float[] position;
    public float[] baseSpeed;
    public float[] currentSpeed;

    public bool isProbe;

    public SpaceObjectData(SpaceObject sO) 
    {
        name = sO.name;
        mass = sO.mass;

        Color c = sO.GetComponent<Renderer>().material.color;
        colour = new float[3];
        colour[0] = c.r;
        colour[1] = c.g;
        colour[2] = c.b;

        position = new float[3];
        position[0] = sO.transform.position.x;
        position[1] = sO.transform.position.y;
        position[2] = sO.transform.position.z;

        currentSpeed = new float[3];
        currentSpeed[0] = sO.rychlost.x;
        currentSpeed[1] = sO.rychlost.y;
        currentSpeed[2] = sO.rychlost.z;

        baseSpeed = new float[3];
        baseSpeed[0] = sO.zakladniRychlostObjektu.x;
        baseSpeed[1] = sO.zakladniRychlostObjektu.y;
        baseSpeed[2] = sO.zakladniRychlostObjektu.z;

        isProbe = sO.isProbe;
    }
}
