using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpaceObjectData
{
    public string name;
    public float mass;
    public float[] colour;

    public float[] scale;

    public float[] position;
    public float[] currentSpeed;

    public bool zobrazitSilocary;
    public bool zobrazitDrahy;

    public bool isProbe;

    public override string ToString()
    {
        return name + ": " + position[0] + " " + position[1] + " " + position[2];
    }
    public SpaceObjectData(SpaceObject sO) 
    {
        name = sO.name;
        mass = sO.mass;

        Color c = sO.GetComponent<Renderer>().material.color;
        colour = new float[3];
        colour[0] = c.r;
        colour[1] = c.g;
        colour[2] = c.b;

        scale = new float[3];
        scale[0] = sO.transform.localScale.x;
        scale[1] = sO.transform.localScale.y;
        scale[2] = sO.transform.localScale.z;

        position = new float[3];
        position[0] = sO.transform.position.x;
        position[1] = sO.transform.position.y;
        position[2] = sO.transform.position.z;

        currentSpeed = new float[3];
        currentSpeed[0] = sO.rychlost.x;
        currentSpeed[1] = sO.rychlost.y;
        currentSpeed[2] = sO.rychlost.z;

        zobrazitSilocary = sO.zobrazitSilocary;
        zobrazitDrahy = sO.zobrazitDrahy;

        isProbe = sO.isProbe;
    }
}
