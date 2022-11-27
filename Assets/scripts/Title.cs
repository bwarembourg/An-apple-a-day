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
        if (PlayerPrefs.GetInt("day", 0) == 0)
            continueBtn.SetActive(false);
        if (resetTuto)
            PlayerPrefs.SetInt("tutoDone", 0);
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
