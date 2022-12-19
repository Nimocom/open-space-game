using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;

public static class GameManager
{
    public static SystemSerializeData currentSystemData;
    public static CurrentPlanetData currentPlanetData;
    public static List<Character> currentSystemPlayers = new List<Character>();
    public static List<Planet> currentSystemPlanets = new List<Planet>();
    public static List<ItemContainer> currentSystemContainers = new List<ItemContainer>();

    static StarSystem currentStarSystem;

    public static void SetActivePlanet(Planet planet)
    {
        currentPlanetData = new CurrentPlanetData();
        currentPlanetData.planetName = planet.name;
        currentPlanetData.population = planet.population;
        currentPlanetData.textures = planet.planetTextures;
    }

    public static void LoadGame()
    {
        currentSystemPlayers.Clear();
        currentSystemPlanets.Clear();
        string jsonString =  File.ReadAllText(Application.streamingAssetsPath + "/SavedData.json");
        currentSystemData = JsonUtility.FromJson<SystemSerializeData>(jsonString);
        SceneManager.LoadSceneAsync(currentSystemData.sceneIndex);
    }

    public static void SaveGame()
    {
        currentSystemData = new SystemSerializeData();
        currentSystemData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        currentSystemData.systemIndex = StarSystem.instance.GetSystemIndex();
        SaveNPC();
        SaveMainPlayer();
        SaveContainers();
        SavePlanets();

        string json = JsonUtility.ToJson(currentSystemData);


        File.WriteAllText(Application.streamingAssetsPath + "/SavedData.json", json);
        System.GC.Collect();
        UIManager.inst.ShowGameSavedText();
    }

    public static void InitializeSpaceEnvironment()
    {
        LoadSystem();
        LoadPlanets();
        LoadContainers();
        LoadMainPlayer();
        LoadNPC();
        System.GC.Collect();
        //Resources.UnloadUnusedAssets();
    }

    public static void InitializePlanetEnvironment()
    {

    }

    public static void LoadSystem()
    {
        currentStarSystem = Object.Instantiate(DataBase.allSystems.FirstOrDefault(x => x.GetSystemIndex() == currentSystemData.systemIndex));
        RenderSettings.skybox = currentStarSystem.GetSkyboxMaterial();
    }

    public static void LoadPlanets()
    {
        Planet[] planets = GameObject.FindObjectsOfType<Planet>();

        foreach (var item in currentSystemData.planetTransforms)
        {
            Planet planet = planets.Single(x => x.planetIndex == item.planetIndex);
            planet.transform.position = item.position;
            planet.transform.rotation = item.rotation;
            currentSystemPlanets.Add(planet);
        }
    }

    public static void LoadContainers()
    {
        foreach (var item in currentSystemData.containers)
        {
            ItemContainer container = Object.Instantiate<ItemContainer>(DataBase.itemContainer, item.position, item.rotation);
            Module module = Object.Instantiate<Module>(DataBase.moduleTemplates[item.moduleData.templateID]);
            module.SetSerializableData(item.moduleData.stringList);
            container.InitializeLoot(module);
            currentSystemContainers.Add(container);
        }
    }

    public static void LoadMainPlayer()
    {
        Character player = DeserializePlayer(currentSystemData.mainPlayer);
        currentSystemPlayers.Add(player);
        InitializeMainPlayer(player.hull);
    }

    public static void LoadNPC()
    {
        foreach (var item in currentSystemData.npc)
        {
            currentSystemPlayers.Add(DeserializePlayer(item));
        }
    }


    public static void SaveNPC()
    {
        foreach (var npc in GameObject.FindObjectsOfType<AIController>())
        {
            currentSystemData.npc.Add(SerializePlayer(npc.character));
        }
    }

    public static void SaveMainPlayer()
    {
        currentSystemData.mainPlayer = SerializePlayer(PlayerController.inst.character);
    }

    public static void SaveContainers()
    {
        ContainerSerializableData containerData;
        foreach (var container in GameObject.FindObjectsOfType<ItemContainer>())
        {
            
            containerData = new ContainerSerializableData();
            containerData.moduleData.templateID = container.module.templateID;
            containerData.moduleData.stringList = container.module.GetSerializableData();
            containerData.position = container.transform.position;
            containerData.rotation = container.transform.rotation;
            currentSystemData.containers.Add(containerData);
        }
    }

    public static void SavePlanets()
    {
        PlanetSerializableData planetData;
        foreach (var item in currentSystemPlanets)
        {
            planetData = new PlanetSerializableData();
            planetData.planetIndex = item.planetIndex;
            planetData.position = item.transform.position;
            planetData.rotation = item.transform.rotation;

            currentSystemData.planetTransforms.Add(planetData);
        }
    }

    public static PlayerSerializableData SerializePlayer(Character player)
    {
        PlayerSerializableData characterData = new PlayerSerializableData();
        characterData.characterData = player.GetCharacterData();
        characterData.shipData.stringList = player.hull.GetSerializableData();
        characterData.shipData.templateID = player.hull.templateID;

        return characterData;
    }

    public static Character DeserializePlayer(PlayerSerializableData data)
    {
        Character character = Object.Instantiate<Character>(DataBase.allCharacters[(int)data.characterData.fraction]);
        character.SetCharacterData(data.characterData);

        Hull hull = Object.Instantiate(DataBase.hullTemplate);
        hull.InitializeHull(character);
        hull.SetSerializableData(data.shipData.stringList);
        return character;
    }

    public static void InitializeMainPlayer(Hull hull)
    {
        PlayerController.inst.InitializeController(hull);
        CameraController.inst.InitializeCamera(hull);
        MapCanvasController.inst.InitializeMap(hull);
        UIManager.inst.InitializeStatusBars(hull);
    }
}

