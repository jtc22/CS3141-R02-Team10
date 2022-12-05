using System.Collections;
using System.Collections.Generic;
using static MaterialProperties;
using UnityEngine;

public class CaveSimulation : MonoBehaviour
{
    MapRenderer mapr;
    Map map;
    public void Start()
    {
        mapr = this.transform.GetComponent<MapRenderer>();
        map = mapr.map;
    }

    public void Erosion()
    {
        map.age += 15;
        map.InitializeMap();
    }    
}
