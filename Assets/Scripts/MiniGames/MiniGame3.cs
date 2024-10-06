using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MiniGame3 : MonoBehaviour
{
    [FormerlySerializedAs("TimeLeftText")] public TextMeshProUGUI timeLeftText;
    [FormerlySerializedAs("CoinsText")] public TextMeshProUGUI coinsText;
    private int _timeLeft = 30;
    private int _coins;
    private int _myID;
    private MiniGamesSwitcher _miniGamesSwitcher => MiniGamesSwitcher.Instance;

    public void Clicked()
    {
        if (_coins == 0)
        {
            _coins = 0;
            _timeLeft = 30;
            StartCoroutine(Timer());
        }
        _coins += 1;
        
    }

    private IEnumerator Timer()
    {
        for (int i=0;i<30;i++)
        {
            _timeLeft -= 1;
            yield return new WaitForSeconds(1);
        }

        if (_coins < 100)
        {
            _miniGamesSwitcher.HideMiniGames();
            
        }
        
    }
    
    void Update()
    {
        timeLeftText.text = "Осталось времени: " + _timeLeft;
        coinsText.text = "Монет: " + _coins;
        
        
        if (_coins >= 100)
        {
            _miniGamesSwitcher.HideMiniGames();
            _miniGamesSwitcher.passedMiniGameID = _myID;
        }

        if (_miniGamesSwitcher.isWaitingAction)
        {
            _myID = _miniGamesSwitcher.waitingMiniGameID;
            _miniGamesSwitcher.isWaitingAction = false;
        }
    }
}
