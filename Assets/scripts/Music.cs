using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] AudioClip[] musics;
    [SerializeField] AudioClip gameover;

    public static Music current;

    private AudioSource source;

    private string lastPlayed;
    private bool paused = false;

    private float timer = 0f;
    private float maxTimer = 1.5f;
    private bool fadinOut = false;
    private bool fadinIn = false;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();
        lastPlayed = source.clip.name;
    }

    public void PlayRandom()
    {
        source.Stop();
        List<AudioClip> availableMusics = musics.ToList().FindAll(m => m.name != lastPlayed);
        source.clip = availableMusics[Random.Range(0, availableMusics.Count)];
        source.Play();
        lastPlayed = source.clip.name;
    }

    public void PlayGameOver()
    {
        source.Stop();
        source.clip = gameover;
        source.Play();
    }

    public void Pause()
    {
        fadinOut = true;
        paused = true;
    }

    private void Resume()
    {
        fadinIn = true;
        source.Play();
    }

    private void Update()
    {
        if (paused)
        {
            timer += Time.deltaTime;
            if (timer >= maxTimer)
            {
                Resume();
                paused = false;
                timer = 0;
            }
        }

        if (fadinOut)
        {
            source.volume = source.volume - Time.deltaTime * 1f;
            if (source.volume <= 0)
            {
                source.Pause();
                fadinOut = false;
            }
        }
        if (fadinIn)
        {
            source.volume = source.volume + Time.deltaTime * 1f;
            if (source.volume >= 0.7f)
            {
                source.volume = 0.7f;
                fadinIn = false;
            }
        }
    }
}
