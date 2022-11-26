using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleBG : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] applePrefabs;
    [SerializeField] private float maxTimer = 1f;

    private List<SpriteRenderer> apples = new List<SpriteRenderer>();
    private float timer = 0f;
    private float speed = 15f;

    private bool activated = false;

    private void Update()
    {
        if (!activated)
            return;

        timer += Time.deltaTime;
        if (timer >= maxTimer)
        {
            timer = 0f;
            float x = Random.Range(-12f, 12f);
            SpriteRenderer applePrefab = applePrefabs[Random.Range(0, applePrefabs.Length)];
            SpriteRenderer apple = Instantiate(applePrefab, transform.position + new Vector3(x, 8f, 0), Quaternion.identity, transform);
            apple.sortingLayerName = "uistate";
            apple.transform.Rotate(0, 0, Random.Range(-30f, 30f));
            apple.sortingOrder = 4;
            apples.Add(apple);
        }

        foreach (SpriteRenderer apple in apples)
        {
            if (apple != null)
            {
                apple.transform.Translate(0, -Time.deltaTime * speed, 0);
                if (apple.transform.position.y < -8f + transform.position.y)
                {
                    Destroy(apple.gameObject);
                }
            }
        }
    }

    public void Activate()
    {
        activated = true;
    }

    public void Reset()
    {
        apples.ForEach(a => {
            if (a != null && a.gameObject != null)
                Destroy(a.gameObject);
        });
        apples.Clear();
        activated = false;
    }
}
