using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public static SFX current;

    [SerializeField] private AudioClip select;
    [SerializeField] private AudioClip[] rolls;
    [SerializeField] private AudioClip swooshIn;
    [SerializeField] private AudioClip swooshOut;
    [SerializeField] private AudioClip coin;
    [SerializeField] private AudioClip[] eatGood;
    [SerializeField] private AudioClip[] eatBad;
    [SerializeField] private AudioClip dayComplete;

    public enum Type { ROLL, SELECT, SWOOSH_IN, SWOOSH_OUT, COIN, EAT_BAD, EAT_GOOD, DAY_COMPLETE };

    AudioSource[] sources;


    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        sources = GetComponents<AudioSource>();
    }

    public void Play(Type type) {
        AudioSource source = GetAvailableAudioSource();
        if (source == null)
            return;
        switch(type)
        {
            case Type.ROLL: source.clip = rolls[Random.Range(0, rolls.Length)]; break;
            case Type.SWOOSH_IN: source.clip = swooshIn; break;
            case Type.SWOOSH_OUT: source.clip = swooshOut; break;
            case Type.COIN: source.clip = coin; break;
            case Type.EAT_BAD: source.clip = eatBad[Random.Range(0, eatBad.Length)]; break;
            case Type.EAT_GOOD: source.clip = eatGood[Random.Range(0, eatGood.Length)]; break;
            case Type.DAY_COMPLETE:
                Music.current.Pause();
                source.clip = dayComplete;
                break;
            default: source.clip = select; break;
        }
        source.Play();
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach(AudioSource source in sources)
        {
            if (!source.isPlaying)
                return source;
        }
        return null;
    }
}
