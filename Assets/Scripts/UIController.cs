using JetBrains.Annotations;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
    public MiniGamesSwitcher miniGamesSwitcher;
    public GameObject developerMenu;
    public GameObject blurUI;
    [FormerlySerializedAs("DedEffectobj")] public GameObject dedEffectobj;
    [FormerlySerializedAs("DeadEffect")] public Image deadEffect; // ������ �� �����������
    public GameObject deadScreen; // ������ �� ������ DeadScreen
    public GameObject deadScreen1; // ������ �� ������ DeadScreen
    public GameObject deadScreen2; // ������ �� ������ DeadScreen
    public float fadeSpeed = 2f; // �������� ���������
    public SetGraphics setGraphics;
    private int _currentGraphicIndex = 2;
    [FormerlySerializedAs("PauseMenu")] public GameObject pauseMenu; 
    public Slider slider;
    public GameObject crosshair;
    private PlayerMovement _playerMovement;
    private CameraController _camera => CameraController.Instance;
    private float _currentAlpha = 0f; // ������� ��������������


    private bool _isPanelActive = false;
    
    
    
    

    
    
    
    void Start()
    {
     

        dedEffectobj.SetActive(false);
        _playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        // ������������� �������� �������� �� ���������
        slider.value = 400;
        // �������� ������ ��� ������
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        _camera.mouseSensitivity = slider.value;

        _camera.LockCursor();

    }

    public void CloseApp()
    {
        Application.Quit();
    }
    public void RestartLevel()
    {
        Time.timeScale = 1;
        _camera.LockCursor();
        SceneManager.LoadScene(1);
    }
    public void Update()
    {
        if(_playerMovement.hp<=0)
        {
            _playerMovement.hp = 0;
            dedEffectobj.SetActive(true);
            _currentAlpha += fadeSpeed * Time.deltaTime;

            // ������������ �������������� ��������� 1
            _currentAlpha = Mathf.Clamp(_currentAlpha, 0f, 1f);

            // ��������� ���� �����������
            deadEffect.color = new Color(deadEffect.color.r, deadEffect.color.g, deadEffect.color.b, _currentAlpha);

            // ���������, �������� �� �������������� 1
            if (_currentAlpha >= 1f)
            {
                // �������� ������ DeadScreen
                deadScreen.SetActive(true);
                deadScreen1.SetActive(true);
                deadScreen2.SetActive(true);
                _camera.UnlockCursor();
                Time.timeScale = 0;

            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //setGraphics.quality.value = currentGraphicIndex;
                _currentGraphicIndex = setGraphics.quality.value;
                if (!miniGamesSwitcher.isInGame && !developerMenu.activeSelf)
                {
                    _isPanelActive = !_isPanelActive;
                    pauseMenu.SetActive(_isPanelActive);
                    crosshair.SetActive(!_isPanelActive);
                    if (!_isPanelActive)
                    {
                        
                        Time.timeScale = 1;
                        blurUI.SetActive(false);
                        _camera.mouseSensitivity = slider.value;
                        _camera.isPaused = false;
                        _camera.LockCursor();

                    }
                    else
                    {

                    
                            _camera.isPaused = true;
                            Time.timeScale = 0;
                            blurUI.SetActive(true);
                            _camera.UnlockCursor();
                        
                    }
                }
                else
                {
                    if (developerMenu.activeSelf)
                    {
                        developerMenu.SetActive(false);
                        Time.timeScale = 1;
                        blurUI.SetActive(false);
                        _camera.mouseSensitivity = slider.value;
                        _camera.isPaused = false;
                        _camera.LockCursor();
                    }

                    if (miniGamesSwitcher.isInGame )
                
                    {
                        miniGamesSwitcher.HideMiniGames();
                        Time.timeScale = 1;
                        blurUI.SetActive(false);
                        _camera.mouseSensitivity = slider.value;
                        _camera.isPaused = false;
                        _camera.LockCursor();
                    }
                    
                }
            }
        }
        // ��������� ������� ������� Esc
      
        
    }
    
}
