using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/MapData")]
public class MapData : ScriptableObject
{
    public int width = 800;
    public int height = 600;
    public int depth = 30;
    public int age = 450;
    public int waterLevel = 600/3;
    public float waveForce = 1.0f;

}
