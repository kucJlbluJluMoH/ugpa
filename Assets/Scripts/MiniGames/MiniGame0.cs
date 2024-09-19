using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class MiniGame0 : MonoBehaviour
{
    private int MyId;
    private String rightAnswer;
    public TMP_InputField tmpInputFiled;
    public TextMeshProUGUI sequnceTxt;
    
    private MiniGamesSwitcher _miniGamesSwitcher;
    // Шифр для цифр от 1 до 6
    private static readonly Dictionary<int, string> cipher = new Dictionary<int, string>
    {
        { 1, "00100" },
        { 2, "01001" },
        { 3, "01101" },
        { 4, "01111" },
        { 5, "00010" },
        { 6, "01011" }
    };

    private void Start()
    {
        _miniGamesSwitcher = GameObject.Find("MiniGamesCanvas").GetComponent<MiniGamesSwitcher>();
    }

    private void Update()
    {
        
        if (_miniGamesSwitcher.isWaitingAction)
        {
            MyId = _miniGamesSwitcher.WaitingMiniGameID;
            _miniGamesSwitcher.isWaitingAction = false;
            GenerateCode();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (tmpInputFiled.text == rightAnswer)
            {
                _miniGamesSwitcher.HideMiniGames();
                _miniGamesSwitcher.PassedMiniGameID = MyId;
            }
            else
            {
                tmpInputFiled.text = "";
                _miniGamesSwitcher.HideMiniGames();
            }
            
        }
    }

    private void GenerateCode()
    {
        int lengthOfSequence = 3; // Длина последовательности
        List<int> randomSequence = GenerateRandomSequence(lengthOfSequence);
        
        string encryptedSequence = EncryptSequence(randomSequence);
        sequnceTxt.text = "Последовательность: " + encryptedSequence;                                                                         
        // Выводим результаты в консоль
        // Дешифровка для проверки
        List<int> decryptedSequence = DecryptSequence(encryptedSequence);
        rightAnswer = string.Join("", decryptedSequence);
        Debug.Log("Дешифрованная последовательность: " + rightAnswer);
    }
    // Генерирует случайную последовательность цифр
    private List<int> GenerateRandomSequence(int length)
    {
        List<int> sequence = new List<int>();
        for (int i = 0; i < length; i++)
        {
            sequence.Add(Random.Range(1, 7)); // Генерируем числа от 1 до 6
        }
        return sequence;
    }

    // Шифрует последовательность
    private string EncryptSequence(List<int> sequence)
    {
        string encrypted = "";
        foreach (var number in sequence)
        {
            if (cipher.ContainsKey(number))
            {
                encrypted += cipher[number];
            }
            else
            {
                throw new System.ArgumentException("Неверный номер для шифрования: " + number);
            }
        }
        return encrypted;
    }

    // Дешифрует зашифрованную последовательность
    private List<int> DecryptSequence(string encryptedSequence)
    {
        List<int> decrypted = new List<int>();

        // Проверяем каждую зашифрованную часть (длина шифра равна 5 символам)
        for (int i = 0; i < encryptedSequence.Length; i += 5)
        {
            string chunk = encryptedSequence.Substring(i, 5);
            int number = -1;

            foreach (var kvp in cipher)
            {
                if (kvp.Value == chunk)
                {
                    number = kvp.Key;
                    break;
                }
            }

            if (number == -1)
            {
                throw new System.ArgumentException("Не удалось расшифровать часть: " + chunk);
            }

            decrypted.Add(number);
        }
        return decrypted;
    }
}
