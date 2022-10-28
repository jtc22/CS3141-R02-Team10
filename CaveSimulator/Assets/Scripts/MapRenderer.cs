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
    private GameObject[] typeHolders;

    // Start is called before the first frame update
    void Start()
    {
        map = new Map(300, 200, 1, new GameObject[3] {stone, limeStone, air});
        currDepth = map.depth / 2;

        createdVoxels = new GameObject[map.width, map.height, map.depth];

        typeHolders = new GameObject[map.numMaterials()];

        // Create the gameobjects requires to hold the textures of the generated map
        for(int i = 0; i < map.numMaterials(); i++)
        {
            typeHolders[i] = new GameObject("Material"); // + map.materialMakeUp[i].name);
            typeHolders[i].transform.position = new Vector3(map.width/2, map.height/2, 0);
            typeHolders[i].transform.parent = this.transform;
            typeHolders[i].AddComponent<SpriteRenderer>();
            typeHolders[i].AddComponent<MineralHover>();
            typeHolders[i].layer = i + 10;
        }

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

        for (int i = 0; i < map.numMaterials(); i++)
        {
            SpriteRenderer sr = typeHolders[i].GetComponent<SpriteRenderer>();
            Texture2D tex = map.materialTextureLayers[z, i];
            sr.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 1.0f);
            typeHolders[i].AddComponent<BoxCollider2D>();
        }
    }

}
