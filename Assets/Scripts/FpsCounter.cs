using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FpsCounter : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI fpstxt;
    private float pollingTime = 0.1f; // Обновлять значение FPS каждую секунду
    private float timePassed;
    private int frameCount;

    void Update()
    {
        // Подсчет FPS
        timePassed += Time.deltaTime;
        frameCount++;

        if (timePassed >= pollingTime)
        {
            int fps = Mathf.RoundToInt(frameCount / timePassed);
            fpstxt.text = ""+fps;

            frameCount = 0;
            timePassed -= pollingTime;
        }
    }

}
