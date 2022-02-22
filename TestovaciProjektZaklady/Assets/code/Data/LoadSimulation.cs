using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LoadSimulation
{
    public static void LoadSimulationFromData(GameObject objektSimulace)
    {
        TransferSimulationDataBetweenScenes.HasDataToTransfer = false;
        List<SpaceObjectData> sodList = TransferSimulationDataBetweenScenes.DataToTransfer.Telesa;

        foreach (Transform child in objektSimulace.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (SpaceObjectData sod in sodList)
        {
            GameObject newObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            newObject.transform.parent = objektSimulace.transform;

            AddSpaceObject(newObject, sod);
            AddTrailRenderer(newObject);
            AddPopisek(newObject);
            AddParentSilocar(newObject);
        }
    }

    static void AddSpaceObject(GameObject toAdd, SpaceObjectData sod)
    {
        toAdd.AddComponent<SpaceObject>();

        toAdd.GetComponent<SpaceObject>().LoadFromData(sod);

        toAdd.GetComponent<SpaceObject>().vsechnaSilovaPusobeni = new List<Vector3>();
    }

    static void AddTrailRenderer(GameObject toAdd)
    {
        toAdd.AddComponent<TrailRenderer>();
        toAdd.GetComponent<TrailRenderer>().widthMultiplier = 0.5f;
        toAdd.GetComponent<TrailRenderer>().time = float.PositiveInfinity;
        toAdd.GetComponent<TrailRenderer>().enabled = true;
        toAdd.GetComponent<TrailRenderer>().emitting = true;
    }

    static void AddPopisek(GameObject toAdd)
    {
        GameObject objektPopisku = new GameObject("Label");
        objektPopisku.AddComponent<TextMesh>();
        objektPopisku.transform.SetParent(toAdd.transform);
    }

    static void AddParentSilocar(GameObject toAdd)
    {
        GameObject objektPopisku = new GameObject("ParentSilocar");
        objektPopisku.transform.SetParent(toAdd.transform);
    }
}
