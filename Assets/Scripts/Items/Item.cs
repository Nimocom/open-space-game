using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour 
{
    public enum ItemType
    {
        Module,
        Hull
    }

    public Item.ItemType itemType;
    public Sprite sprite;

    public int templateID;
    public float weight;
    public float health;
    public int level;
    public int cost;


    public virtual List<string> GetSerializableData()
    {
        List<string> serializedData = new List<string>();

        ItemData itemData = new ItemData()
        {
            templateID = this.templateID,
            health = this.health,
            weight = this.weight,
            level = this.level,
            name = this.name,
            cost = this.cost          
        };
        
        serializedData.Add(JsonUtility.ToJson(itemData));
        return serializedData;
    }

    public virtual void SetSerializableData(List<string> data)
    {
        ItemData itemData = JsonUtility.FromJson<ItemData>(data[data.Count - 1]);

        templateID = itemData.templateID;
        health = itemData.health;
        weight = itemData.weight;
        level = itemData.level;
        name = itemData.name;
        cost = itemData.cost;

        data.Remove(data[data.Count - 1]);
    }

    public override string ToString()
    {
        return /*LocaManager.textUnits["name"] + name + "\n" + */
            LocaManager.textUnits["weight"] + weight.ToString("0.00") + "\n" + 
            LocaManager.textUnits["health"] + health.ToString("0.00") + "\n" + 
            LocaManager.textUnits["level"] + level + "\n" + 
            LocaManager.textUnits["cost"] + cost + "\n";
    }

    public static Item GenerateItem(int level)
    {
        int type = Random.Range(0, 2);

        if (type == 0)
            return Module.GenerateModule(level);
        else 
            return Hull.GenerateHull(level);

    }
}
