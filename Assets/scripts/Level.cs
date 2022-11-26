using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private TextAsset[] levels;
    [SerializeField] private Animator belt;
    [SerializeField] private GameObject noseIdle;
    [SerializeField] private Animator noseEating;
    [SerializeField] private Animator noseEatingBG;
    [SerializeField] private Transform hand;

    [Header("ITEMS")]
    [SerializeField] private GameObject redApple;
    [SerializeField] private GameObject chocolate;
    [SerializeField] private GameObject poo;
    [SerializeField] private GameObject rabbit;
    [SerializeField] private GameObject frog;
    [SerializeField] private GameObject rottenRedApple;
    [SerializeField] private GameObject coin;

    private float speed = 20f;
    private float padding = 5f;
    private float x = 0;
    private List<GameObject> items = new List<GameObject>();
    private List<Vector3> dests = new List<Vector3>();
    private bool rollin = false;
    private bool eatin = false;
    private bool catchin = false;
    private bool catchinUp = false;
    private bool ate = false;

    private float timerEat = 0f;
    private float maxTimerEat = 0.25f;
    public int coinCatched = 0;
    public int appleEated = 0;

    public static Level current;
    public State state = State.GAME;

    private void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        RenderLevel(0);
    }

    public void RenderLevel(int lvl)
    {
        Life.current.Reset();
        eatin = false;
        rollin = false;
        catchin = false;
        coinCatched = 0;
        appleEated = 0;
        items.ForEach(item => Destroy(item));
        items.Clear();
        noseIdle.SetActive(true);
        noseEating.SetTrigger("reset");
        noseEatingBG.SetTrigger("reset");
        belt.SetTrigger("stop");
        x = 0;
        string lvlStr = levels[lvl].text;
        string[] strs = lvlStr.Split('|');
        string goalStr = strs[0];
        string[] itemsStr = strs[1].Split('-');
        Goal.current.Init(goalStr);
        foreach(string itemStr in itemsStr)
        {
            if (itemStr == "A")
            {
                GameObject itemGO = Instantiate(redApple, new Vector3(x, -1f, 0), Quaternion.identity, transform);
                items.Add(itemGO);
                x += padding;
            }
            if (itemStr == "P")
            {
                GameObject itemGO = Instantiate(coin, new Vector3(x, -1f, 0), Quaternion.identity, transform);
                items.Add(itemGO);
                x += padding;
            }
            if (itemStr == "a")
            {
                GameObject itemGO = Instantiate(rottenRedApple, new Vector3(x, -1f, 0), Quaternion.identity, transform);
                items.Add(itemGO);
                x += padding;
            }
            if (itemStr == "k")
            {
                GameObject itemGO = Instantiate(chocolate, new Vector3(x, -1f, 0), Quaternion.identity, transform);
                items.Add(itemGO);
                x += padding;
            }
            if (itemStr == "f")
            {
                GameObject itemGO = Instantiate(frog, new Vector3(x, -1f, 0), Quaternion.identity, transform);
                items.Add(itemGO);
                x += padding;
            }
            if (itemStr == "p")
            {
                GameObject itemGO = Instantiate(poo, new Vector3(x, -1f, 0), Quaternion.identity, transform);
                items.Add(itemGO);
                x += padding;
            }
            if (itemStr == "r")
            {
                GameObject itemGO = Instantiate(rabbit, new Vector3(x, -1f, 0), Quaternion.identity, transform);
                items.Add(itemGO);
                x += padding;
            }
        }
    }

    private void Roll()
    {
        belt.SetTrigger("roll");
        rollin = true;
        dests.Clear();
        foreach (GameObject item in items)
        {
            dests.Add(item.transform.position - new Vector3(5f, 0, 0));
        }
    }

    private void Eat()
    {
        noseIdle.SetActive(false);
        noseEating.SetTrigger("eat");
        noseEatingBG.SetTrigger("eat");
        ate = false;
        eatin = true;
        timerEat = 0;
    }

    private void CatchCoin()
    {
        catchin = true;
        catchinUp = true;
    }

    private void CatchItem(GameObject item)
    {
        if (item == null)
            return;
        item.SetActive(false);
        if (item.tag == "coin")
        {
            coinCatched++;
        }
    }

    private void EatItem(GameObject item)
    {
        if (item == null)
            return;
        item.SetActive(false);
        if (item.tag == "bad")
            Life.current.LowerFromItem();
        else if (item.tag == "apple")
        {
            if (Goal.current.IsInGoal(item.GetComponent<Apple>().type))
            {
                Life.current.Add();
                appleEated++;
            }
            else
                Life.current.LowerFromItem();
        } else if (item.tag == "coin")
        {
            Life.current.LowerFromItem();
        }
    }

    private void Update()
    {
        if (state != State.GAME)
            return;
        if (!rollin && !eatin && !catchin)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                Roll();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Eat();
                return;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                CatchCoin();
                return;
            }
        }

        if (eatin)
        {
            timerEat += Time.deltaTime;
            if (!ate && timerEat >= maxTimerEat)
            {
                ate = true;
                GameObject item = items.Find(item => Vector3.Distance(new Vector3(0, -1f, 0), item.transform.position) < 0.1f);
                EatItem(item);
            }
            if (noseEating.GetCurrentAnimatorStateInfo(0).IsName("empty"))
            {
                eatin = false;
                noseIdle.SetActive(true);
                Roll();
            }
        }

        if (rollin)
        {
            for (int i = 0; i < items.Count; i++)
            {
                GameObject item = items[i];
                item.transform.position = Vector3.MoveTowards(item.transform.position, dests[i], Time.deltaTime * speed);
                if (Vector3.Distance(item.transform.position, dests[i]) < 0.1f)
                {
                    item.transform.position = dests[i];
                }
            }
            if (Vector3.Distance(items[0].transform.position, dests[0]) < 0.1f)
            {
                rollin = false;
                belt.SetTrigger("stop");
            }
        }

        if (catchin)
        {
            if (catchinUp)
            {
                hand.transform.position = Vector3.MoveTowards(hand.transform.position, new Vector3(-1f, -3f, 0f), Time.deltaTime * speed);
                if (Vector3.Distance(hand.transform.position, new Vector3(-1f, -3f, 0f)) < 0.1f) {
                    hand.transform.position = new Vector3(-1f, -3f, 0f);
                    catchinUp = false;
                    GameObject item = items.Find(item => Vector3.Distance(new Vector3(0, -1f, 0), item.transform.position) < 0.1f);
                    CatchItem(item);
                }
            } else
            {
                hand.transform.position = Vector3.MoveTowards(hand.transform.position, new Vector3(-1f, -6f, 0f), Time.deltaTime * speed);
                if (Vector3.Distance(hand.transform.position, new Vector3(-1f, -6f, 0f)) < 0.1f)
                {
                    hand.transform.position = new Vector3(-1f, -6f, 0f);
                    catchin = false;
                    Roll();
                }
            }
        }
    }

}
