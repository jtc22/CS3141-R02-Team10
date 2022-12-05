using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MaterialProperties 
{
    public enum Material {
        limestone,
        dolomite,
        basalt,
        sandstone,
        granite,
        water,
        air
    }

    public struct MaterialProperty {
            public string name;
            public float density;
            public float hardness;
            public float solubility;
            public Color color;
    }

    public static MaterialProperty getMaterialProperties(Material material){
            MaterialProperty prop = new MaterialProperty();
            switch (material)
            {
                case (Material.basalt):
                    prop.name = "Basalt";
                    prop.density = 3f;
                    prop.hardness = 5.5f;
                    prop.solubility = 1;
                    prop.color = new Color(.380f, .412f, .424f, 1);
                    return prop;
                case (Material.limestone):
                    prop.name = "Limestone";
                    prop.density = 2f; // 1.5-2.71 g/cm^3
                    prop.hardness = 3.5f;
                    prop.solubility = 1;
                    prop.color = new Color(0.74f, 0.74f, 0.56f, 1);
                    return prop;
                case (Material.sandstone):
                    prop.name = "Sandstone";
                    prop.density = 2.3f; // 2.0-2.6
                    prop.hardness = 6.5f;
                    prop.solubility = 1;
                    prop.color = new Color(.698f, .565f, .510f, 1);
                    return prop;
                case (Material.dolomite):
                    prop.name = "Dolomite";
                    prop.density = 2.84f; // Avg
                    prop.hardness = 3.75f;
                    prop.solubility = 1;
                    prop.color = new Color(.714f, .702f, .671f, 1);
                    return prop;
                case (Material.granite):
                    prop.name = "Granite";
                    prop.density = 2.75f; // 2.7-2.8
                    prop.hardness = 7f;
                    prop.solubility = 1;
                    prop.color = new Color(.404f, .404f, .404f, 1);
                    return prop;
                case (Material.water):
                    prop.name = "Water";
                    prop.density = 1.0f;
                    prop.hardness = 1;
                    prop.solubility = 1;
                    prop.color = new Color(0.1f, 0.1f, 0.8f, 1);
                    return prop;
                case (Material.air):
                    prop.name = "Air";
                    prop.density = 1f;
                    prop.hardness = 1;
                    prop.solubility = 1;
                    prop.color = new Color(0.9f, 0.9f, 1.0f, 1);
                    return prop;
                default:
                    prop.name = "nothing";
                    prop.density = 1f;
                    prop.hardness = 1;
                    prop.solubility = 1;
                    prop.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
                    return prop;
            }
        }
}