using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int DoorID;
    private Animator Animator;
    private AudioSource audioSource;
    private bool IsOpened = false;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Animator = GetComponent<Animator>();

    }
    public void OpenDoor()
    {
        IsOpened = true;
        audioSource.Play();
        Animator.SetTrigger("Open");
    }
    public void InteractWithDoor()
    {
        
        if (!IsOpened && DoorID!=-1)
        {
            OpenDoor();
        }
    }
    void Update()
    {
        
    }
}
