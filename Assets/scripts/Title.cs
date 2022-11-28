using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    public static Title current;

    [SerializeField] private GameObject titlePanel;
    [SerializeField] private AppleBG appleBG;
    [SerializeField] private GameObject continueBtn;
    [SerializeField] private bool resetTuto;
    [SerializeField] private bool resetGame;
    [SerializeField] private Color green;
    [SerializeField] private Transform pressAnyKey;

    private bool hiding = false;
    private Transform camera;
    private float speed = 15f;
    private int lvl;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        appleBG.Activate();
        if (resetTuto)
            PlayerPrefs.SetInt("tutoDone", 0);
        if (resetGame)
            PlayerPrefs.SetInt("day", 0);
        if (PlayerPrefs.GetInt("day", 0) == 0)
        {
            continueBtn.SetActive(false);
            Font.current.Write("Press any key to start a new game", -6f, -4f, pressAnyKey.transform, 8, true, false, 6f, true, "uistate");
        }
        else
        {
            List<SpriteRenderer> days = Font.current.Write("DAY " + (PlayerPrefs.GetInt("day", 0) + 1) + "/" + Level.current.levels.Length, -6f, -1f,
                titlePanel.transform, 6, true, false, 6f, true, "uistate");
            days.ForEach(d => d.color = green);
            Font.current.Write("Press any key to start to continue", -6f, -4f, pressAnyKey.transform, 8, true, false, 6f, true, "uistate");
        }
    }

    public void StartGame(int level)
    {
        lvl = level;
        Level.current.RenderLevel(level);
        titlePanel.SetActive(false);
        hiding = true;
    }

    private void Update()
    {
        if (!hiding && Input.anyKeyDown && Level.current.state == State.TITLE)
        {
            SFX.current.Play(SFX.Type.SELECT);
            if (PlayerPrefs.GetInt("day", 0) == 0)
                StartGame(0);
            else
                StartGame(PlayerPrefs.GetInt("day", 0));
        }
        if (hiding)
        {
            camera.position = Vector3.MoveTowards(camera.position, new Vector3(0, 0f, -10f), Time.deltaTime * speed);
            if (Vector3.Distance(camera.position, new Vector3(0, 0f, -10f)) < 0.1f)
            {
                camera.position = new Vector3(0, 0f, -10f);
                Level.current.state = lvl == 0 ? State.GAME : State.INTRO;
                appleBG.Reset();
                hiding = false;
            }
        }
    }
}
