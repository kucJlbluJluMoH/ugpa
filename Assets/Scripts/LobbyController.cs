using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LobbyController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Settings()
    {
        
    }
    
    
    void LoadSave()
    {
        SceneManager.LoadScene(1);
    }
    public void NewGame()
    {

        SceneManager.LoadScene(1);
    }
    public void CloseApp()
    {
        Application.Quit();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
