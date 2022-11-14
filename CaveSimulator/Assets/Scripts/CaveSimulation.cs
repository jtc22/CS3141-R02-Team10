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
        HashSet<Vector3> pixelsToErode = new HashSet<Vector3>();
        foreach (Vector3 pixel in map.erosionMap)
        {
            for(int xoff = -2; xoff <= 10; xoff++)
            {
                for (int yoff = -2; yoff <= 10; yoff++)
                {
                    for (int zoff = -5; zoff <= 5; zoff++)
                    {
                        // Check if it is in bounds of the map
                        if (pixel.x + xoff > map.width - 1 || 
                            pixel.x + xoff < 0 || 
                            pixel.y + yoff > map.height - 1 || 
                            pixel.y + yoff < 0 || 
                            pixel.z + zoff > map.depth - 1 || 
                            pixel.z + zoff < 0) continue;

                        if (getMaterial(pixel + new Vector3(xoff, yoff, zoff)) == MaterialProperties.Material.water)
                        {
                            MaterialProperties.Material material = getMaterial(pixel);
                            MaterialProperties.MaterialProperty matProp = MaterialProperties.getMaterialProperties(material);

                            float fr = (matProp.density * matProp.hardness) / matProp.solubility; // Resistance force
                            float fw = Random.Range(1.0f, 3.1f) * (pixel.y > map.waterLevel ? Mathf.Exp(-(pixel.y - map.waterLevel) / 10) : 1); // Wave force
                            float k = 0.5f; // Arbitrary constant

                            if (erodePixel(pixel, fr, fw, k))
                            {
                                pixelsToErode.Add(pixel);
                            }

                        }
                    }
                }
            }
        }
        map.updateMap(map.erosionMap);

        // foreach(Vector3 pixel in pixelsToErode)
        // {
        //     MaterialProperties.Material material = getMaterial(pixel);
        //     MaterialProperties.MaterialProperty matProp = MaterialProperties.getMaterialProperties(material);

        //     float fr = (matProp.density * matProp.hardness) / matProp.solubility; // Resistance force
        //     float fw = Random.Range(1.0f, 6.0f) * (pixel.y > map.waterLevel ? Mathf.Exp(-(pixel.y - map.waterLevel) / 10) : 1); // Wave force
        //     float k = 1.0f; // Arbitrary constant

        //     if(erodePixel(pixel, fr, fw, k))
        //     {
        //         map.erosionMap.Add(pixel + new Vector3(1, 0, 0));
        //         map.erosionMap.Add(pixel + new Vector3(0, 1, 0));
        //     }
        // }
    }

    bool erodePixel(Vector3 pixel, float fr, float fw, float k)
    {
        if (k * Mathf.Log(fw / fr) > 0)
        {
            map.mapMatrix[(int)pixel.x, (int)pixel.y, (int)pixel.z] = (int)MaterialProperties.Material.air;
            return true;
        }
        return false;
    }

    MaterialProperties.Material getMaterial(Vector3 pixel)
    {
        return (MaterialProperties.Material) map.mapMatrix[(int)pixel.x, (int)pixel.y, (int)pixel.z];
    }
}
