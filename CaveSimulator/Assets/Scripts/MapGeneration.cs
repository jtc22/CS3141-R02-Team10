using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] int depth;
    [SerializeField] GameObject stone;
    [SerializeField] GameObject limestone;

    // Start is called before the first frame update
    void Start()
    {
        Generation();
    }

    // Update is called once per frame
    void Generation()
    {
        // TODO generate Z dimension
        for (int x = -width; x < width; x++)
        {
            for (int y = -height; y < height; y++)
            {
                if(Random.Range(0,3) / 1 == 0)
                {
                    spawnObj(limestone, x/10f, y/10f);
                } else {
                    spawnObj(stone, x/10f, y/10f);
                }
            }
        }
    }

    void spawnObj(GameObject obj, float width, float height, float depth = 0)
    {
        obj = Instantiate(obj, new Vector3(width, height, depth), Quaternion.identity);
        obj.transform.parent = this.transform;
    }
}
