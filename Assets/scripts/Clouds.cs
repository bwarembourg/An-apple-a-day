using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{
    [SerializeField] private SpriteRenderer cloud;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] float[] ys;
    [SerializeField] private float maxTimer = 1f;
    [SerializeField] float x;

    private List<SpriteRenderer> clouds = new List<SpriteRenderer>();
    private float timer = 0f;
    private float speed = 5f;


    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= maxTimer)
        {
            timer = 0f;
            float y = ys[Random.Range(0, ys.Length)];
            SpriteRenderer cloudGo = Instantiate(cloud, transform.position + new Vector3(x, y, 0), Quaternion.identity, transform);
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
