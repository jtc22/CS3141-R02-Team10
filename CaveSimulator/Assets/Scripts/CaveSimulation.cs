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
        HashSet<Vector3> pixelsToErode = new HashSet<Vector3>(map.erosionMap);
        foreach(Vector3 pixel in pixelsToErode)
        {
            MaterialProperties.Material material = (MaterialProperties.Material) map.mapMatrix[(int)pixel.x, (int)pixel.y, (int)pixel.z];
            MaterialProperties.MaterialProperty matProp = MaterialProperties.getMaterialProperties(material);

            float fr = (matProp.density * matProp.hardness) / matProp.solubility; // Resistance force
            float fw = 5.0f; // Wave force
            float k = 5.0f; // Arbitrary constant

            if(k * Mathf.Log(fw/fr) > 0)
                map.mapMatrix[(int)pixel.x, (int)pixel.y, (int)pixel.z] = (int)MaterialProperties.Material.air;
        }
        map.updateMap(pixelsToErode);

    }
}
