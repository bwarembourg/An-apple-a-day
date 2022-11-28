using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public static GameOver current;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private AppleBG appleBG;
    [SerializeField] private Transform pressAnyKey;
    [SerializeField] private Color blue;

    private bool showing = false;
    private bool hiding = false;

    private Transform camera;
    private float speed = 15f;

    private List<SpriteRenderer> appleFont = new List<SpriteRenderer>();
    private List<SpriteRenderer> coinFont = new List<SpriteRenderer>();
    private List<SpriteRenderer> reasonFont = new List<SpriteRenderer>();
    private Reason reason;
    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").transform;

        Font.current.Write("Press any key to start again!", -6f, -4f, pressAnyKey.transform, 8, true, false, 6f, true, "uistate");
    }

    public void DoGameOver(Reason reason)
    {
        reasonFont.ForEach(f => Destroy(f.gameObject));
        reasonFont.Clear();
        appleFont.ForEach(f => Destroy(f.gameObject));
        appleFont.Clear();
        coinFont.ForEach(f => Destroy(f.gameObject));
        coinFont.Clear();
        this.reason = reason;
        Level.current.state = State.GAME_OVER;
        appleBG.Activate();
        gameOverPanel.SetActive(true);
        showing = true;
    }

    private void Update()
    {
        if (Input.anyKeyDown && Level.current.state == State.GAME_OVER && !showing)
        {
            SFX.current.Play(SFX.Type.SELECT);
            Debug.Log("restart");
            Level.current.RenderLevel(Level.current.currentLevel);
            gameOverPanel.SetActive(false);
            hiding = true;
        }
        if (showing)
        {
            camera.position = Vector3.MoveTowards(camera.position, new Vector3(0, 13f, -10f), Time.deltaTime * speed);
            if (Vector3.Distance(camera.position, new Vector3(0, 13f, -10f)) < 0.1f)
            {
                showing = false;
                appleFont = Font.current.Write("x" + Level.current.appleEated, 0, 0.125f - 1f, gameOverPanel.transform, 8, true, false, 0, false, "uistate");
                appleFont.ForEach(f => f.color = blue);
                coinFont = Font.current.Write("x" + Level.current.coinCatched + "/3", 0, -1.75f, gameOverPanel.transform, 8, true, false, 0, false, "uistate");
                coinFont.ForEach(f => f.color = blue);
                reasonFont = Font.current.Write(GetReasonFont(reason), -3.5f, 0.75f, gameOverPanel.transform, 8,
                    true, false, 3.5f, true, "uistate");
                reasonFont.ForEach(f => f.color = blue);
            }
        }
        if (hiding)
        {
            camera.position = Vector3.MoveTowards(camera.position, new Vector3(0, 0f, -10f), Time.deltaTime * speed);
            if (Vector3.Distance(camera.position, new Vector3(0, 0f, -10f)) < 0.1f)
            {
                camera.position = new Vector3(0, 0f, -10f);
                Level.current.state = State.INTRO;
                appleBG.Reset();
                hiding = false;
            }
        }
    }

    private string GetReasonFont(Reason reason)
    {
        switch (reason)
        {
            case Reason.COINS: return "Not enough coins for Charlie...";
            case Reason.COIN_MISSED: return "Too bad! You missed a coin...";
            case Reason.TOO_MUCH: return "Too much apples for Charlie...";
            default: return "Not enough apples for Charlie...";
        }
    }
}
