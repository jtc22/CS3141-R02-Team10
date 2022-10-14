using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    [SerializeField] GameObject stone;
    [SerializeField] GameObject limestone;
    [SerializeField] GameObject air;
    [SerializeField]
    [Range(0.0f, 10.0f)] float noiseFrequency = 1.01f;

    private int[,,] map = new int[100, 100, 20];

    private GameObject objStone;
    private GameObject objLimestone;
    private GameObject objAir;
    
    private int width;
    private int height;
    private int depth;
    private int currDepth;

    private GameObject[,,] createdVoxels;
    private HashSet<Vector3> visibleVoxels = new HashSet<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        width = map.GetLength(0);
        height = map.GetLength(1);
        depth = map.GetLength(2);
        currDepth = depth / 2;

        objStone = new GameObject("StoneHolder");
        objLimestone = new GameObject("LimestoneHolder");
        objAir = new GameObject("AirHolder");

        objStone.transform.parent = this.transform;
        objLimestone.transform.parent = this.transform;
        objAir.transform.parent = this.transform;

        createdVoxels = new GameObject[width, height, depth];

        InitializeMap();
        Generation();
    }
    //comment
    void Update()
    {
        if(Input.mouseScrollDelta.y > 0)
        {
            if(currDepth < depth - 1)
            {
                currDepth++;
                Generation();
            }
        }
        else if(Input.mouseScrollDelta.y < 0)
        {
            if (currDepth > 0)
            {
                currDepth--;
                Generation();
            }
        }
    }

    void InitializeMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    float noise = Mathf.Abs(perlinNoise.get3DPerlinNoise(new Vector3((float)x/width, (float)y/height, (float)z/depth), noiseFrequency));
                    map[x,y,z] = (int) Mathf.Round(noise * 3);
                    
                }
            }
        }
    }


    void Generation()
    {
        int z = currDepth;
        DisableVisibleVoxels();
        visibleVoxels.Clear();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                visibleVoxels.Add(new Vector3(x, y, z));
            }
        }
        GenerateObjects();
    }

    void GenerateObjects()
    {
        foreach (Vector3 v in visibleVoxels)
        {
            int x = (int) v.x;
            int y = (int) v.y;
            int z = (int) v.z;
            // If voxel already is created, just set it to active and continue
            if(createdVoxels[x, y, z] != null)
            {
                createdVoxels[x, y, z].SetActive(true);
                continue;
            }
            GameObject obj;
            // If the voxel doesn't exist create a new one with the given location
            if(map[x,y,z] == 0)
            {
                obj = spawnObj(limestone, x / 8f, y / 8f);
                obj.transform.parent = objLimestone.transform;
            }
            else if (map[x, y, z] == 1)
            {
                obj = spawnObj(stone, x / 8f, y / 8f);
                obj.transform.parent = objStone.transform;
            }
            else
            {
                obj = spawnObj(air, x / 8f, y / 8f);
                obj.transform.parent = objAir.transform;
            }

            createdVoxels[x, y, z] = obj;
        }
    }

    void DisableVisibleVoxels()
    {
        foreach(Vector3 v in visibleVoxels)
        {
            createdVoxels[(int) v.x, (int) v.y, (int) v.z].SetActive(false);
        }
    }

    GameObject spawnObj(GameObject obj, float width, float height, float depth = 0)
    {
        obj = Instantiate(obj, new Vector3(width, height, depth), Quaternion.identity);
        return obj;
    }
}
