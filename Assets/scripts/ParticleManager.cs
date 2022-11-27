using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager current;

    [SerializeField] Particle particlePrefab;
    [SerializeField] Color red;
    [SerializeField] Color yellow;
    [SerializeField] float maxTimerPart;
    [SerializeField] float maxTimer;

    private List<Particle> particles = new List<Particle>();
    private bool activated = false;
    private float minX = 0;
    private float mediumX = 0;
    private float minY = 0;
    private float maxX = 0;
    private float mediumY = 0;
    private float maxY = 0;
    private float timerPart = 0;
    private float timer = 0;
    private Color color;

    private void Awake()
    {
        current = this;
    }

    public void Life()
    {
        mediumX = 6.625f;
        mediumY = -4.625f;
        minX = mediumX - 1.5f;
        maxX = mediumX + 1.5f;
        minY = mediumY - 0.5f;
        maxY = mediumY + 0.5f;
        color = red;
        activated = true;
    }

    public void Coin()
    {
        mediumX = -8f;
        mediumY = 4.875f;
        minX = mediumX - 1.5f;
        maxX = mediumX + 1.5f;
        minY = mediumY + 0.5f;
        maxY = mediumY - 0.5f;
        color = yellow;
        activated = true;
    }

    private void Update()
    {
        if (activated)
        {
            timer += Time.deltaTime;
            timerPart += Time.deltaTime;
            if (timerPart >= maxTimerPart)
            {
                float x = Random.Range(minX, maxX);
                float y = Random.Range(minY, maxY);
                Vector3 dir = new Vector3(x, y, 0) - new Vector3(mediumX, mediumY, 0);
                dir.Normalize();
                Particle particle = Instantiate(particlePrefab, new Vector3(x, y, 0f), Quaternion.identity, transform);
                particle.Init(color, dir);
                particles.Add(particle);
                timerPart = 0;
            }
            if (timer >= maxTimer)
            {
                timer = 0;
                timerPart = 0;
                activated = false;
            }
        }
    }

}
