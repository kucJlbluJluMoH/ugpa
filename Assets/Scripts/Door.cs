using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int DoorID;
    public AudioClip Open;
    public AudioClip Close;
    private Animator Animator;
    private Developermenu _developermenu;
    private AudioSource audioSource;
    private bool IsOpened = false;
    private MiniGamesSwitcher _miniGamesSwitcher;
    private bool isWaintingToOpen;
    void Start()
    {
        _miniGamesSwitcher = GameObject.Find("MiniGamesCanvas").GetComponent<MiniGamesSwitcher>();
        _developermenu = GameObject.Find("DeveloperMenuController").GetComponent<Developermenu>();
        audioSource = GetComponent<AudioSource>();
        Animator = GetComponent<Animator>();

    }
    public void OpenDoor()
    {
        if (!IsOpened&&DoorID!=-1)
        {
            _miniGamesSwitcher.MaxIdOfOppenedDoor = DoorID;
            IsOpened = true;
            isWaintingToOpen = false;
            audioSource.clip = Open;
            audioSource.Play();
            Animator.SetTrigger("Open");
        }
    }


    public void InteractWithDoor()
    {
        
        if (!IsOpened && DoorID!=-1)
        {
            _miniGamesSwitcher.ShowMiniGame(DoorID);
            _miniGamesSwitcher.PassedMiniGameID = -1;
            isWaintingToOpen = true;
        }
    }
    void Update()
    {
        if (isWaintingToOpen && !_miniGamesSwitcher.isInGame)
        {
            isWaintingToOpen = false;
            audioSource.clip = Close;
            audioSource.Play();
        }
        
        if (_developermenu.ForceDoorIDOpen == DoorID)
        {
            OpenDoor();
        }
        if (_miniGamesSwitcher.PassedMiniGameID == DoorID && DoorID!=-1)
        {
            OpenDoor();
        }
        if (!IsOpened && _developermenu.isOpennedAllDoors)
        {
            OpenDoor();
        }

    }
}
