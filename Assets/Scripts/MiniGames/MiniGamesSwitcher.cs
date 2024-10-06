using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MiniGamesSwitcher : MonoBehaviour
{
    public CameraController cameraController;
    public bool isInGame = false;
    public bool isWaitingAction;
    public GameObject blurUI;
    public GameObject gameCanvas;
    public GameObject miniGame0;
    public GameObject miniGame1;
    public GameObject miniGame2;
    public GameObject miniGame3;
    public GameObject miniGame4;
    public GameObject miniGame5;
    public GameObject miniGame6;
    public GameObject miniGame7;
    public GameObject miniGame8;
    public GameObject miniGame9;
    public GameObject miniGame10;
    public GameObject miniGame11;
    public GameObject miniGame12;
    public GameObject miniGame13;
    public GameObject miniGame14;
    public GameObject miniGame15;
    public GameObject miniGame16;
    [FormerlySerializedAs("WaitingMiniGameID")] public int waitingMiniGameID=-1;
    [FormerlySerializedAs("PassedMiniGameID")] public int passedMiniGameID=-1;
    [FormerlySerializedAs("MaxIdOfOppenedDoor")] public int maxIdOfOppenedDoor = -1;
    private List<GameObject> _miniGames;

    public static MiniGamesSwitcher Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

     void Start()
    {
        _miniGames = new List<GameObject>()
        {
            miniGame0,
            miniGame1,
            miniGame2,
            miniGame3,
            miniGame4,
            miniGame5,
            miniGame6,
            miniGame7,
            miniGame8,
            miniGame9,
            miniGame10,
            miniGame11,
            miniGame12,
            miniGame13,
            miniGame14,
            miniGame15,
            miniGame16
        };
        HideMiniGames(); // Initially hide all mini-games

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HideMiniGames();
            isWaitingAction = false;
        }
    }

    public void ShowMiniGame(int gameIndex)
    {
        waitingMiniGameID = gameIndex;
        // Hide all mini-games
        HideMiniGames();
        cameraController.UnlockCursor();
        cameraController.isPaused = true;
        isInGame = true;
        isWaitingAction = true;
        gameCanvas.SetActive(false);
        blurUI.SetActive(true);
        // Ensure the index is within bounds
        if (gameIndex >= 0 && gameIndex < _miniGames.Count)
        {
            // Show the specified mini-game
            _miniGames[gameIndex].SetActive(true);
        }
        else
        {
            Debug.LogError("Game index out of bounds: " + gameIndex);
        }
    }
    
    public void HideMiniGames()
    {
        
        
        cameraController.LockCursor();
        cameraController.isPaused = false;
        isInGame = false;
        gameCanvas.SetActive(true);
        isWaitingAction = false;
        // Hide all mini-game GameObjects
        foreach (var miniGame in _miniGames)
        {
            if (miniGame != null)
            {
                miniGame.SetActive(false);
            }
        }
        blurUI.SetActive(false);
    }
}


