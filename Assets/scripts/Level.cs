using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] public TextAsset[] levels;
    [SerializeField] private Animator belt;
    [SerializeField] private GameObject noseIdle;
    [SerializeField] private Animator noseEating;
    [SerializeField] private Animator noseEatingBG;
    [SerializeField] private Transform hand;
    [SerializeField] private Transform goalPanel;
    [SerializeField] private GameObject pressAnyKey;


    [Header("ITEMS")]
    [SerializeField] private GameObject redApple;
    [SerializeField] private GameObject orangeApple;
    [SerializeField] private GameObject greenApple;
    [SerializeField] private GameObject yellowApple;

    [SerializeField] private GameObject chocolate;
    [SerializeField] private GameObject poo;
    [SerializeField] private GameObject rabbit;
    [SerializeField] private GameObject frog;

    [SerializeField] private GameObject rottenRedApple;
    [SerializeField] private GameObject rottenOrangeApple;
    [SerializeField] private GameObject rottenGreenApple;
    [SerializeField] private GameObject rottenYellowApple;

    [SerializeField] private GameObject coin;

    public int currentLevel;

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
    private List<SpriteRenderer> day = new List<SpriteRenderer>();

    public static Level current;
    public State state = State.TITLE;
    public bool tuto = false;
    private bool keyAvailable = true;

    private bool showinGoalPanel = false;
    private bool hidinGoalPanel = false;
    private float timerGoal = 0;
    private float maxTimerGoal = 0.5f;
    private bool rolledOverCoin = false;
    private bool catchedCoin = false;
    private bool swooshed = false;

    private void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //RenderLevel(0);
        Font.current.Write("Press any key to start!", -8f, 5f, pressAnyKey.transform, 50, true, false, 8f, true, "ui");
        pressAnyKey.SetActive(false);
    }

    public void RenderLevel(int lvl)
    {
        Eyes.current.Reset();
        swooshed = false;
        rolledOverCoin = false;
        catchedCoin = false;
        showinGoalPanel = true;
        tuto = lvl == 0;
        keyAvailable = true;
        Tuto.current.step = 0;
        if (tuto && PlayerPrefs.GetInt("tutoDone", 0) == 0)
            Tuto.current.Say();
        else
            tuto = false;
        currentLevel = lvl;
        Life.current.Reset();
        eatin = false;
        rollin = false;
        catchin = false;
        coinCatched = 0;
        CoinManager.current.UpdateCoins(Level.current.coinCatched);
        appleEated = 0;
        items.ForEach(item => Destroy(item));
        items.Clear();
        day.ForEach(d => Destroy(d.gameObject));
        day.Clear();
        noseIdle.SetActive(true);
        noseEating.SetTrigger("reset");
        noseEatingBG.SetTrigger("reset");
        belt.SetTrigger("stop");
        x = 0;
        string dayStr = lvl + 1 < 10 ? "0" + (lvl + 1) : "" + (lvl + 1);
        day = Font.current.Write("DAY " + dayStr + "/" + levels.Length, 6.5f, 5.25f, transform, 0);
        string lvlStr = levels[lvl].text;
        string[] strs = lvlStr.Split('|');
        string goalStr = strs[0];
        string[] itemsStr = strs[1].Split('-');
        Goal.current.Init(goalStr);
        foreach (string itemStr in itemsStr)
        {
            // APPLES
            if (itemStr == "A")
            {
                GameObject itemGO = Instantiate(redApple, new Vector3(x, -1f, 0), Quaternion.identity, transform);
                items.Add(itemGO);
                x += padding;
            }
            if (itemStr == "B")
            {
                GameObject itemGO = Instantiate(orangeApple, new Vector3(x, -1f, 0), Quaternion.identity, transform);
                items.Add(itemGO);
                x += padding;
            }
            if (itemStr == "C")
            {
                GameObject itemGO = Instantiate(greenApple, new Vector3(x, -1f, 0), Quaternion.identity, transform);
                items.Add(itemGO);
                x += padding;
            }
            if (itemStr == "D")
            {
                GameObject itemGO = Instantiate(yellowApple, new Vector3(x, -1f, 0), Quaternion.identity, transform);
                items.Add(itemGO);
                x += padding;
            }

            // COIN
            if (itemStr == "P")
            {
                GameObject itemGO = Instantiate(coin, new Vector3(x, -1f, 0), Quaternion.identity, transform);
                items.Add(itemGO);
                x += padding;
            }

            // ROTTEN APPLES
            if (itemStr == "a")
            {
                GameObject itemGO = Instantiate(rottenRedApple, new Vector3(x, -1f, 0), Quaternion.identity, transform);
                items.Add(itemGO);
                x += padding;
            }
            if (itemStr == "b")
            {
                GameObject itemGO = Instantiate(rottenOrangeApple, new Vector3(x, -1f, 0), Quaternion.identity, transform);
                items.Add(itemGO);
                x += padding;
            }
            if (itemStr == "c")
            {
                GameObject itemGO = Instantiate(rottenGreenApple, new Vector3(x, -1f, 0), Quaternion.identity, transform);
                items.Add(itemGO);
                x += padding;
            }
            if (itemStr == "d")
            {
                GameObject itemGO = Instantiate(rottenYellowApple, new Vector3(x, -1f, 0), Quaternion.identity, transform);
                items.Add(itemGO);
                x += padding;
            }

            // ENEMIES
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
        SFX.current.Play(SFX.Type.ROLL);
        GameObject rolledItem = items.Find(item => Vector3.Distance(new Vector3(0, -1f, 0), item.transform.position) < 0.1f);
        if (rolledItem != null && rolledItem.tag == "coin" && !catchedCoin)
            rolledOverCoin = true;
        belt.SetTrigger("roll");
        rollin = true;
        dests.Clear();
        foreach (GameObject item in items)
        {
            dests.Add(item.transform.position - new Vector3(5f, 0, 0));
        }
        catchedCoin = false;
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
            SFX.current.Play(SFX.Type.COIN);
            coinCatched++;
            Eyes.current.Do(Eyes.type.STAR);
            CoinManager.current.UpdateCoins(Level.current.coinCatched);
            catchedCoin = true;
        }
    }

    private void EatItem(GameObject item)
    {
        if (item == null)
            return;
        item.SetActive(false);
        if (item.tag == "bad")
        {
            Life.current.LowerFromItem();
            Eyes.current.Do(Eyes.type.NOPE);
        }
        else if (item.tag == "apple")
        {
            if (Goal.current.IsInGoal(item.GetComponent<Apple>().type))
            {
                Eyes.current.Do(Eyes.type.LOVE);
                Life.current.Add();
                appleEated++;
            }
            else
            {
                Eyes.current.Do(Eyes.type.NOPE);
                Life.current.LowerFromItem();
            }                
        } else if (item.tag == "coin")
        {
            Eyes.current.Do(Eyes.type.NOPE);
            Life.current.LowerFromItem();
            GameOver.current.DoGameOver(Reason.COIN_MISSED);
        }
    }

    private void Update()
    {
        if (state == State.INTRO)
        {
            if (showinGoalPanel)
            {
                if (!swooshed)
                {
                    SFX.current.Play(SFX.Type.SWOOSH_IN);
                    swooshed = true;
                }
                goalPanel.position = Vector3.MoveTowards(goalPanel.position, new Vector3(-1f, 0f, 0f), Time.deltaTime * speed);
                if (Vector3.Distance(goalPanel.position, new Vector3(-1f, 0f, 0f)) < 0.1f) {
                    goalPanel.position = new Vector3(-1f, 0f, 0f);
                    showinGoalPanel = false;
                }
            }

            if (!showinGoalPanel && !hidinGoalPanel)
            {
                timerGoal += Time.deltaTime;
                pressAnyKey.SetActive(true);
                if (timerGoal >= maxTimerGoal && Input.anyKeyDown)
                {
                    SFX.current.Play(SFX.Type.SWOOSH_OUT);
                    timerGoal = 0;
                    hidinGoalPanel = true;
                    pressAnyKey.SetActive(false);
                }
            }
            if (hidinGoalPanel)
            {
                goalPanel.position = Vector3.MoveTowards(goalPanel.position, new Vector3(-1f, -7f, 0f), Time.deltaTime * speed);
                if (Vector3.Distance(goalPanel.position, new Vector3(-1f, -7f, 0f)) < 0.1f)
                {
                    goalPanel.position = new Vector3(-1f, -7f, 0f);
                    hidinGoalPanel = false;
                    state = State.GAME;
                }
            }
        }
        if (state != State.GAME)
            return;
        if (!Input.anyKey)
        {
            keyAvailable = true;
        }
        if (tuto && keyAvailable)
        {
            if (Tuto.current.step == 1 && Input.anyKeyDown)
            {
                keyAvailable = false;
                SFX.current.Play(SFX.Type.SELECT);
                Tuto.current.Say();
            }
            else if (Tuto.current.step == 2 && Input.GetKeyDown(KeyCode.DownArrow))
            {
                keyAvailable = false;
                Eat();
                Tuto.current.Say();
                return;
            }
            else if (Tuto.current.step == 3 && Input.anyKeyDown)
            {
                keyAvailable = false;
                SFX.current.Play(SFX.Type.SELECT);
                Tuto.current.Say();
                return;
            }
            else if (Tuto.current.step == 4 && Input.anyKeyDown)
            {
                keyAvailable = false;
                SFX.current.Play(SFX.Type.SELECT);
                Tuto.current.Say();
                return;
            }
            else if (Tuto.current.step == 5 && Input.GetKeyDown(KeyCode.DownArrow))
            {
                keyAvailable = false;
                Eat();
                Tuto.current.Say();
                return;
            }
            else if (Tuto.current.step == 6 && Input.anyKeyDown)
            {
                keyAvailable = false;
                SFX.current.Play(SFX.Type.SELECT);
                Tuto.current.Say();
                return;
            }
            else if (Tuto.current.step == 7 && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow)))
            {
                keyAvailable = false;
                Tuto.current.Say();
                Roll();
                return;
            }
            else if (Tuto.current.step == 8 && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow)))
            {
                keyAvailable = false;
                Tuto.current.Say();
                Roll();
                return;
            }
            else if (Tuto.current.step == 9 && Input.GetKeyDown(KeyCode.UpArrow))
            {
                keyAvailable = false;
                Tuto.current.Say();
                CatchCoin();
                return;
            }
            else if (Tuto.current.step == 10 && Input.anyKeyDown)
            {
                keyAvailable = false;
                SFX.current.Play(SFX.Type.SELECT);
                Tuto.current.Say();
                return;
            }
        }
        if (!tuto && !rollin && !eatin && !catchin)
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
            if (Input.GetKeyDown(KeyCode.R))
            {
                state = State.INTRO;
                RenderLevel(currentLevel);
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
                if (rolledOverCoin)
                    GameOver.current.DoGameOver(Reason.COIN_MISSED);
                GameObject item = items.Find(item => Vector3.Distance(new Vector3(0, -1f, 0), item.transform.position) < 1f);
                if (item == null)
                {
                    if (coinCatched >= 3)
                    {
                        if (currentLevel + 1 < levels.Length)
                            PlayerPrefs.SetInt("day", currentLevel + 1);
                        DayComplete.current.DoDayComplete();
                    }
                    else
                        GameOver.current.DoGameOver(Reason.COINS);
                }
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
