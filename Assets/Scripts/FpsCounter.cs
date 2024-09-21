using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FpsCounter : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI fpstxt;
    private float _pollingTime = 0.1f; // Обновлять значение FPS каждую секунду
    private float _timePassed;
    private int _frameCount;

    void Update()
    {
        // Подсчет FPS
        _timePassed += Time.deltaTime;
        _frameCount++;

        if (_timePassed >= _pollingTime)
        {
            int fps = Mathf.RoundToInt(_frameCount / _timePassed);
            fpstxt.text = ""+fps;

            _frameCount = 0;
            _timePassed -= _pollingTime;
        }
    }
    

}
