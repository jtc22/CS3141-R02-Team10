using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{

    [SerializeField] public static GameObject[] makeUp;
    [SerializeField] public GameObject[] mapMaterials;
    public static int[,,] map = new int[100, 100, 20];
    public static GameObject[] materialHolders;


    // Start is called before the first frame update
    void Start()
    {
        materialHolders = new GameObject[mapMaterials.Length];
        for (int i = 0; i < materialHolders.Length; i++)
        {
            materialHolders[i] = new GameObject("Material" + i);
            materialHolders[i].transform.parent = this.transform;
        }
        makeUp = mapMaterials;
    }
}
