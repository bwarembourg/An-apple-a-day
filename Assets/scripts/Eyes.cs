using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes : MonoBehaviour
{
    [SerializeField] private float maxTimer = 0.5f;
    [SerializeField] private GameObject bg;

    public static Eyes current;

    public enum type { LOVE, NOPE, STAR }

    private bool animated = false;
    private float timer = 0f;
    private Animator animator;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Do(type type)
    {
        animated = true;
        bg.SetActive(true);
        switch(type)
        {
            case type.STAR:
                animator.SetTrigger("star");
                ParticleManager.current.Coin();
                break;
            case type.NOPE:
                animator.SetTrigger("nope");
                SFX.current.Play(SFX.Type.EAT_BAD);
                break;
            default:
                animator.SetTrigger("love");
                SFX.current.Play(SFX.Type.EAT_GOOD);
                ParticleManager.current.Life();
                break;
        }
    }

    public void Reset()
    {
        bg.SetActive(false);
        animator.SetTrigger("reset");
        animated = false;
    }

    private void Update()
    {
        if (animated)
        {
            timer += Time.deltaTime;
            if (timer >= maxTimer)
            {
                bg.SetActive(false);
                animator.SetTrigger("reset");
                animated = false;
                timer = 0;
            }
        }
    }

}
