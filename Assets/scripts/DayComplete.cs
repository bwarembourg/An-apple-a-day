using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayComplete : MonoBehaviour
{
    public static DayComplete current;

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
    private List<SpriteRenderer> dayFont = new List<SpriteRenderer>();

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        Font.current.Write("Press any key to start next day!", -6f, -4f, pressAnyKey.transform, 8, true, false, 6f, true, "uistate");
    }

    public void DoDayComplete()
    {
        SFX.current.Play(SFX.Type.DAY_COMPLETE);
        appleFont.ForEach(f => Destroy(f.gameObject));
        appleFont.Clear();
        coinFont.ForEach(f => Destroy(f.gameObject));
        coinFont.Clear();
        dayFont.ForEach(f => Destroy(f.gameObject));
        dayFont.Clear();
        Level.current.state = State.COMPLETE;
        appleBG.Activate();
        gameOverPanel.SetActive(true);
        showing = true;
    }

    private void Update()
    {
        if (Input.anyKeyDown && Level.current.state == State.COMPLETE && !showing)
        {
            SFX.current.Play(SFX.Type.SELECT);
            gameOverPanel.SetActive(false);
            if (Level.current.currentLevel + 1 < Level.current.levels.Length)
            {
                Level.current.RenderLevel(Level.current.currentLevel + 1);
                hiding = true;
            }
            else
            {
                GameComplete.current.DoGameComplete();
            }
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
                string dayStr = Level.current.currentLevel + 1 < 10 ? "0" + (Level.current.currentLevel + 1) : "" + (Level.current.currentLevel + 1);
                dayFont = Font.current.Write("Day " + dayStr + "/" + Level.current.levels.Length + " complete!", -4f, 0.5f,
                    gameOverPanel.transform, 8, true, false, 4f, true, "uistate");
                dayFont.ForEach(f => f.color = blue);
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
}
