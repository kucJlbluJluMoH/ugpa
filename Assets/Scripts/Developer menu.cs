using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;    

public class Developermenu : MonoBehaviour
{
    public GameObject pauseMenu;
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

    [FormerlySerializedAs("ForceDoorIDOpen")] public int forceDoorIDOpen=-1;
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
                
                developerMenu.SetActive(false);
                if (!pauseMenu.activeSelf)
                {
                    Time.timeScale = 1;
                    blurUI.SetActive(false);
                    camera.isPaused = false;
                    camera.LockCursor();
                }
            }

            
            else
            {
                camera.isPaused = true;
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
            isKilledEverybody = false;

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
        if (playerMovement.hp<=100)
        {
            playerMovement.hp = 100000;
            playerMovement.maxHp = 100000;
            Color currentColor = deathlessImage.color;
            currentColor.a = 0.6f; // Change this value based on your needs
            deathlessImage.color = currentColor; // Apply the updated color
        }
        else
        {
            playerMovement.hp = 100;
            playerMovement.maxHp = 100;
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
        forceDoorIDOpen = miniGamesSwitcher.maxIdOfOppenedDoor + 1;
    }

    public void GetLogs()
    {
        
    }
}
