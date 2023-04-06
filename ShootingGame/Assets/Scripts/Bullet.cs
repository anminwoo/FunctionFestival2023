using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public int damage;
    private float moveSpeed;

    protected Vector3 moveDir;

    protected virtual void Start()
    {
        GameManager.instance.bullets.Add(this);
    }

    protected void Update()
    {
        transform.position += moveDir * (moveSpeed * Time.deltaTime);
    }

    protected void OnDestroy()
    {
        GameManager.instance.bullets.Remove(this);
    }

    public void InitBullet(int damage, float speed)
    {
        this.damage = damage;
        moveSpeed = speed;
    }
}
