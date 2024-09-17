using JetBrains.Annotations;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
    public GameObject blurUI;
    public GameObject DedEffectobj;
    public Image DeadEffect; // ������ �� �����������
    public GameObject deadScreen; // ������ �� ������ DeadScreen
    public GameObject deadScreen1; // ������ �� ������ DeadScreen
    public GameObject deadScreen2; // ������ �� ������ DeadScreen
    public float fadeSpeed = 2f; // �������� ���������
    public SetGraphics setGraphics;
    private int currentGraphicIndex = 2;
    public GameObject PauseMenu; 
    public Slider slider;
    public GameObject crosshair;
    private PlayerMovement playerMovement;
    private CameraController camera;
    private float currentAlpha = 0f; // ������� ��������������


    private bool isPanelActive = false;
    
    
    
    

    
    
    
    void Start()
    {
     

        DedEffectobj.SetActive(false);
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        // ������������� �������� �������� �� ���������
        slider.value = 400;
        // �������� ������ ��� ������
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
        camera.mouseSensitivity = slider.value;

        camera.LockCursor();

    }

    public void CloseApp()
    {
        Application.Quit();
    }
    public void RestartLevel()
    {
        Time.timeScale = 1;
        camera.LockCursor();
        SceneManager.LoadScene(1);
    }
    public void Update()
    {
        if(playerMovement.HP<=0)
        {
            DedEffectobj.SetActive(true);
            currentAlpha += fadeSpeed * Time.deltaTime;

            // ������������ �������������� ��������� 1
            currentAlpha = Mathf.Clamp(currentAlpha, 0f, 1f);

            // ��������� ���� �����������
            DeadEffect.color = new Color(DeadEffect.color.r, DeadEffect.color.g, DeadEffect.color.b, currentAlpha);

            // ���������, �������� �� �������������� 1
            if (currentAlpha >= 1f)
            {
                // �������� ������ DeadScreen
                deadScreen.SetActive(true);
                deadScreen1.SetActive(true);
                deadScreen2.SetActive(true);
                camera.UnlockCursor();
                Time.timeScale = 0;
                
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //setGraphics.quality.value = currentGraphicIndex;
                currentGraphicIndex = setGraphics.quality.value;
                isPanelActive = !isPanelActive;
                PauseMenu.SetActive(isPanelActive);
                crosshair.SetActive(!isPanelActive);
                if (!isPanelActive)
                {
                    Time.timeScale = 1;
                    blurUI.SetActive(false);
                    camera.mouseSensitivity = slider.value;
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
        // ��������� ������� ������� Esc
      
        
    }
    
}
