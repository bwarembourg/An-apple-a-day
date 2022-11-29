using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] AudioClip[] musics;

    public static Music current;

    private AudioSource source;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayRandom()
    {
        source.Stop();
        List<AudioClip> availableMusics = musics.ToList().FindAll(m => m.name != source.clip.name);
        source.clip = availableMusics[Random.Range(0, availableMusics.Count)];
        source.Play();
    }
}
