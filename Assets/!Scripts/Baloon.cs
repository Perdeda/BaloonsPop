using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Baloon : MonoBehaviour
{
    public delegate void OnBaloonPop(int reward = 1,Baloon baloon = null,bool byPlayer = false);
    public delegate void OnBaloonClick();
    public static event OnBaloonPop onBaloonPop;
    public static event OnBaloonClick onBaloonClick;

    [SerializeField]
    SpriteRenderer spr;
    [SerializeField]
    Rigidbody2D rb;
    int baloonHp = 4;
    int baloonScoreReward = 1;

    void OnEnable()
    {
        ChangeBaloonColor();
    }
    public void Init(int startHp,float gravityScale)
    {
        baloonHp = startHp;
        rb.gravityScale = gravityScale;
        baloonScoreReward = baloonHp;
        ChangeBaloonColor();
    }

    void ChangeBaloonColor()
    {
        switch (baloonHp)
        {
            case 1: spr.color = new Color(255, 0, 0); break;
            case 2: spr.color = new Color(0, 0, 255); break;
            case 3: spr.color = new Color(0, 255, 0); break;
            default: spr.color = new Color(0, 0, 0); break;
        }
    }
    public void TakeDamage(int damageToTake)
    {
        baloonHp -= damageToTake;
        if (baloonHp <= 0)
            Die();
        else
            ChangeBaloonColor();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //7 - BaloonPopCollider
        if (collision.gameObject.layer == 7)
        {
            DieNoReward();
        }
    }
    void Die()
    {
        onBaloonPop?.Invoke(baloonScoreReward,this,true);
    }
    void DieNoReward()
    {
        onBaloonPop?.Invoke(0, this, false);
    }
}
