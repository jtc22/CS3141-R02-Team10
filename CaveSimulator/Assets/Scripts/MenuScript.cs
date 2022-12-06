using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuScript : MonoBehaviour
{
    [SerializeField] private MapData mapData;

    public void ExitSim()
    {
        Application.Quit();
    }

    public void SimCave()
    {
        // Load all data into mapData
        mapData.width = (int)GameObject.Find("Cave_Width_Slider").GetComponent<Slider>().value;
        mapData.height = (int)GameObject.Find("Cave_Height_Slider").GetComponent<Slider>().value;
        mapData.depth = (int)GameObject.Find("Cave_Depth_Slider").GetComponent<Slider>().value;
        mapData.age = (int)(GameObject.Find("Age_Slider").GetComponent<Slider>().value * mapData.width);
        mapData.waterLevel = (int)(GameObject.Find("Water_Level_Slider").GetComponent<Slider>().value * mapData.height);
        mapData.waveForce = GameObject.Find("Wave_Force_Slider").GetComponent<Slider>().value;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
