using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SectorsManager : MonoBehaviour
{
    private MiniGamesSwitcher _miniGamesSwitcher => MiniGamesSwitcher.Instance;
    private int _maxIndexOppenedDoor = 0;
    [FormerlySerializedAs("Sectors")] public List<GameObject> sectors;
    // Start is called before the first frame update
    void Start()
    {
        for (int i =0; i<sectors.Count;i++)
        {
            sectors[i].SetActive(false);
        }
        sectors[0].SetActive(true);
        sectors[1].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (_miniGamesSwitcher.maxIdOfOppenedDoor+1 >= _maxIndexOppenedDoor)
        {
            _maxIndexOppenedDoor += 1;
            sectors[_maxIndexOppenedDoor].SetActive(true);
        }
    }
}
