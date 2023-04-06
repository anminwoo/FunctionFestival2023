using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Boss : Enemy
{
    [SerializeField] private UIManager uiManager;

    [SerializeField] private Transform[] fireTransforms;

    bool isDead;

    protected override void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        base.Start();
        OnAppear();
    }

    private void OnAppear()
    {
        StartCoroutine(MoveCoroutine());
        uiManager.ActivateBossDurabilityGage();
        StartCoroutine(AttackCoroutine());
    }

    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);
        StartCoroutine(ChangeSpriteColor());
        uiManager.ChangeBossDurabilityFill((float)durability / maxDurability);
    }

    public override void Die()
    {
        if(isDead)
        {
            return;
        }

        isDead = true;
        var player = FindObjectOfType<Player>();
        player.StopAllCoroutines();
        StopAllCoroutines();
        uiManager.ChangeBossDurabilityFill(0);
        base.Die();
        StartCoroutine(BossDieCoroutine());
    }

    public IEnumerator BossDieCoroutine()
    {
        float timer = 0f;
        for(int i = 0; i < 10; i++)
        {
            Explose();
            yield return new WaitForSeconds(0.5f);
        }
        uiManager.ActivateResultPanel();
        Destroy(gameObject);
    }

    protected override void Explose()
    {
        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
        float rangeX = collider.bounds.size.x;
        float rangeY = collider.bounds.size.y;

        float randomRangeX = Random.Range(-rangeX / 2, rangeX / 2);
        float randomRangeY = Random.Range(-rangeY / 2, rangeY / 2);

        Vector3 spawnPos = new Vector3(randomRangeX, randomRangeY, 0);

        SoundManager.instance.PlaySound(deadClip);
        Instantiate(explosedObject, transform.localPosition + spawnPos, Quaternion.identity);
    }

    private IEnumerator MoveCoroutine()
    {
        float timer = 0f;
        while (timer <= 2.5f)
        {
            transform.position += Vector3.down * (moveSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    protected override void Attack()
    {
        int randomNumber = Random.Range(0, 5);
        switch(randomNumber)
        {
            case 0:
                StartCoroutine(Pattern0());
                break;
            case 1:
                StartCoroutine(Pattern1());
                StartCoroutine(Pattern2());
                break;
            case 2:
                StartCoroutine(Pattern3());
                break;
            case 3:
                StartCoroutine(Pattern4());
                break;
            case 4:
                StartCoroutine(Pattern5());
                break;
            case 5:
                StartCoroutine(Pattern6());
                break;
        }
    }

    private IEnumerator Pattern0() // 가운데 한 발
    {
        for (int i = 0; i < 3; i++)
        {
            SpawnBullet();
            yield return new WaitForSeconds(0.3f);
        }
    }

    private IEnumerator Pattern1() // 왼쪽부터 모든 발사구에서 한발씩
    {
        foreach(var firePos in fireTransforms)
        {
            SpawnBullet(firePos, Vector3.zero);
            yield return new WaitForSeconds(0.3f);
        }
    }

    private IEnumerator Pattern2() // 오른쪽부터 모든 발사구에서 한발씩
    {
        for(int i = fireTransforms.Length - 1; i >= 0; i--)
        {
            SpawnBullet(fireTransforms[i], Vector3.zero);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator Pattern3() // 모든 발사구에서 3발씩
    {
        foreach (var firePos in fireTransforms)
        {
            for(int i = 0; i < 3; i++)
            {
                SpawnBullet(firePos, Vector3.zero);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.7f);
        }
    }

    private IEnumerator Pattern4() // 원 모양 발사
    {
        for(int i = -180; i < 180; i+=10)
        {
            SpawnBullet(transform, new Vector3(0, 0, i));
        }
        yield return new WaitForSeconds(2f);
    }

    private IEnumerator Pattern5() // 시간 간격 두고 모든 발사구에서 원 모양 발사
    {
        foreach(var firePos in fireTransforms)
        {
            for(int i = -180; i < 180; i+= 20)
            {
                SpawnBullet(firePos, new Vector3(0, 0, i));
            }
            yield return new WaitForSeconds(2f);
        }
    }

    private IEnumerator Pattern6()
    {
        for(int i = 0; i < 70; i += 14)
        {
            SpawnBullet(fireTransforms[0], new Vector3(0, 0, i));
        }

        for (int i = 0; i < 5; i++)
        {
            SpawnBullet(fireTransforms[1], Vector3.zero);
        }

        for (int i = 0; i > -70; i -= 14)
        {
            SpawnBullet(fireTransforms[2], new Vector3(0, 0, i));
        }

        yield return new WaitForSeconds(2f);
    }
}
