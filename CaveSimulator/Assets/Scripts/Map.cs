using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{

    // TODO getlayer()
    // TODO edit functions
    // TODO material class
    // TODO map parameters (rainfall, temp, elevation, humidity, air pressure/makeup), water table
    // TODO pack into CSV file
    // TODO load map from CSV
    // TODO enum for material type
    // TODO getAttributes(enum) for material 


    // TODO Generation - map features
    // TODO Add dolomite, sandstone, and granite

    public enum Material {
        basalt,
        limeStone,
        air
    }

    private struct MaterialProperty {
        public int density;
        public int hardness;
        public int solubility;
        public Color color;
    }

    // Public Variables
    public int[,,] mapMatrix { get; set; }
    public int width { get; }
    public int height { get; }
    public int depth { get; }
    public Texture2D[,] materialTextureLayers { get; set; }

    // Private variables
    private float noiseFrequency = 1.01f;
    

    // Constructor
    public Map(int width, int height, int depth, GameObject[] materials)
    {
        this.width = width;
        this.height = height;
        this.depth = depth;
        this.noiseFrequency = width / 20f;
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

        // Loop through all points on the map and set the texture value at that map
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float divisor = Mathf.Max(width, height, depth);
                    float noise = Mathf.Abs(perlinNoise.get3DPerlinNoise(new Vector3((float)x / divisor, (float)y / divisor, (float)z / divisor), noiseFrequency));
                    int material = (int)Mathf.Round(noise * 3);
                    if (material > 2) material = 2;
                    MaterialProperty mat = getMaterialProperties((Material) material);
                    mapMatrix[x, y, z] = material;
                    materialTextureLayers[z, material].SetPixel(x, y, mat.color);
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

    public int numMaterials()
    {
        return System.Enum.GetNames(typeof(Material)).Length;
    }

    private MaterialProperty getMaterialProperties(Material material){
        MaterialProperty prop = new MaterialProperty();
        switch (material)
        {
            case(Material.basalt):
                prop.density = 1;
                prop.hardness = 1;
                prop.solubility = 1;
                prop.color = new Color(0.77f, 0.78f, 0.79f, 1);
                return prop;
            case(Material.limeStone):
                prop.density = 1;
                prop.hardness = 1;
                prop.solubility = 1;
                prop.color = new Color(0.74f, 0.74f, 0.56f, 1);
                return prop;
            case(Material.air):
                prop.density = 1;
                prop.hardness = 1;
                prop.solubility = 1;
                prop.color = new Color(0.32f, 0.32f, 0.32f, 1);
                return prop;
            default:
                prop.density = 1;
                prop.hardness = 1;
                prop.solubility = 1;
                prop.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                return prop;
        }
    }

}
