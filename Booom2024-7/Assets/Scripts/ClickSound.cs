using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSound : MonoBehaviour
{
    public AudioClip clickSound; // 点击音效
    private AudioSource audioSource; // AudioSource组件

    void Start()
    {
        // 获取 AudioSource 组件，如果不存在则添加一个
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 设置点击音效
        audioSource.clip = clickSound;
    }

    void OnMouseDown()
    {
        // 播放点击音效
        if (audioSource != null && clickSound != null)
        {
            audioSource.Play();
        }
    }

}
