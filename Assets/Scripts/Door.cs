using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Door : MonoBehaviour
{
    [FormerlySerializedAs("DoorID")] public int doorID;
    [FormerlySerializedAs("Open")] public AudioClip open;
    [FormerlySerializedAs("Close")] public AudioClip close;
    private Animator _animator;
    private Developermenu _developermenu => Developermenu.Instance;
    private AudioSource _audioSource;
    private bool _isOpened = false;
    private MiniGamesSwitcher _miniGamesSwitcher => MiniGamesSwitcher.Instance;
    private bool _isWaintingToOpen;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();

    }
    public void OpenDoor()
    {
        if (!_isOpened&&doorID!=-1)
        {
            _miniGamesSwitcher.maxIdOfOppenedDoor = doorID;
            _isOpened = true;
            _isWaintingToOpen = false;
            _audioSource.clip = open;
            _audioSource.Play();
            _animator.SetTrigger("Open");
        }
    }


    public void InteractWithDoor()
    {
        
        if (!_isOpened && doorID!=-1)
        {
            _miniGamesSwitcher.ShowMiniGame(doorID);
            _miniGamesSwitcher.passedMiniGameID = -1;
            _isWaintingToOpen = true;
        }
    }
    void Update()
    {
        if (_isWaintingToOpen && !_miniGamesSwitcher.isInGame)
        {
            _isWaintingToOpen = false;
            _audioSource.clip = close;
            _audioSource.Play();
        }
        
        if (_developermenu.forceDoorIDOpen == doorID)
        {
            OpenDoor();
        }
        if (_miniGamesSwitcher.passedMiniGameID == doorID && doorID!=-1)
        {
            OpenDoor();
        }
        if (!_isOpened && _developermenu.isOpennedAllDoors)
        {
            OpenDoor();
        }

    }
}
