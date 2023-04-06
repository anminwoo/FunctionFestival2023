using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemy : Enemy
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
}
