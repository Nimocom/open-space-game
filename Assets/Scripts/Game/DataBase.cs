using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;

public static class DataBase
{
    public static Sprite[] allCharacterAvatars;
    public static Character[] allCharacters;
    public static StarSystem[] allSystems;
    //public static Item[] allItemplates;

    public static Dictionary<int, Module> moduleTemplates = new Dictionary<int, Module>();

    public static Dictionary<int, HullPart> hullParts = new Dictionary<int, HullPart>();
    public static Dictionary<int, HullBase> hullBases = new Dictionary<int, HullBase>();

    public static Dictionary<int, HullPart> cabineParts = new Dictionary<int, HullPart>();
    public static Dictionary<int, HullPart> frontParts = new Dictionary<int, HullPart>();
    public static Dictionary<int, HullPart> rearParts = new Dictionary<int, HullPart>();
    public static Dictionary<int, HullPart> wingParts = new Dictionary<int, HullPart>();
 
    public static Dictionary<int, GameObject> vertexerWaves = new Dictionary<int, GameObject>();

    public static Dictionary<int, Rocket> rocketShells = new Dictionary<int, Rocket>();

//    public static Dictionary<int, Shell> smallShells = new Dictionary<int, Shell>();
//    public static Dictionary<int, Shell> mediumShells = new Dictionary<int, Shell>();
//    public static Dictionary<int, Shell> bigShells = new Dictionary<int, Shell>();

    public static Dictionary<int, Shell> gunShells = new Dictionary<int, Shell>();

    public static EnergyGenerator energyGenerator;
    public static RepairDroid repairDroid;
    public static Engine engine;
    public static Shield shield;

    public static Hull hullTemplate;

    public static RocketLauncher rocketLauncher;
    public static Vertexer vertexer;
    public static Blaster blaster;
    public static Laser laser;

    public static Character mainPlayer;
    public static Character military;
    public static Character pirate;
    public static Character empty;

    public static ItemContainer itemContainer;

    public static void LoadData()
    {
        itemContainer = Resources.Load<ItemContainer>("Other/World/ItemContainer");

        allCharacterAvatars = Resources.LoadAll<Sprite>("Other/Avatars");
        allCharacters = Resources.LoadAll<Character>("Other/Characters");
        allSystems = Resources.LoadAll<StarSystem>("Other/Systems");
        //allItemplates = Resources.LoadAll<Item>("Items");

        energyGenerator = Resources.Load<EnergyGenerator>("Modules/Devices/EnergyGenerator");
        repairDroid = Resources.Load<RepairDroid>("Modules/Devices/RepairDroid");
        engine = Resources.Load<Engine>("Modules/Devices/Engine");
        shield = Resources.Load<Shield>("Modules/Devices/Shield");

        rocketLauncher = Resources.Load<RocketLauncher>("Modules/Weapons/RocketLauncher");
        vertexer = Resources.Load<Vertexer>("Modules/Weapons/Vertexer");
        blaster = Resources.Load<Blaster>("Modules/Weapons/Blaster");
        laser = Resources.Load<Laser>("Modules/Weapons/Laser");



        mainPlayer = Resources.Load<Character>("Other/Characters/MainPlayer");
        military = Resources.Load<Character>("Other/Characters/Military");
        pirate = Resources.Load<Character>("Other/Characters/Pirate");

        itemContainer = Resources.Load<ItemContainer>("Other/World/ItemContainer");
        hullTemplate = Resources.Load<Hull>("Hull/HullTemplate");

        HullPart[] hullPartsArray = Resources.LoadAll<HullPart>("Hull/Parts");
        HullBase[] hullBasesArray = Resources.LoadAll<HullBase>("Hull/Bases");
        Module[] modulesArray = Resources.LoadAll<Module>("Modules");

        Rocket[] rocketsArray = Resources.LoadAll<Rocket>("Shells/Rocket");
        Shell[] gunShellsArray = Resources.LoadAll<Shell>("Shells/Gun");


//        Shell[] smallShellsArray = Resources.LoadAll<Shell>("Shells/Small");
//        Shell[] mediumShellsArray = Resources.LoadAll<Shell>("Shells/Medium");
//        Shell[] bigShellsArray = Resources.LoadAll<Shell>("Shells/Big");
//
//        Rocket[] rocketShellsArray = Resources.LoadAll<Rocket>("Shells/Rockets");
//
//        for(int i = 0; i < rocketShellsArray.Length; i++)
//            rocketShells.Add(rocketShellsArray[i].rocketShellID, rocketShellsArray[i]);
//
//        for (int i = 0; i < smallShellsArray.Length; i++)
//        {
//            smallShells.Add(smallShellsArray[i].shellID, smallShellsArray[i]);
//            shells.Add(smallShellsArray[i].shellID, smallShellsArray[i]);
//        }
//        for (int i = 0; i < mediumShellsArray.Length; i++)
//        {
//            mediumShells.Add(mediumShellsArray[i].shellID, mediumShellsArray[i]);
//            shells.Add(mediumShellsArray[i].shellID, mediumShellsArray[i]);
//        }
//        for (int i = 0; i < bigShellsArray.Length; i++)
//        {
//            bigShells.Add(bigShellsArray[i].shellID, bigShellsArray[i]);
//            shells.Add(bigShellsArray[i].shellID, bigShellsArray[i]);
//        }


        for (int i = 0; i < modulesArray.Length; i++)
            moduleTemplates.Add(modulesArray[i].templateID, modulesArray[i]);
        
        for (int i = 0; i < hullPartsArray.Length; i++)
            hullParts.Add(hullPartsArray[i].partID, hullPartsArray[i]);
        for (int i = 0; i < hullBasesArray.Length; i++)
            hullBases.Add(hullBasesArray[i].baseID, hullBasesArray[i]);

        for (int i = 0; i < rocketsArray.Length; i++)
            rocketShells.Add(rocketsArray[i].rocketShellID, rocketsArray[i]);
        for (int i = 0; i < gunShellsArray.Length; i++)
            gunShells.Add(gunShellsArray[i].shellID, gunShellsArray[i]);

        foreach (var part in hullParts.Values)
        {
            if (part.partType == HullPart.PartType.FrontPart)
                frontParts.Add(part.partID, part);
            else if (part.partType == HullPart.PartType.RearPart)
                rearParts.Add(part.partID, part);
            else if (part.partType == HullPart.PartType.Cabine)
                cabineParts.Add(part.partID, part);
            else
                wingParts.Add(part.partID, part);
        }



//        foreach (var item in allItemplates)
//        {
//            if (item.itemType == Item.ItemType.Module)
//                moduleTemplates.Add(item.templateID, item as Module);
//            else
//                hullTemplates.Add(item.templateID, item as Hull);
//        }

//        foreach (var module in moduleTemplates.Values)
//        {
//            if (module.moduleType == Module.ModuleType.Device)
//                deviceTemplates.Add(module as Device);
//            else
//                weaponTemplates.Add(module as Weapon);
//        }
//
//        rocketLaunchers.AddRange(weaponTemplates.Where(x => x.weaponType == Weapon.WeaponType.RocketLauncher).Cast<RocketLauncher>());
//        weaponTemplates.RemoveAll(x => x.weaponType == Weapon.WeaponType.RocketLauncher);

//        foreach (var item in devices)
//        {
//            
//            if (item.deviceType == Device.DeviceType.EnergyGenerator)
//                energyGenerators.Add(item as EnergyGenerator);
//            else if (item.deviceType == Device.DeviceType.RepairDroid)
//                repairDroids.Add(item as RepairDroid);
//            else if (item.deviceType == Device.DeviceType.Engine)
//                engines.Add(item as Engine);
//            else 
//                shields.Add(item as Shield);
//        }
    }
}
