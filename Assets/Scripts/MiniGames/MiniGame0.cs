using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MiniGame0 : MonoBehaviour
{
    public String rightAnswer;
    
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

    public void GenerateCode()
    {
        int lengthOfSequence = 3; // Длина последовательности
        List<int> randomSequence = GenerateRandomSequence(lengthOfSequence);
        
        string encryptedSequence = EncryptSequence(randomSequence);
                                                                                                   
        // Выводим результаты в консоль
        Debug.Log("Сгенерированная последовательность: " + string.Join(", ", randomSequence));
        Debug.Log("Зашифрованная последовательность: " + encryptedSequence);
                                                                                                   
        // Дешифровка для проверки
        List<int> decryptedSequence = DecryptSequence(encryptedSequence);
        Debug.Log("Дешифрованная последовательность: " + string.Join(", ", decryptedSequence));
        rightAnswer = string.Join(", ", decryptedSequence);
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
