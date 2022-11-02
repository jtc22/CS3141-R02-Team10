using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MaterialProperties;

public class Map
{

    // TODO getlayer()
    // TODO edit functions
    // TODO material class
    // TODO map parameters (rainfall, temp, elevation, humidity, air pressure/makeup), water table
    // TODO pack into CSV file
    // TODO load map from CSV
    // TODO getAttributes(enum) for material 

    // TODO Generation - map features


    // Public Variables
    public int[,,] mapMatrix { get; set; }
    public int width { get; }
    public int height { get; }
    public int depth { get; }
    public Texture2D[,] materialTextureLayers { get; set; }
    public int waterLevel { get; set; }
    public int waterDepth { get; set; }

    // Private variables
    private float noiseFrequency = 1.01f;
    

    // Constructor
    public Map(int width, int height, int depth)
    {
        this.width = width;
        this.height = height;
        this.depth = depth;
        this.noiseFrequency = height / Random.Range(25.0f, 55.0f);
        waterLevel = height / 3;
        waterDepth = height / 6;
        mapMatrix = new int[width, height, depth];
        InitializeMap();
    }

    // This where all the cool stuff happens eventually
    void InitializeMap()
    {
        // Create all of the textures needed for the map
        materialTextureLayers = new Texture2D[depth, numMaterials()];
        for (int z = 0; z < depth; z++)
        {
            for (int mat = 0; mat < numMaterials(); mat++)
            {
                Texture2D tempTex = new Texture2D(width, height);

                // Reset all pixels color to transparent
                Color32 resetColor = new Color32(255, 255, 255, 0);
                Color32[] resetColorArray = tempTex.GetPixels32();

                for (int i = 0; i < resetColorArray.Length; i++)
                {
                    resetColorArray[i] = resetColor;
                }

                tempTex.SetPixels32(resetColorArray);

                tempTex.filterMode = FilterMode.Point;
                materialTextureLayers[z, mat] = tempTex;
            }
        }

        float cliffOffset = 0;

        // Loop through all points on the map and set the texture value at that map
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float divisor = height; //Mathf.Max(width, height, depth);
                    float noise = Mathf.Abs(perlinNoise.get3DPerlinNoise(new Vector3((float)x / (width * 10), (float)y / height, (float)z / (width*10)), noiseFrequency));
                    Material material = (Material)(int)(noise * (numMaterials()+1));

                    if(y > (sig(0.1f, 200, x + (int)cliffOffset) * height * 0.9))
                    {
                        material = Material.air;
                    }

                    // Set the air to water if its under the water level
                    if(y <= waterLevel && material == Material.air)
                    {
                        material = Material.water;
                    }

                    MaterialProperty mat = getMaterialProperties(material);
                    mapMatrix[x, y, z] = (int)material;
                    materialTextureLayers[z, (int)material].SetPixel(x, y, mat.color);
                }
                cliffOffset += Random.Range(-1.5f,1.5f);
            }
        }

        // Apply the pixel updates to all textures
        for (int z = 0; z < depth; z++)
        {
            for (int mat = 0; mat < numMaterials(); mat++)
            {
                materialTextureLayers[z, mat].Apply();
            }
        }
    }

    public int numMaterials()
    {
        return System.Enum.GetNames(typeof(Material)).Length;
    }

    float sig(float c1, int c2, int x)
    {
        return 1/(1 + Mathf.Exp(-1 * c1 * (x - c2)));
    }

}
