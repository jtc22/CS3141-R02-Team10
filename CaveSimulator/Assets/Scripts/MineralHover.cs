using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralHover : MonoBehaviour
{
    SpriteRenderer sr;
    Sprite s;
    Texture2D tex;
    Collider2D col2d;

    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
        col2d = this.GetComponent<BoxCollider2D>();
        s = sr.sprite;
        tex = s.texture;

    }

    public void MouseOver()
    {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Color c = tex.GetPixel((int)mousePos.x, (int)mousePos.y);
            if(c.a == 0)
            {
                sr.color = new Color(1, 1, 1, 1);
            }
            else
            {
                MaterialProperties.Material mat = (MaterialProperties.Material) System.Enum.Parse(typeof(MaterialProperties.Material), col2d.transform.name.ToLower());
                MaterialProperties.MaterialProperty matProp = MaterialProperties.getMaterialProperties(mat);
                string data = "Current Mineral: " + matProp.name + "\n" +
                                "Makeup: " + (getPercentage() * 100).ToString("F2") + "%\n" +
                                "Hardness: " + matProp.hardness + "\n" +
                                "Density: " + matProp.density + "\n" +
                                "Soluability: " + matProp.solubility + "\n";
                MaterialDataUI.ShowMineralData(data);
                sr.color = new Color(0.9f, .9f, 0.9f, 1);
            }
                
    }

    // TODO rework this, probably should be calculated in the map class for better accuracy and faster speeds
    public float getPercentage()
    {
        Color32[] pixs = tex.GetPixels32();
        int count = 0;
        foreach(Color32 pixel in pixs)
        {
            if(pixel.a > 0) count++;
        }

        return (float) count / (tex.height * tex.width);

    }

}
