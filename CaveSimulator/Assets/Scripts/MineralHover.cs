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
                string data = "Current Mineral: " + col2d.transform.name + "\n" +
                                "Makeup: " + (getPercentage() * 100).ToString("F2") + "%\n";
                MaterialDataUI.ShowMineralData(data);
                sr.color = new Color(0.9f, .9f, 0.9f, 1);
            }
                
    }

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
