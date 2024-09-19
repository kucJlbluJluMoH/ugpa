using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Random = UnityEngine.Random;

public class MiniGame1 : MonoBehaviour
{
	private int MyID;
	public ButtonTrans RED;
	public ButtonTrans GREEN;
	public ButtonTrans BLUE;
	public GameObject ShowSequenceButton;
	public Image colorDisplay; // Ссылка на компонент Image в UI
	private float Delay = 0.3f;
	private string correctSequence;
	private string inputSequence ="";
	private int sequenceLength=5;
	private MiniGamesSwitcher _miniGamesSwitcher;
	private void Start()
	{
		// Генерируем случайную последовательность _miniGamesSwitcher = GameObject.Find("MiniGamesCanvas").GetComponent<MiniGamesSwitcher>();
		_miniGamesSwitcher = GameObject.Find("MiniGamesCanvas").GetComponent<MiniGamesSwitcher>();
		
	}

	private string GenerateRandomColorSequence(int length)
	{
		string sequence = "";
		for (int i = 0; i < length; i++)
		{
			int randomColor = Random.Range(0, 3); // 0, 1 или 2
			sequence += randomColor.ToString();
		}
		return sequence;
	}

	private IEnumerator DisplayColorSequence()
	{
		foreach (char colorChar in correctSequence)
		{
			// Изменяем цвет отображения в зависимости от символа
			switch (colorChar)
			{
				case '0':
					colorDisplay.color = Color.red; // Красный
					break;
				case '1':
					colorDisplay.color = Color.green; // Зеленый
					break;
				case '2':
					colorDisplay.color = Color.blue; // Синий
					break;
			}

			// Делаем паузу перед переходом к следующему цвету
			yield return new WaitForSeconds(Delay);
			colorDisplay.color = Color.black;
			yield return new WaitForSeconds(Delay);
		}
	}

	public void GenerateSequence()
	{
		_miniGamesSwitcher.isWaitingAction = false;
		ShowSequenceButton.SetActive(false);
		correctSequence = GenerateRandomColorSequence(sequenceLength);
		Debug.Log("Сгенерированная последовательность: " + correctSequence);
        
		// Запускаем корутину, чтобы показать цвета
		StartCoroutine(DisplayColorSequence());
	}

	private void Update()
	{
		if (_miniGamesSwitcher.isWaitingAction)
		{
			MyID = _miniGamesSwitcher.WaitingMiniGameID;
			RED.Clicked();
			GREEN.Clicked();
			BLUE.Clicked();
			ShowSequenceButton.SetActive(true);
			_miniGamesSwitcher.isWaitingAction = false;
		}
		if (inputSequence!=""&&inputSequence.Length!=0)
		{
			if (inputSequence.Length == sequenceLength)
			{
				if (correctSequence == inputSequence)
				{
					_miniGamesSwitcher.HideMiniGames();
					_miniGamesSwitcher.PassedMiniGameID = MyID;
				}
				else
				{
					inputSequence = "";
					_miniGamesSwitcher.HideMiniGames();
				}
			}
		}
		
	}

	public void Red()
	{
		inputSequence += "0";
	}
	public void Green()
	{inputSequence += "1";}
	public void Blue()
	{inputSequence += "2";}
}