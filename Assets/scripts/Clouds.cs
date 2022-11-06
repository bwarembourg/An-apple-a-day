using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{
    [SerializeField] private SpriteRenderer cloud;
    [SerializeField] private Sprite[] sprites;

    private List<SpriteRenderer> clouds = new List<SpriteRenderer>();
    private List<float> ys = new List<float>();
    private float timer = 0f;
    private float maxTimer = 1f;
    private float speed = 5f;

    private void Start()
    {
        ys.Add(4.5f);
        ys.Add(3.5f);
        ys.Add(2.5f);
        ys.Add(1.5f);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= maxTimer)
        {
            timer = 0f;
            float y = ys[Random.Range(0, ys.Count)];
            SpriteRenderer cloudGo = Instantiate(cloud, new Vector3(-8f, y, 0), Quaternion.identity, transform);
            cloudGo.sprite = sprites[Random.Range(0, sprites.Length)];
            clouds.Add(cloudGo);
        }

        foreach(SpriteRenderer cloud in clouds)
        {
            if (cloud != null)
            {
                cloud.transform.Translate(Time.deltaTime * speed, 0, 0);
                if (cloud.transform.position.x > 12f)
                {
                    Destroy(cloud.gameObject);
                }
            }
        }
    }
}
