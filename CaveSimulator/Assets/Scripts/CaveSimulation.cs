using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MaterialProperties;

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
        HashSet<Vector3> pixelsToErode = waterContact();
        foreach(Vector3 pixel in pixelsToErode)
        {
            Map.Material material = (Map.Material) map.mapMatrix[(int)pixel.x, (int)pixel.y, (int)pixel.z];

            if(Map.getMaterialProperties(material).density * Map.getMaterialProperties(material).hardness < Random.Range(0f,3.1f))
                map.mapMatrix[(int)pixel.x, (int)pixel.y, (int)pixel.z] = (int)Map.Material.air;
        }
        map.updateMap(pixelsToErode);

    }

    public HashSet<Vector3> waterContact()
    {
        int maxHeight = map.waterLevel+20;
        HashSet<Vector3> contactSet = new HashSet<Vector3>();

        for(int z = 0; z < map.depth; z++)
        {
            for(int x = 0; x < map.width; x++)
            {
                for(int y = 0; y < maxHeight; y++)
                {
                    if(map.mapMatrix[x, y, z] == (int)Map.Material.water)
                    {
                        for(int xOff = -1; xOff <= 1; xOff++) {
                            if (x + xOff > map.width - 1 || x + xOff < 0)
                                continue;
                            if (map.mapMatrix[x + xOff, y, z] != (int)Material.water && map.mapMatrix[x + xOff, y, z] != (int)Map.Material.air)
                                contactSet.Add(new Vector3(x + xOff, y, z));
                        }
                        for(int yOff = -1; yOff <= 1; yOff++) {
                            if (y + yOff > map.height - 1 || y + yOff < 0)
                                continue;
                            if (map.mapMatrix[x, y + yOff, z] != (int)Material.water && map.mapMatrix[x, y + yOff, z] != (int)Map.Material.air)
                                contactSet.Add(new Vector3(x, y + yOff, z));
                        }
                        for(int zOff = -1; zOff <= 1; zOff++) {
                            if (z + zOff > map.depth - 1 || z + zOff < 0)
                                continue;
                            if (map.mapMatrix[x, y, z + zOff] != (int)Material.water && map.mapMatrix[x, y, z + zOff] != (int)Map.Material.air)
                                contactSet.Add(new Vector3(x, y, z + zOff));
                        }    
                    }
                }
            }
        }

        return contactSet;
    }
}
