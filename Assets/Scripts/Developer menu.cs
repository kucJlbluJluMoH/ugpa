using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Developermenu : MonoBehaviour
{
    public GameObject blurUI;
    public GameObject developerMenu;
    public GameObject spawn;

    public MiniGamesSwitcher miniGamesSwitcher;
    public CameraController camera;
    public PlayerMovement playerMovement;
    public Image spawnImage;
    public Image killEverybodyImage;
    public Image freezeEnemyImage;
    public Image deathlessImage;
    public Image openAllDoorsImage;

    public int ForceDoorIDOpen=-1;
    public bool isOpennedAllDoors = false;
    public bool isFreezed = false;
    public bool isKilledEverybody  = false;
    void Start()
    {
        developerMenu.SetActive(false);
        miniGamesSwitcher = GameObject.Find("MiniGamesCanvas").GetComponent<MiniGamesSwitcher>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            developerMenu.SetActive(!developerMenu.activeSelf);
            if (!developerMenu.activeSelf)
            {
                Time.timeScale = 1;
                blurUI.SetActive(false);
                camera.IsPaused = false;
                camera.LockCursor();

            }
            else
            {
                camera.IsPaused = true;
                Time.timeScale = 0;
                blurUI.SetActive(true);
                camera.UnlockCursor();
            }
        }
        
    }

    public void KillEverybody()
    {
        if (!isKilledEverybody)
        {
            isKilledEverybody = true;
            Color currentColor = killEverybodyImage.color;
            currentColor.a = 0.6f; // Change this value based on your needs
            killEverybodyImage.color = currentColor; // Apply the updated color
        }
        else
        {
            isKilledEverybody = false;
            Color currentColor = killEverybodyImage.color;
            currentColor.a = 1; // Change this value based on your needs
            killEverybodyImage.color = currentColor; // Apply the updated color
        }
    }
    public void FreezeEnemy()
    {
        if (!isFreezed)
        {
            isFreezed = true;
            Color currentColor = freezeEnemyImage.color;
            currentColor.a = 0.6f; // Change this value based on your needs
            freezeEnemyImage.color = currentColor; // Apply the updated color
        }
        else
        {
            isFreezed = false;
            Color currentColor = freezeEnemyImage.color;
            currentColor.a = 1; // Change this value based on your needs
            freezeEnemyImage.color = currentColor; // Apply the updated color
        }
    }
    public void SpawnOff()
    {
        if (spawn.activeSelf)
        {
            spawn.SetActive(false);
            Color currentColor = spawnImage.color;
            currentColor.a = 0.6f; // Change this value based on your needs
            spawnImage.color = currentColor; // Apply the updated color
        }
        else
        {
            spawn.SetActive(true);
            Color currentColor = spawnImage.color;
            currentColor.a = 1; // Change this value based on your needs
            spawnImage.color = currentColor; // Apply the updated color
        }
        
    }
    public void Deathless()
    {
        if (playerMovement.HP<=100)
        {
            playerMovement.HP = 100000;
            playerMovement.maxHP = 100000;
            Color currentColor = deathlessImage.color;
            currentColor.a = 0.6f; // Change this value based on your needs
            deathlessImage.color = currentColor; // Apply the updated color
        }
        else
        {
            playerMovement.HP = 100;
            playerMovement.maxHP = 100;
            Color currentColor = deathlessImage.color;
            currentColor.a = 1; // Change this value based on your needs
            deathlessImage.color = currentColor; // Apply the updated color
        }
    }
    public void OpenAllDoors()
    {
        if (!isOpennedAllDoors)
        {
            isOpennedAllDoors = true;
            Color currentColor = openAllDoorsImage.color;
            currentColor.a = 0.6f; // Change this value based on your needs
            openAllDoorsImage.color = currentColor; // Apply the updated color
        }
        else
        {
            isOpennedAllDoors = false;
            Color currentColor = openAllDoorsImage.color;
            currentColor.a = 1; // Change this value based on your needs
            openAllDoorsImage.color = currentColor; // Apply the updated color
        }
    }
    public void OpenCurrentDoor()
    {
        ForceDoorIDOpen = miniGamesSwitcher.MaxIdOfOppenedDoor + 1;
    }

    public void GetLogs()
    {
        
    }
}
