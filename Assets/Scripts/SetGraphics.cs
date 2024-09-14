using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class SetGraphics : MonoBehaviour
{

    public TMP_Dropdown quality;

   public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index,false);
        //QualitySettings.vSyncCount = index;

    }
}
