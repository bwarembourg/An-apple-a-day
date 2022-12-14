using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuto : MonoBehaviour
{
    public static Tuto current;
    [SerializeField] private GameObject bubblePrefab;
    [SerializeField] private GameObject pressAnyKey;

    private List<SpriteRenderer> text = new List<SpriteRenderer>();
    private GameObject bubble = null;

    public int step = 0;

    private void Awake()
    {
        current = this;
    }


    private void Start()
    {
        Font.current.Write("Press any key to continue...", -8f, 5f, pressAnyKey.transform, 50, true, false, 8f, true, "ui");
        pressAnyKey.SetActive(false);
    }

    public void Say()
    {
        step++;
        if (bubble != null)
            Destroy(bubble);
        text.ForEach(t => Destroy(t.gameObject));
        text.Clear();
        pressAnyKey.SetActive(false);
        switch (step)
        {
            default:
                text = Font.current.Write(GetStr(step), -9.75f, -1f, transform, 20, true, true, -0.75f, true);
                bubble = Instantiate(bubblePrefab, new Vector3(-5.25f, -2.25f, 0), Quaternion.identity, transform);
                pressAnyKey.SetActive(true);
                break;
            case 2:
                text = Font.current.Write(GetStr(step), -4.5f, 2f, transform, 20, true, true, 4.5f, true);
                bubble = Instantiate(bubblePrefab, new Vector3(0, 1.5f, 0), Quaternion.identity, transform);
                break;
            case 3:
                text = Font.current.Write(GetStr(step), 0.75f, -1f, transform, 20, true, true, 9.75f, true);
                bubble = Instantiate(bubblePrefab, new Vector3(5.25f, -2.25f, 0), Quaternion.identity, transform);
                pressAnyKey.SetActive(true);
                break;
            case 4:
                text = Font.current.Write(GetStr(step), 0.75f, -1f, transform, 20, true, true, 9.75f, true);
                bubble = Instantiate(bubblePrefab, new Vector3(5.25f, -2.25f, 0), Quaternion.identity, transform);
                pressAnyKey.SetActive(true);
                break;
            case 5:
                break;
            case 6:
                text = Font.current.Write(GetStr(step), -4.5f, 2f, transform, 20, true, true, 4.5f, true);
                bubble = Instantiate(bubblePrefab, new Vector3(0, 1.5f, 0), Quaternion.identity, transform);
                pressAnyKey.SetActive(true);
                break;
            case 7:
                text = Font.current.Write(GetStr(step), -4.5f, 2f, transform, 20, true, true, 4.5f, true);
                bubble = Instantiate(bubblePrefab, new Vector3(0, 1.5f, 0), Quaternion.identity, transform);
                break;
            case 8:
                break;
            case 9:
                text = Font.current.Write(GetStr(step), -4.5f, 2f, transform, 20, true, true, 4.5f, true);
                bubble = Instantiate(bubblePrefab, new Vector3(0, 1.5f, 0), Quaternion.identity, transform);
                break;
            case 10:
                text = Font.current.Write(GetStr(step), -4.5f, 2.5f, transform, 20, true, true, 4.5f, true);
                bubble = Instantiate(bubblePrefab, new Vector3(0, 1.5f, 0), Quaternion.identity, transform);
                pressAnyKey.SetActive(true);
                break;
            case 11:
                Level.current.tuto = false;
                PlayerPrefs.SetInt("tutoDone", 1);
                break;
        }
    }

    private string GetStr(int step)
    {
        switch (step)
        {
            default:  return "An apple a day... keeps the doctor away! Charlie needs red apples today.";
            case 2: return "Press DOWN arrow key to eat the apple.";
            case 3: return "Warning! Charlie will get sick if she eats too much apples.";
            case 4: return "She will get hungry if she doesn't eat enough apples. Keep your health balanced!";
            case 6: return "Some items are not apples. you better not eat them!";
            case 7: return "Use LEFT or RIGHT arrow key to avoid them.";
            case 9: return "Wow! Charlie loves coins! Press UP arrow key to catch it!";
            case 10: return "Catch 3 coins to complete the day!";
        }
    }
}
