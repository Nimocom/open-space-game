using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour 
{
    public int playerLevel;

	void Start () 
    {
        //GenerateShip(Character.Fraction.Player);
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            GameManager.currentSystemPlayers.Add(GenerateShip(Character.Fraction.Military));
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            GameManager.currentSystemPlayers.Add(GenerateShip(Character.Fraction.Pirate));
                
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
            EnergyGenerator.GenerateEnergyGenerator(10).LoadToCargoHold(PlayerController.inst.hull);
        if (Input.GetKeyDown(KeyCode.Keypad2))
            Engine.GenerateEngine(10).LoadToCargoHold(PlayerController.inst.hull);
        if (Input.GetKeyDown(KeyCode.Keypad3))
            RepairDroid.GenerateRepairDroid(10).LoadToCargoHold(PlayerController.inst.hull);
        if (Input.GetKeyDown(KeyCode.Keypad4))
            Shield.GenerateShield(10).LoadToCargoHold(PlayerController.inst.hull);
        if (Input.GetKeyDown(KeyCode.Keypad5))
            Blaster.GenerateBlaster(10).LoadToCargoHold(PlayerController.inst.hull);
        if (Input.GetKeyDown(KeyCode.Keypad6))
            Laser.GenerateLaser(10).LoadToCargoHold(PlayerController.inst.hull);
        if (Input.GetKeyDown(KeyCode.Keypad7))
            RocketLauncher.GenerateRocketLauncher(10).LoadToCargoHold(PlayerController.inst.hull);
    }

    Character GenerateShip(Character.Fraction fraction)
    {
        Hull hull = Hull.GenerateHull(10);
        Character character = null;

        if (fraction == Character.Fraction.Pirate)
            hull.InitializeHull(character = Instantiate(DataBase.pirate));
        else if (fraction == Character.Fraction.Military)
            hull.InitializeHull(character = Instantiate(DataBase.military));
        else
        {
            hull.InitializeHull(character = Instantiate(DataBase.mainPlayer));
            GameManager.InitializeMainPlayer(hull);
        }

        EnergyGenerator.GenerateEnergyGenerator(10).InstallModule(hull);
        RepairDroid.GenerateRepairDroid(10).InstallModule(hull);
        Engine.GenerateEngine(10).InstallModule(hull);
        Shield.GenerateShield(10).InstallModule(hull);

        Blaster.GenerateBlaster(10).InstallModule(hull, 0);
        Blaster.GenerateBlaster(10).InstallModule(hull, 1);
        Laser.GenerateLaser(10).InstallModule(hull, 2);
        //RocketLauncher.GenerateRocketLauncher(10).InstallModule(hull, 3);

        hull.transform.position = new Vector2(Random.Range(-30f, 30f), Random.Range(-50f, 50f));

        return character;
    }
}
