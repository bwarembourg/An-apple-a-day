using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButton : MonoBehaviour
{
    public enum ButtonType { NEW, CONTINUE };

    [SerializeField] private ButtonType type;
    [SerializeField] private Sprite on;
    [SerializeField] private Sprite off;
    [SerializeField]

    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnMouseEnter()
    {
        sr.sprite = on;
    }

    private void OnMouseExit()
    {
        sr.sprite = off;
    }

    private void OnMouseDown()
    {
        SFX.current.Play(SFX.Type.SELECT);
        if (type == ButtonType.NEW)
            Title.current.StartGame(0);
        else
        {
            Title.current.StartGame(PlayerPrefs.GetInt("day", 0));
        }
    }
}
