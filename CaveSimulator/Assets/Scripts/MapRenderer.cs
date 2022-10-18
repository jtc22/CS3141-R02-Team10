using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRenderer : MonoBehaviour
{
    [SerializeField] public GameObject stone;
    [SerializeField] public GameObject limeStone;
    [SerializeField] public GameObject air;
    
    public Map map { get; set;}
    private int currDepth;
    private GameObject[,,] createdVoxels;
    private HashSet<Vector3> visibleVoxels = new HashSet<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        map = new Map(500, 200, 10, 25.5f, new GameObject[3] {stone, limeStone, air});
        currDepth = map.depth / 2;

        createdVoxels = new GameObject[map.width, map.height, map.depth];

        Generation();
    }
    //comment
    void Update()
    {
        if(Input.mouseScrollDelta.y > 0)
        {
            if(currDepth < map.depth - 1)
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

    void Generation()
    {
        int z = currDepth;
        DisableVisibleVoxels();
        visibleVoxels.Clear();

        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                visibleVoxels.Add(new Vector3(x, y, z));
            }
        }
        GenerateObjects();
    }

    void GenerateObjects()
    {
        GameObject obj;
        foreach (Vector3 v in visibleVoxels)
        {
            int x = (int) v.x;
            int y = (int) v.y;
            int z = (int) v.z;
            int mapVal = map.mapMatrix[x, y, z];
            // If voxel already is created, just set it to active and continue
            if(createdVoxels[x, y, z] != null)
            {
                createdVoxels[x, y, z].SetActive(true);
                continue;
            }
            // If the voxel doesn't exist create a new one with the given location
            obj = spawnObj(map.materialMakeUp[mapVal], x, y);
            // obj.transform.parent = Map.materialHolders[mapVal].transform;

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
        obj.transform.parent = this.transform;
        return obj;
    }
}
