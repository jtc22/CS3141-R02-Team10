using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MaterialProperties;

public class Map
{
    // Public Variables
    public int[,,] mapMatrix { get; set; }
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
    public Map(int width, int height, int depth, int age, int waterLevel)
    {
        this.width = width;
        this.height = height;
        this.depth = depth;
        this.noiseFrequency = height / Random.Range(25.0f, 55.0f);
        this.age = age;
        this.waterLevel = waterLevel;
        mapMatrix = new int[width, height, depth];
        erosionMap = new HashSet<Vector3>();
        InitializeMap();
    }

    // This where all the cool stuff happens eventually
    public void InitializeMap()
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
                    float divisor = height;
                    float noise = Mathf.Abs(perlinNoise.get3DPerlinNoise(new Vector3((float)x / (width * 5), (float)y / height, (float)z / (width * 5)), noiseFrequency));
                    MaterialProperties.Material material = (MaterialProperties.Material)(int)(noise * (numMaterials()-1));

                    float steepness = 0.1f;
                    int cliffFaceOffset = 130;
                    
                    double cliffFace = sig(steepness, cliffFaceOffset, cliffX) * height * 0.87f + height*0.1f;

                    if(y > cliffFace)
                    {
                        material = MaterialProperties.Material.air;
                    }

                    float erosionNoise = (Mathf.PerlinNoise((float)x / (width * 0.02f), (float)z / (width * 0.02f)) * 0.4f + 0.4f) * (Mathf.Pow(0.975f, Mathf.Abs(waterLevel-y)));

                    // Add to the matrix that will be erroded
                    divisor = Mathf.Max(width, height);
                    if( y < cliffFace && 
                        y < waterLevel + (Mathf.PerlinNoise((float)x/(width*0.12f), (float)z/(width*0.12f)) * age/3.5) && 
                        noise < erosionNoise && //(Random.Range(0.45f, 0.6f)) && 
                        // perlinNoise.get3DPerlinNoise(new Vector3((float)x / (divisor), (float)y / divisor, (float)z / (divisor)), noiseFrequency*1.0f) < 0.3f &&
                        x < (-((float)1/(age / 10) * 1.8f) * Mathf.Pow((y - waterLevel), 2) + cliffFaceOffset + age + (Mathf.PerlinNoise((float)x / (width * 0.02f), (float)z / (width * 0.02f)) * 60)))
                    {
                        material = MaterialProperties.Material.air;
                    }

                    // Set the air to water if its under the water level
                    if(y <= waterLevel && material == MaterialProperties.Material.air)
                    {
                        material = MaterialProperties.Material.water;
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

    float invSig(float c1, int c2, int y){
        return Mathf.Log(y / (1-y)) / c1 + c2;
    }

}
