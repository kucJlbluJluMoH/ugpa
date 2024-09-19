using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MiniGame2 : MonoBehaviour
{

     public TextMeshProUGUI displayTxt;
     public TMP_InputField InputField;
     
     
     
     private String rightAnswer;
     private int MyID;
     private int startingNum;
     private int missingIndex;
     private int step;
     private String sequence="";
     private MiniGamesSwitcher _miniGamesSwitcher;
     private void Start()
     {
          _miniGamesSwitcher = GameObject.Find("MiniGamesCanvas").GetComponent<MiniGamesSwitcher>();

     }

     private void Generate()
     {
          int tempType = Random.Range(0,3);//0+ 1- 2* 
          missingIndex = Random.Range(0, 5);
          switch (tempType)
          {
                   
               case 0:
                    startingNum = Random.Range(-10,10);
                    step = Random.Range(1, 10);
                    
                    for (int i = 1; i < 6; i++)
                    {
                         if (i != missingIndex)
                         {
                              startingNum += step;
                              sequence += " " + (startingNum);
                         }
                         else
                         {
                              startingNum += step;
                              rightAnswer = ""+startingNum;
                              sequence += " ?";
                         }

                    }
                    
                    break;
               case 1:
                    
                    startingNum = Random.Range(-10,10);
                    step = Random.Range(1, 10);
                    for (int i = 1; i < 6; i++)
                    {
                         if (i != missingIndex)
                         {
                         startingNum -= step;
                         sequence += " " + (startingNum);
                         }
                         else
                         {
                              startingNum -= step;
                              rightAnswer = ""+startingNum;
                              sequence += " ?";
                         }
                    }
                    
                    break;
               case 2:
                    
                    startingNum = Random.Range(-5,5);
                    while (startingNum==0)
                    {
                         startingNum=Random.Range(-5,5);
                    }
                    step = Random.Range(1, 5);
                    for (int i = 1; i < 6; i++)
                    {
                         if (i != missingIndex)
                         {
                         startingNum *= step;
                         sequence += " " + (startingNum);
                         }
                         else
                         {
                              startingNum *= step;
                              rightAnswer = ""+startingNum;
                              sequence += " ?";
                         }
                    }
                    break;

               
          }
          Debug.Log(""+startingNum+" "+step+" "+sequence );
          displayTxt.text = "Закономерность: " + sequence;
     }
     private void Update()
     {
          if (Input.GetKeyDown(KeyCode.Return))
          {
               if (InputField.text == rightAnswer)
               {
                    _miniGamesSwitcher.HideMiniGames();
                    _miniGamesSwitcher.PassedMiniGameID = MyID;
               }
               else
               {
                    _miniGamesSwitcher.HideMiniGames();
                    sequence = "";
               }
          }

          if (_miniGamesSwitcher.isWaitingAction)
          {
               MyID = _miniGamesSwitcher.WaitingMiniGameID;
               sequence = "";
               Generate();
               _miniGamesSwitcher.isWaitingAction = false;
          }
     }
}
     