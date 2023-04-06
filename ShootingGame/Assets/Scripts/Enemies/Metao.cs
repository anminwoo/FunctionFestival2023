using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metao : Enemy
{
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
