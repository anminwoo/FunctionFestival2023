using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleEnemy : Enemy
{
    protected override void Start()
    {
        base.Start();

        StartCoroutine(AttackCoroutine());
    }

    private void Update()
    {
        Move();
    }

    public override void Die()
    {
        base.Die();
        Destroy(gameObject);
    }

    protected override void Attack()
    {
        for (int i = -25; i <= 25; i += 50)
        {
            SpawnBullet(transform, new Vector3(0, 0, i));
        }
    }
}
