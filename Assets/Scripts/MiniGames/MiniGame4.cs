using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MiniGame4 : MonoBehaviour
{

	[FormerlySerializedAs("Postions")] public List<GameObject> postions;
	public List<GameObject> positionsForRandomShuffle;
	[FormerlySerializedAs("PuzzlePieces")] public List<GameObject> puzzlePieces;
	public bool _isDragging;
	private MiniGamesSwitcher _miniGamesSwitcher;
	private int _previusClickedID=-1;
	private int _myID;
	protected int _rightPlaces = 0;
	private void Start()
	{
		_miniGamesSwitcher = GameObject.Find("MiniGamesCanvas").GetComponent<MiniGamesSwitcher>();
		Shuffle();
	}

	private (bool isPiece, int index) CheckAndGetId(GameObject obj)
	{
		bool isPiece;
		isPiece = false;
		int tempI = -1;
		for (int i = 0; i < puzzlePieces.Count; i++)
		{
			if (puzzlePieces[i] == obj)
			{
				tempI = i;
				isPiece = true;
				break;
			}
		}

		if (!isPiece)
		{
			for (int i = 0; i < postions.Count; i++)
			{
				if (postions[i] == obj)
				{
					tempI = i;
					isPiece = false;
					break;
				}
			}
		}
		return (isPiece,tempI);

	}
	public void Click(GameObject obj)
	{
		var (isPiece, index) = CheckAndGetId(obj);
		if (!_isDragging && isPiece)
		{
			_isDragging = true;
			puzzlePieces[index].transform.localScale = new Vector3(1.1f, 1.1f, 1);
			_previusClickedID = index;
		}

		else if (_isDragging && !isPiece)
		{

			puzzlePieces[_previusClickedID].transform.position = postions[index].transform.position;
			puzzlePieces[_previusClickedID].transform.localScale = new Vector3(1, 1, 1);
			_previusClickedID = -1;
			_isDragging = false;
		

		}

		else if (_isDragging && isPiece)
		{

			if (index == _previusClickedID)
			{
				puzzlePieces[_previusClickedID].transform.localScale = new Vector3(1, 1, 1);
				_previusClickedID = -1;
				_isDragging = false;

			}
			else
			{
				puzzlePieces[_previusClickedID].transform.localScale = new Vector3(1, 1, 1);
				_previusClickedID = index;
				puzzlePieces[index].transform.localScale = new Vector3(1.1f, 1.1f, 1);
			}
			
		}
	}
	private void Shuffle()
	{
		_previusClickedID = -1;
		_rightPlaces = 0;
		_isDragging = false;
        // Перемешиваем puzzlePieces
        List<GameObject> shuffledPuzzlePieces = new List<GameObject>(puzzlePieces);
        ShuffleList(shuffledPuzzlePieces);

        // Перемещаем перемешанные кусочки на случайные позиции
        for (int i = 0; i < shuffledPuzzlePieces.Count; i++)
        {
            // Перемещаем кусочек пазла на соответствующую позицию
            shuffledPuzzlePieces[i].transform.position = positionsForRandomShuffle[i].transform.position;
        }
    }

    private void ShuffleList<T>(List<T> list)
    {
        System.Random rand = new System.Random();
        int n = list.Count;

        for (int i = n - 1; i > 0; i--)
        {
            int j = rand.Next(0, i + 1);
            // Меняем местами элементы
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    private bool CheckRightPlaces()
    {
	    _rightPlaces = 0;
	    for (int i = 0; i < postions.Count; i++)
	    {

		    if (puzzlePieces[i].transform.position == postions[i].transform.position)
		    {
			    _rightPlaces += 1;
			 
		    }			    
	    }
		    

	    Debug.Log(_rightPlaces);
	    if (_rightPlaces == 5)
	    {
		    return true;
	    }
	    else
	    {
		    return false;
	    }
	    
	    

    }
	private void Update()
	{
		if (_miniGamesSwitcher.isWaitingAction)
		{
		    _myID = _miniGamesSwitcher.waitingMiniGameID;
		    _miniGamesSwitcher.isWaitingAction = false;
		    Shuffle();
		}

		if (CheckRightPlaces())
		{
			_miniGamesSwitcher.HideMiniGames();
			_miniGamesSwitcher.passedMiniGameID = _myID;
		}
	}
}