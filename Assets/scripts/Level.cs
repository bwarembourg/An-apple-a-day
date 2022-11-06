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

    [Header("ITEMS")]
    [SerializeField] private GameObject redApple;
    [SerializeField] private GameObject chocolate;
    [SerializeField] private GameObject poo;


    private float speed = 20f;
    private float padding = 5f;
    private float x = 0;
    private List<GameObject> items = new List<GameObject>();
    private List<Vector3> dests = new List<Vector3>();
    private bool rollin = false;
    private bool eatin = false;
    private bool ate = false;

    private float timerEat = 0f;
    private float maxTimerEat = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        RenderLevel(0);
    }

    public void RenderLevel(int lvl)
    {
        items.ForEach(item => Destroy(item));
        items.Clear();
        x = 0;
        string lvlStr = levels[0].text;
        string[] itemsStr = lvlStr.Split('-');
        foreach(string itemStr in itemsStr)
        {
            if (itemStr == "A")
            {
                GameObject itemGO = Instantiate(redApple, new Vector3(x, -1f, 0), Quaternion.identity, transform);
                items.Add(itemGO);
                x += padding;
            }
            if (itemStr == "c")
            {
                GameObject itemGO = Instantiate(chocolate, new Vector3(x, -1f, 0), Quaternion.identity, transform);
                items.Add(itemGO);
                x += padding;
            }
            if (itemStr == "p")
            {
                GameObject itemGO = Instantiate(poo, new Vector3(x, -1f, 0), Quaternion.identity, transform);
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

    private void EatItem(GameObject item)
    {
        Debug.Log(item);
        Debug.Log(items[0].transform.position.x + "-" + items[0].transform.position.y);
        if (item == null)
            return;
        item.SetActive(false);
        Life.current.Add();
    }

    private void Update()
    {
        if (!rollin && !eatin)
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
    }

}
