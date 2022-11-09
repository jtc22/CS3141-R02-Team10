using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MaterialProperties;

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
    public List<Vector3> erosionMap { get; set; }
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
        erosionMap = new List<Vector3>();
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

        // Loop through all points on the map and set the texture value at that map
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float divisor = height; //Mathf.Max(width, height, depth);
                    float noise = Mathf.Abs(perlinNoise.get3DPerlinNoise(new Vector3((float)x / (width * 10), (float)y / height, (float)z / (width*10)), noiseFrequency));
                    MaterialProperties.Material material = (MaterialProperties.Material)(int)(noise * (numMaterials()-1));

                    float cliffFace = sig(0.1f, 150, x) * height * 0.9f; // + (int)(Mathf.PerlinNoise(y, depth) * 25)


                    if(y > cliffFace) // TODO Perlin noise for the offset
                    {
                        material = MaterialProperties.Material.air;
                    }

                    // Set the air to water if its under the water level
                    if(y <= waterLevel && material == MaterialProperties.Material.air)
                    {
                        material = MaterialProperties.Material.water;
                    }

                    // Add to the matrix that will be erroded
                    if(y > cliffFace - 60 && y < cliffFace + 20 && y < waterLevel + 40)
                    {
                        erosionMap.Add(new Vector3(x, y, z));
                    }

                    MaterialProperty mat = getMaterialProperties(material);
                    mapMatrix[x, y, z] = (int)material;
                    materialTextureLayers[z, (int)material].SetPixel(x, y, mat.color);
                }
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

    public void updateMap(HashSet<Vector3> updated)
    {
        // Loop through all points on the map and set the texture value at that map
        foreach(Vector3 pix in updated){
            int x = (int)pix.x;
            int y = (int)pix.y;
            int z = (int)pix.z;
            MaterialProperties.Material material = (MaterialProperties.Material) mapMatrix[x, y, z];
            // Set the air to water if its under the water level
            if (y <= waterLevel && material == MaterialProperties.Material.air)
            {
                material = MaterialProperties.Material.water;
                mapMatrix[x, y, z] = (int) MaterialProperties.Material.water;
            }
            MaterialProperty mat = getMaterialProperties(material);
            materialTextureLayers[z, (int)material].SetPixel(x, y, mat.color);
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
        return System.Enum.GetNames(typeof(MaterialProperties.Material)).Length;
    }

    float sig(float c1, int c2, int x)
    {
        return 1/(1 + Mathf.Exp(-1 * c1 * (x - c2)));
    }

}
