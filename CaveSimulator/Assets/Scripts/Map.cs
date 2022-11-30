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
    public CaveMat[,,] mapMatrix { get; set; }
    public HashSet<Vector3> erosionMap { get; set; }
    public int width { get; }
    public int height { get; }
    public int depth { get; }
    public Texture2D[,] materialTextureLayers { get; set; }
    public int waterLevel { get; set; }
    public int age {get; set;}

    // Private variables
    private float noiseFrequency = 1.01f;
    

    // Constructor
    public Map(int width, int height, int depth)
    {
        this.width = width;
        this.height = height;
        this.depth = depth;
        this.noiseFrequency = height / Random.Range(25.0f, 55.0f);
        age = 450;
        waterLevel = height / 3;
        mapMatrix = new CaveMat[width, height, depth];
        erosionMap = new HashSet<Vector3>();
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

        int cliffX = 0;

        // Loop through all points on the map and set the texture value at that map
        for (int x = 0; x < width; x++)
        {
            if (Random.Range(0f,1f) > .5){
                cliffX++;
            } 

            for (int z = 0; z < depth; z++)
            {
                for (int y = 0; y < height; y++)
                {
                    float divisor = height; //Mathf.Max(width, height, depth);
                    float noise = Mathf.Abs(perlinNoise.get3DPerlinNoise(new Vector3((float)x / (width * 10), (float)y / height, (float)z / (width*10)), noiseFrequency));
                    CaveMat material = (CaveMat)(int)(noise * (numMaterials()-1));

                    // int cliffOffset = (int)((Mathf.PerlinNoise(x, y) - .3) * 30);

                    float steepness = 0.1f;
                    int cliffFaceOffset = 130;
                    
                    double cliffFace = sig(steepness, cliffFaceOffset, cliffX) * height * 0.9f;

                    if(y > cliffFace) // TODO Perlin noise for the offset
                    {
                        material = CaveMat.air;
                    }

                    // Add to the matrix that will be eroded
                    divisor = Mathf.Max(width, height);
                    if( // If y is beneath the face of the cliff
                        y < cliffFace && 
                        // If y is beneath or around the water level
                        y < waterLevel + (Mathf.PerlinNoise((float)x/(width*0.12f), (float)z/(width*0.12f)) * 70) && 
                        // Generating small pockets of air, adding randomness
                        perlinNoise.get3DPerlinNoise(new Vector3((float)x / (divisor), (float)y / divisor, (float)z / (divisor)), noiseFrequency*2.0f) < (0.08f - (getHardness(mapMatrix[x,y,z]) / 100)) &&
                        // Comparing x against age
                        x < (-0.05f * Mathf.Pow((y - waterLevel), 2) + cliffFaceOffset + age + (Mathf.PerlinNoise((float)x / (width * 0.02f), (float)z / (width * 0.02f)) * 100))
                        )                        
                    {
                        erosionMap.Add(new Vector3(x, y, z));
                        material = CaveMat.air;
                    }

                    // Set the air to water if its under the water level
                    if(y <= waterLevel && material == CaveMat.air)
                    {
                        material = CaveMat.water;
                    }

                    MaterialProperty mat = getMaterialProperties(material);
                    mapMatrix[x, y, z] = material;
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
            CaveMat material = mapMatrix[x, y, z];
            // Set the air to water if its under the water level
            if (y <= waterLevel && material == CaveMat.air)
            {
                material = CaveMat.water;
                mapMatrix[x, y, z] = CaveMat.water;
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
        return System.Enum.GetNames(typeof(CaveMat)).Length;
    }

    float sig(float c1, int c2, int x)
    {
        return 1/(1 + Mathf.Exp(-1 * c1 * (x - c2)));

    }

    float invSig(float c1, int c2, int y){
        return Mathf.Log(y / (1-y)) / c1 + c2;
    }

}
