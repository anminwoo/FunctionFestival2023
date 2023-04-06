using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity
{
    public void GetDamage(int damage);

    public void Die();
}
