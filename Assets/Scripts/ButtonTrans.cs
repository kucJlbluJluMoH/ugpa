using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTrans : MonoBehaviour
{

    private Image _img;// Start is called before the first frame update
    void Start()
    {
        _img = GetComponent<Image>();
    }

    IEnumerator FadeOut()
    {
        float duration = 0.1f; // Duration of the fade
        float timeElapsed = 0f;
        Color currentColor = _img.color;

        // Fade from current alpha back to 1 (fully opaque)
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime; // Increment time
            float t = timeElapsed / duration; // Calculate the proportion of the duration completed
            
            // Interpolate the alpha value back to 1 (opaque)
            currentColor.a = Mathf.Lerp(0f, 1f, t); 
            _img.color = currentColor; // Update the color
            
            yield return null; // Wait until the next frame
        }

        // Ensure the alpha is set to 1 after completion
        currentColor.a = 1f;
        _img.color = currentColor;
    }
    
    public void Clicked()
    {
        Color currentColor = _img.color;
        currentColor.a = 0; // Set to fully transparent
        _img.color = currentColor; // Apply the updated color

        // Start the fade coroutine to go back to opaque
        StartCoroutine(FadeOut());
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
