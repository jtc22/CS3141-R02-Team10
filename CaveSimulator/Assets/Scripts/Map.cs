using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{

    // Public Variables
    public int[,,] mapMatrix { get; set; }
    public int width { get; }
    public int height { get; }
    public int depth { get; }
    public GameObject[] materialMakeUp { get; }

    // Private variables
    private float noiseFrequency = 1.01f;
    

    // Constructor
    public Map(int width, int height, int depth, float noiseFrequency, GameObject[] materials)
    {
        this.width = width;
        this.height = height;
        this.depth = depth;
        this.noiseFrequency = noiseFrequency;
        this.materialMakeUp = materials;
        mapMatrix = new int[width, height, depth];
        InitializeMap();
    }

    // This where all the cool stuff happens eventually
    void InitializeMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    float divisor = Mathf.Max(width, height, depth);
                    float noise = Mathf.Abs(perlinNoise.get3DPerlinNoise(new Vector3((float)x / divisor, (float)y / divisor, (float)z / divisor), noiseFrequency));
                    mapMatrix[x, y, z] = (int)Mathf.Round(noise * 2);
                }
            }
        }
    }


}
