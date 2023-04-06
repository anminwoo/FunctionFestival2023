using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemy : Enemy
{

    [SerializeField] private Transform firePositionL;
    [SerializeField] private Transform firePositionR;

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
        SpawnBullet(firePositionL, Vector3.zero);
        SpawnBullet(firePositionR, Vector3.zero);
    }
}
