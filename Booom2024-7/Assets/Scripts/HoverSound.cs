using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverSound : MonoBehaviour
{
    public AudioClip hoverSound; // 悬停音效
    private AudioSource audioSource; // AudioSource组件
    private bool isHovering = false; // 是否悬停在物体上

    void Start()
    {
        // 获取 AudioSource 组件，如果不存在则添加一个
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 设置悬停音效
        audioSource.clip = hoverSound;
    }

    void OnMouseEnter()
    {
        isHovering = true;
        // 播放悬停音效
        if (audioSource != null && hoverSound != null)
        {
            audioSource.Play();
        }
    }

    void OnMouseExit()
    {
        isHovering = false;
        // 停止悬停音效
        if (audioSource != null && hoverSound != null)
        {
            audioSource.Stop();
        }
    }

    void Update()
    {
        // 如果音效正在播放且不在悬停状态，则停止音效
        if (audioSource.isPlaying && !isHovering)
        {
            audioSource.Stop();
        }
    }
}