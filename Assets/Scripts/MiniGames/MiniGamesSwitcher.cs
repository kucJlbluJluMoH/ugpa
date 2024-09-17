using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGamesSwitcher : MonoBehaviour
{
    public GameObject blurUI;
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

    private List<GameObject> miniGames;

    void Start()
    {
        miniGames = new List<GameObject>()
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
        ShowMiniGame(0);
    }

    public void ShowMiniGame(int gameIndex)
    {
        // Hide all mini-games
        HideMiniGames();
        blurUI.SetActive(true);
        // Ensure the index is within bounds
        if (gameIndex >= 0 && gameIndex < miniGames.Count)
        {
            // Show the specified mini-game
            miniGames[gameIndex].SetActive(true);
        }
        else
        {
            Debug.LogError("Game index out of bounds: " + gameIndex);
        }
    }
    
    public void HideMiniGames()
    {
        // Hide all mini-game GameObjects
        foreach (var miniGame in miniGames)
        {
            miniGame.SetActive(false);
        }
        blurUI.SetActive(false);
    }
}


