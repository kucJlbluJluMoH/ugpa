using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MiniGame1 : MonoBehaviour
{
	private int _myID;
	[FormerlySerializedAs("RED")] public ButtonTrans red;
	[FormerlySerializedAs("GREEN")] public ButtonTrans green;
	[FormerlySerializedAs("BLUE")] public ButtonTrans blue;
	[FormerlySerializedAs("ShowSequenceButton")] public GameObject showSequenceButton;
	public Image colorDisplay; // Ссылка на компонент Image в UI
	private float _delay = 0.3f;
	private string _correctSequence;
	private string _inputSequence ="";
	private int _sequenceLength=5;
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
		foreach (char colorChar in _correctSequence)
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
			yield return new WaitForSeconds(_delay);
			colorDisplay.color = Color.black;
			yield return new WaitForSeconds(_delay);
		}
	}

	public void GenerateSequence()
	{
		_miniGamesSwitcher.isWaitingAction = false;
		showSequenceButton.SetActive(false);
		_correctSequence = GenerateRandomColorSequence(_sequenceLength);
		Debug.Log("Сгенерированная последовательность: " + _correctSequence);
        
		// Запускаем корутину, чтобы показать цвета
		StartCoroutine(DisplayColorSequence());
	}

	private void Update()
	{
		if (_miniGamesSwitcher.isWaitingAction)
		{
			_myID = _miniGamesSwitcher.waitingMiniGameID;
			red.Clicked();
			green.Clicked();
			blue.Clicked();
			showSequenceButton.SetActive(true);
			_miniGamesSwitcher.isWaitingAction = false;
		}
		if (_inputSequence!=""&&_inputSequence.Length!=0)
		{
			if (_inputSequence.Length == _sequenceLength)
			{
				if (_correctSequence == _inputSequence)
				{
					_miniGamesSwitcher.HideMiniGames();
					_miniGamesSwitcher.passedMiniGameID = _myID;
				}
				else
				{
					_inputSequence = "";
					_miniGamesSwitcher.HideMiniGames();
				}
			}
		}
		
	}

	public void Red()
	{
		_inputSequence += "0";
	}
	public void Green()
	{_inputSequence += "1";}
	public void Blue()
	{_inputSequence += "2";}
}