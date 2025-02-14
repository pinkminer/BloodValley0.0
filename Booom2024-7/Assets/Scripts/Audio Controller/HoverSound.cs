using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverSound : MonoBehaviour
{
    public AudioClip hoverSound; // ��ͣ��Ч
    private AudioSource audioSource; // AudioSource���
    private bool isHovering = false; // �Ƿ���ͣ��������

    void Start()
    {
        // ��ȡ AudioSource �������������������һ��
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // ������ͣ��Ч
        audioSource.clip = hoverSound;
    }

    void OnMouseEnter()
    {
        isHovering = true;
        // ������ͣ��Ч
        if (audioSource != null && hoverSound != null)
        {
            audioSource.Play();
        }
    }

    void OnMouseExit()
    {
        isHovering = false;
        // ֹͣ��ͣ��Ч
        if (audioSource != null && hoverSound != null)
        {
            audioSource.Stop();
        }
    }

    void Update()
    {
        // �����Ч���ڲ����Ҳ�����ͣ״̬����ֹͣ��Ч
        if (audioSource.isPlaying && !isHovering)
        {
            audioSource.Stop();
        }
    }
}