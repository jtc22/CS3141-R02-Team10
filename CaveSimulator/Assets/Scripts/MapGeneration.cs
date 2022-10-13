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

    private int[,,] map = new int[100, 100, 100];

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

        createdVoxels = new GameObject[width, height, depth];

        InitializeMap();
        Generation();
    }

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
                    // Debug.Log(noise);
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
            
            // If the voxel doesn't exist create a new one with the given location
            if(map[x,y,z] == 0)
            {
                createdVoxels[x, y, z] = spawnObj(stone, x / 8f, y / 8f);
            }
            else if (map[x, y, z] == 1)
            {
                createdVoxels[x, y, z] = spawnObj(limestone, x / 8f, y / 8f);
            }
            else
            {
                createdVoxels[x, y, z] = spawnObj(air, x / 8f, y / 8f);
            }
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
        obj.transform.parent = this.transform;
        return obj;
    }
}
