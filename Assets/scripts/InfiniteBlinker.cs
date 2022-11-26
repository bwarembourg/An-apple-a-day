using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteBlinker : MonoBehaviour
{
    [SerializeField] private bool onChildren;

    private float TIMER_BLINK_MAX = 0.3f;
    private float timerBlink = 0.3f;
    private bool on = true;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (timerBlink >= 0f)
        {
            timerBlink -= Time.deltaTime;
        }
        if (timerBlink <= 0f)
        {
            on = !on;
            ShowHide(on);
            timerBlink = TIMER_BLINK_MAX;
        }
    }

    public void ShowHide(bool on)
    {
        if (onChildren)
        {
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(on);
            }
            return;
        }
        sr.enabled = on;
    }
}
