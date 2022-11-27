using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    private SpriteRenderer sr;
    private bool activated = false;
    private Vector3 dir;
    private float speed = 20f;

    public void Init(Color color, Vector3 dir)
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = color;
        this.dir = dir;
        activated = true;
    }

    private void Update()
    {
        if (activated)
        {
            transform.Translate(dir * Time.deltaTime * speed);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a - Time.deltaTime * speed / 10f);
            if (sr.color.a <= 0f)
            {
                activated = false;
                Destroy(gameObject);
            }
        }
    }
}
