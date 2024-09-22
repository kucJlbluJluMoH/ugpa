using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MiniGame2 : MonoBehaviour
{

     public TextMeshProUGUI displayTxt;
     [FormerlySerializedAs("InputField")] public TMP_InputField inputField;
     
     
     
     private String _rightAnswer;
     private int _myID;
     private int _startingNum;
     private int _missingIndex;
     private int _step;
     private String _sequence="";
     private MiniGamesSwitcher _miniGamesSwitcher => MiniGamesSwitcher.Instance;

     private void Generate()
     {
          int tempType = Random.Range(0,3);//0+ 1- 2* 
          _missingIndex = Random.Range(0, 5);
          switch (tempType)
          {
                   
               case 0:
                    _startingNum = Random.Range(-10,10);
                    _step = Random.Range(1, 10);
                    
                    for (int i = 1; i < 6; i++)
                    {
                         if (i != _missingIndex)
                         {
                              _startingNum += _step;
                              _sequence += " " + (_startingNum);
                         }
                         else
                         {
                              _startingNum += _step;
                              _rightAnswer = ""+_startingNum;
                              _sequence += " ?";
                         }

                    }
                    
                    break;
               case 1:
                    
                    _startingNum = Random.Range(-10,10);
                    _step = Random.Range(1, 10);
                    for (int i = 1; i < 6; i++)
                    {
                         if (i != _missingIndex)
                         {
                         _startingNum -= _step;
                         _sequence += " " + (_startingNum);
                         }
                         else
                         {
                              _startingNum -= _step;
                              _rightAnswer = ""+_startingNum;
                              _sequence += " ?";
                         }
                    }
                    
                    break;
               case 2:
                    
                    _startingNum = Random.Range(-5,5);
                    while (_startingNum==0)
                    {
                         _startingNum=Random.Range(-5,5);
                    }
                    _step = Random.Range(1, 5);
                    for (int i = 1; i < 6; i++)
                    {
                         if (i != _missingIndex)
                         {
                         _startingNum *= _step;
                         _sequence += " " + (_startingNum);
                         }
                         else
                         {
                              _startingNum *= _step;
                              _rightAnswer = ""+_startingNum;
                              _sequence += " ?";
                         }
                    }
                    break;

               
          }
          Debug.Log(""+_startingNum+" "+_step+" "+_sequence );
          displayTxt.text = "Закономерность: " + _sequence;
     }
     private void Update()
     {
          if (Input.GetKeyDown(KeyCode.Return))
          {
               if (inputField.text == _rightAnswer)
               {
                    _miniGamesSwitcher.HideMiniGames();
                    _miniGamesSwitcher.passedMiniGameID = _myID;
               }
               else
               {
                    _miniGamesSwitcher.HideMiniGames();
                    _sequence = "";
               }
          }

          if (_miniGamesSwitcher.isWaitingAction)
          {
               _myID = _miniGamesSwitcher.waitingMiniGameID;
               _sequence = "";
               Generate();
               _miniGamesSwitcher.isWaitingAction = false;
          }
     }
}
     