using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] SpriteRenderer coin1;
    [SerializeField] SpriteRenderer coin2;
    [SerializeField] SpriteRenderer coin3;
    [SerializeField] Sprite coinIcon;
    [SerializeField] Sprite coinIconEmpty;

    public static CoinManager current;

    private void Awake()
    {
        current = this;
    }

    public void UpdateCoins(int coins)
    {
        coin1.sprite = coins >= 1 ? coinIcon : coinIconEmpty;
        coin2.sprite = coins >= 2 ? coinIcon : coinIconEmpty;
        coin3.sprite = coins >= 3 ? coinIcon : coinIconEmpty;
    }
}
