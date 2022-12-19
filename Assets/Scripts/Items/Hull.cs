using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Hull : Item, IDamagable
{
    public Character character;

    public Transform centerSlot;
    public Transform rightSlot;
    public Transform leftSlot;
 
    public Transform cargoHoldContainer;
    public Transform weaponsContainer;
    public Transform devicesContainer;
    public Transform trailContainer;
    public Transform gateWay;

    public GameObject shieldSphere;

    public EnergyGenerator energyGenerator;
    public RepairDroid repairDroid;
    public Engine engine;
    public Shield shield;

    public float maximumCargoPoints;
    public float maximumHealth;

    public Module[] cargoHoldModules = new Module[104];
    public Module[] installedModules = new Module[8];

    public Weapon[] weapons;

    public Rigidbody2D rigBody;

    [SerializeField]    
    Transform shipPartsContainer;
    [SerializeField]
    Transform playerContainer;

    [SerializeField]
    GameObject explosionPrefab;
    [SerializeField]
    HullPartsData partsData;

    void Awake()
    {
        rigBody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rigBody.mass = weight;
    }

    void GenerateLoot()
    {
        Module[] items = GetComponentsInChildren<Module>();
        for (int i = 0; i < items.Length; i++)
            if (Random.Range(0, 6) == 1)
                ThrowOutModule(items[i]);
    }
       
    public void TakeDamage(float hullDamage, float shieldDamage, Character otherCharacter)
    {
        health -= hullDamage;
        if (health <= 0f)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            GenerateLoot();
            GameManager.currentSystemPlayers.Remove(character);
            Destroy(gameObject);
            gameObject.SetActive(false);
        }
        character.SetRelation(otherCharacter.fraction, 0.2f);
    }
        
    public void ThrowOutModule(Module module)
    {
        if (module.isInstalled)
            module.DeinstallModule();
        Instantiate(DataBase.itemContainer, gateWay.position, Random.rotation).InitializeLoot(module, true);
    }

    public void InitializeHull(Character player)
    {
        player.hull = this;

        player.transform.position = transform.position;
        player.transform.rotation = transform.rotation;
        player.transform.parent = playerContainer;

        character = player;
    }

    public override List<string> GetSerializableData()
    {
        List<string> serializedData = base.GetSerializableData();

        ShipData shipData = new ShipData()
        {
            maximumCargoPoints = this.maximumCargoPoints,
            maximumHealth = this.maximumHealth,
            rotation = transform.rotation,
            position = transform.position,
            partsData = this.partsData
        };

        foreach (var module in installedModules.ToList())
        {
            if(module)
            shipData.installedModules.Add(new ItemSerializableData(){ templateID = module.templateID, stringList = module.GetSerializableData() });
        }

        foreach (var module in cargoHoldModules.ToList())
        {
            if(module)
            shipData.cargoHoldModules.Add(new ItemSerializableData(){ templateID = module.templateID, stringList = module.GetSerializableData() });
        }
        
        serializedData.Add(JsonUtility.ToJson(shipData));

        return serializedData;
    }

    public override void SetSerializableData(List<string> data)
    {
        ShipData shipData = JsonUtility.FromJson<ShipData>(data[data.Count -1]);

        maximumCargoPoints = shipData.maximumCargoPoints;
        maximumHealth = shipData.maximumHealth;
        transform.position = shipData.position;
        transform.rotation = shipData.rotation;

        HullBase hullBase = Instantiate(DataBase.hullBases[shipData.partsData.basePartID],shipPartsContainer.transform.position, shipPartsContainer.transform.rotation, shipPartsContainer);

        Instantiate(DataBase.frontParts[shipData.partsData.frontPartID], hullBase.frontPartPoint.position, hullBase.frontPartPoint.rotation, hullBase.frontPartPoint);
        Instantiate(DataBase.rearParts[shipData.partsData.rearPartID], hullBase.rearPartPoint.position, hullBase.rearPartPoint.rotation, hullBase.rearPartPoint);
        Instantiate(DataBase.cabineParts[shipData.partsData.cabinePartID], hullBase.cabinePartPoint.position, hullBase.cabinePartPoint.rotation, hullBase.cabinePartPoint);
        Instantiate(DataBase.wingParts[shipData.partsData.wingPartID], hullBase.wingPartPoint.position, hullBase.wingPartPoint.rotation, hullBase.wingPartPoint);

        partsData = shipData.partsData;

        trailContainer = hullBase.trailContainer;
        centerSlot = hullBase.centerSlor;
        rightSlot = hullBase.rightSlot;
        leftSlot = hullBase.leftSlot;

        foreach (var module in shipData.installedModules)
        {
            var temp = Instantiate(DataBase.moduleTemplates[module.templateID]);
            temp.SetSerializableData(module.stringList);
            temp.InstallModule(this);
        }

        foreach (var module in shipData.cargoHoldModules)
        {
            var temp = Instantiate(DataBase.moduleTemplates[module.templateID]);
            temp.SetSerializableData(module.stringList);
            temp.LoadToCargoHold(this);
        }

        data.Remove(data[data.Count -1]);
        base.SetSerializableData(data);
    }

    public override string ToString()
    {
        return LocaManager.textUnits[itemType.ToString().ToLower()] + "\n" + base.ToString() +
        LocaManager.textUnits["maxhealth"] + maximumHealth.ToString("0.00") + "\n" +
        LocaManager.textUnits["maxchweight"] + maximumCargoPoints.ToString("0.00");
    }

    void OnMouseEnter()
    {
        string infoString = null; 
        infoString += LocaManager.textUnits["name"] + character.name + "\n";
        infoString += LocaManager.textUnits["level"] + character.level + "\n";
        infoString += LocaManager.textUnits["fraction"] + LocaManager.textUnits[character.fraction.ToString().ToLower()] + "\n";
        //infoString += LocaManager.textUnits["hull"] + ": " + this.name + "\n";
        //infoString += LocaManager.textUnits["weight"] + weight.ToString("##.##") + "\n";
        infoString += LocaManager.textUnits["health"] + health.ToString("0.00") + " | " + maximumHealth.ToString("0.00") + "\n";
        if (energyGenerator)
            infoString += LocaManager.textUnits["curenergy"] + energyGenerator.currentEnergy.ToString("0.00") + " | " + energyGenerator.maximumEnergy.ToString("0.00") + "\n";
        if (shield)
            infoString += LocaManager.textUnits["curshield"] + shield.currentShield.ToString("0.00") + " | " + shield.maximumShield.ToString("0.00") + "\n";
        UIManager.inst.ShowInfoPanel(infoString);
    }

    void OnMouseExit()
    {
        UIManager.inst.CloseInfoPanel();
    }

    public static Hull GenerateHull(int level)
    {
        float quality = Random.Range(1.2f, 3f);

        ShipData shipData = new ShipData();
        ItemData itemData = new ItemData();

        Hull hull = Instantiate(DataBase.hullTemplate);

        shipData.partsData = new HullPartsData();
        shipData.partsData.basePartID = DataBase.hullBases.ElementAt(Random.Range(0, DataBase.hullBases.Count)).Value.baseID;

        shipData.partsData.cabinePartID = DataBase.cabineParts.ElementAt(Random.Range(0, DataBase.cabineParts.Count)).Value.partID;
        shipData.partsData.frontPartID = DataBase.frontParts.ElementAt(Random.Range(0, DataBase.frontParts.Count)).Value.partID;
        shipData.partsData.wingPartID = DataBase.wingParts.ElementAt(Random.Range(0, DataBase.wingParts.Count)).Value.partID;
        shipData.partsData.rearPartID = DataBase.rearParts.ElementAt(Random.Range(0, DataBase.rearParts.Count)).Value.partID;

        shipData.maximumCargoPoints = 65f * quality * level;
        shipData.maximumHealth = 160f * quality * level;

        itemData.cost = (int)(45000 * level * quality);
        itemData.weight = Random.Range(76f, 88f) * level;
        itemData.health = shipData.maximumHealth;
        itemData.level = level;

        string shipDataJson = JsonUtility.ToJson(shipData);
        string itemDataJson = JsonUtility.ToJson(itemData);

        List<string> dataList = new List<string>();

        dataList.Add(itemDataJson);
        dataList.Add(shipDataJson);

        hull.SetSerializableData(dataList);

        return hull;
    }
}
