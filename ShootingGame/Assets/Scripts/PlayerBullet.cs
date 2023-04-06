using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    protected override void Start()
    {
        base.Start();
        moveDir = Vector3.up;
    }
}
