using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IEntity
{
    SpriteRenderer renderer;

    [SerializeField] protected Sprite hitSprite;
    [SerializeField] protected GameObject explosedObject;
    [SerializeField] protected AudioClip deadClip;

    [SerializeField] protected int maxDurability;
    [SerializeField] protected int durability;
    [SerializeField] protected int damage;

    [SerializeField] protected float moveSpeed;

    [SerializeField] protected int score;

    [SerializeField] protected GameObject[] items;

    [Header("Shooter Only")]

    [SerializeField] protected EnemyBullet bullet;
    [SerializeField] protected float bulletSpeed;

    [SerializeField] protected float attackTime;

    protected virtual void Start()
    {
        renderer = GetComponent<SpriteRenderer>();

        durability = maxDurability;
        GameManager.instance.enemies.Add(this);
    }

    protected virtual void OnDestroy()
    {
        GameManager.instance.enemies.Remove(this);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerBullet"))
        {
            var bullet = collision.GetComponent<Bullet>();
            GetDamage(bullet.damage);
            Destroy(bullet.gameObject);

        }

        else if(collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<IEntity>();
            player.GetDamage(damage);
        }
    }

    public virtual void GetDamage(int damage)
    {
        durability = Mathf.Clamp(durability - damage, 0, maxDurability);
        StartCoroutine(ChangeSpriteWhenHit());
        if(durability <= 0)
        {
            Die();
        }
    }

    protected virtual void Explose()
    {
        Instantiate(explosedObject, transform.position, Quaternion.identity);
    }

    public virtual void Die()
    {
        var player = FindObjectOfType<Player>();

        player.AddScore(score);

        Explose();

        if(Random.Range(0, 10) > 4)
        {
            SpawnItem();
        }
    }

    private IEnumerator ChangeSpriteWhenHit()
    {
        Sprite normalSprite = renderer.sprite;
        renderer.sprite = hitSprite;
        yield return new WaitForSeconds(0.2f);
        renderer.sprite = normalSprite;
    }

    protected IEnumerator ChangeSpriteColor()
    {
        renderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        renderer.color = Color.white;
    }

    protected void SpawnItem()
    {
        int randomNumber = Random.Range(0, 10);
        switch(randomNumber)
        {
            case < 4:
                InstantiateItem(0);
                break;
            case < 6:
                InstantiateItem(1);
                break;
            case < 8:
                InstantiateItem(2);
                break;
            default:
                InstantiateItem(3);
                break;
        }
    }

    protected void InstantiateItem(int itemIndex) => Instantiate(items[itemIndex], transform.position, Quaternion.identity);

    protected virtual IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(attackTime);
        Attack();
        StartCoroutine(AttackCoroutine());
    }

    protected virtual void Attack()
    {
        SpawnBullet();
    }

    protected void SpawnBullet()
    {
        var spawnedBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        spawnedBullet.InitBullet(damage, bulletSpeed);
    }

    protected virtual void SpawnBullet(Transform spawnPos, Vector3 rotation)
    {
        var spawnedBullet = Instantiate(bullet, spawnPos.position, Quaternion.Euler(rotation));
        spawnedBullet.InitBullet(damage, bulletSpeed);
    }

    protected void Move()
    {
        transform.position += Vector3.down * (moveSpeed * Time.deltaTime);
    }
}