using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSound : MonoBehaviour
{
    public AudioClip clickSound; // �����Ч
    private AudioSource audioSource; // AudioSource���

    void Start()
    {
        // ��ȡ AudioSource �������������������һ��
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // ���õ����Ч
        audioSource.clip = clickSound;
    }

    void OnMouseDown()
    {
        // ���ŵ����Ч
        if (audioSource != null && clickSound != null)
        {
            audioSource.Play();
        }
    }

}
