using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameComplete : MonoBehaviour
{
    public static GameComplete current;

    [SerializeField] private GameObject gameCompletePanel;
    [SerializeField] private AppleBG appleBG;
    [SerializeField] private Color blue;

    private bool showing = false;
    private bool hiding = false;

    private Transform camera;
    private float speed = 15f;

    private List<SpriteRenderer> congrats = new List<SpriteRenderer>();

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    public void DoGameComplete()
    {
        congrats.ForEach(f => Destroy(f.gameObject));
        congrats.Clear();
        Level.current.state = State.COMPLETE;
        appleBG.Activate();
        gameCompletePanel.SetActive(true);
        showing = true;
    }

    private void Update()
    {
        if (showing)
        {
            camera.position = Vector3.MoveTowards(camera.position, new Vector3(0, 13f, -10f), Time.deltaTime * speed);
            if (Vector3.Distance(camera.position, new Vector3(0, 13f, -10f)) < 0.1f)
            {
                showing = false;
                string str = "Congratulations! You have completed all the days! Charlie couldn't be happier. Thanks for playing!";
                congrats = Font.current.Write(str, -4f, 0.5f,
                    gameCompletePanel.transform, 8, true, false, 4f, true, "uistate");
                congrats.ForEach(f => f.color = blue);
            }
        }
    }
}
