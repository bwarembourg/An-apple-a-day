using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public static Goal current;

    [SerializeField] private GameObject appleIcon;
    [SerializeField] private Sprite red;
    [SerializeField] private Sprite orange;
    [SerializeField] private Sprite green;
    [SerializeField] private Sprite yellow;
    [SerializeField] private Transform goalPanel;

    private List<GameObject> icons = new List<GameObject>();
    private string goals;

    private void Awake()
    {
        current = this;
    }

    public void Init(string goals)
    {
        this.goals = goals;
        icons.ForEach(icon => Destroy(icon.gameObject));
        icons.Clear();

        float posX = GetFirstPosX(goals.Length);
        for (int i = 0; i < goals.Length; i++)
        {
            GameObject icon = Instantiate(appleIcon, transform.position + new Vector3(posX, 0, 0), Quaternion.identity, transform);
            icon.GetComponent<SpriteRenderer>().sprite = GetSprite(goals[i]);
            icons.Add(icon);
            GameObject iconPanel = Instantiate(appleIcon, goalPanel.position + new Vector3(posX, 0, 0), Quaternion.identity, goalPanel);
            iconPanel.GetComponent<SpriteRenderer>().sprite = GetSprite(goals[i]);
            icons.Add(iconPanel);
            posX += 1f;
        }
    }

    private float GetFirstPosX(int count)
    {
        switch(count)
        {
            case 1: return 0f;
            case 2: return -0.5f;
            case 3: return -1f;
            default: return -1.5f;
        }
    }

    private Sprite GetSprite(char c)
    {
        switch(c)
        {
            case 'A': return red;
            case 'B': return orange;
            case 'C': return green;
            default: return yellow;
        }
    }

    public bool IsInGoal(string type)
    {
        for (int i = 0; i < goals.Length; i++)
        {
            if (goals[i] == type[0])
                return true;
        }
        return false;
    }
}
