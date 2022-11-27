using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    [SerializeField] private Transform lifeDisplay;

    private float timer = 0f;
    private float maxTimer = 0.5f;

    private int life = 80;

    public static Life current;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        Reset();
    }

    private void Update()
    {
        if (Level.current.state != State.GAME || Level.current.tuto)
            return;
        timer += Time.deltaTime;
        if (timer >= maxTimer)
        {
            Lower();
            timer = 0;
        }
    }

    public void Reset()
    {
        life = 60;
        UpdateDisplay();
    }

    public void Lower()
    {
        if (life > 0)
            life -= 5;
        if (life <= 0)
        {
            life = 0;
            GameOver.current.DoGameOver(Reason.NOT_ENOUGH);
        }
        UpdateDisplay();
    }

    public void Add()
    {
        if (life < 100)
            life += 15;
        if (life >= 100)
        {
            life = 100;
            GameOver.current.DoGameOver(Reason.TOO_MUCH);
        }
        UpdateDisplay();
    }

    public void LowerFromItem()
    {
        if (life > 0)
            life -= 10;
        if (life <= 0)
        {
            GameOver.current.DoGameOver(Reason.NOT_ENOUGH);
            life = 0;
        }
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        lifeDisplay.localScale = new Vector3((float) life / 100f, lifeDisplay.localScale.y, lifeDisplay.localScale.z);
    }
}
