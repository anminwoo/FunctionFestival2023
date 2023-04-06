using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IEntity
{

    private Rigidbody2D rigidbody;
    private Animator animator;

    [SerializeField] private UIManager uiManager;
    [SerializeField] private WarningText warning;
    [Space]

    [SerializeField] public int score;
    [Space]

    [Header("Durability")]
    private int maxDurability = 100;
    [SerializeField] private int durability;
    [Space]

    [Header("Fuel")]
    private float maxFuel = 100f;
    [SerializeField] private float fuel;
    [Space]

    [SerializeField] private float moveSpeed;

    [Header("Weapon")]
    [SerializeField] private int weaponLevel;
    private int maxWeaponLevel = 4;

    private int baseWeaponDamage = 5;
    [SerializeField] private int weaponDamage;
    [SerializeField] private float attackTime;
    private float attackCoolTime;
    [Space]

    [SerializeField] private Bullet bullet;
    [SerializeField] private float bulletSpeed;

    [Header("Invincible")]
    [SerializeField] private bool isInvincible;
    [SerializeField] private float invincibleTimer;


    [Header("Skill")]
    [SerializeField] private int maxSkillCount;
    [SerializeField] private float skillCoolTime;
    [Space]
    [SerializeField] private int repairSkillCount;
    [SerializeField] private int repairSkillRepairAmount;
    [SerializeField] private float repairSkillTimer;
    [Space]
    [SerializeField] private int bombSkillCount;
    [SerializeField] private int bombDamage;
    [SerializeField] private float bombSkillTimer;
    [SerializeField] private GameObject bombGameObject;

    [Header("Item")]
    [SerializeField] private float invincibleTime;
    [SerializeField] private int repairItemRepairAmount;
    [SerializeField] private float fuelItemAmount;
    [SerializeField] private int itemScore;

    [Header("AudioClip")]
    [SerializeField] private AudioClip healClip;
    [SerializeField] private AudioClip powerUpClip;
    [SerializeField] private AudioClip hitClip;


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        GameManager.instance.ClearTime = 0f;
        GameManager.instance.WeaponMaxLevel += Instance_WeaponMaxLevel;
        GameManager.instance.InitSkillCountAndCoolTime += Instance_InitSkillCountAndCoolTime;
        GameManager.instance.InitDurability += Instance_InitDurability;
        GameManager.instance.InitFuel += Instance_InitFuel;
        GameManager.instance.MoveNextScene += Instance_MoveNextScene;


        LoadPlayer();
        DebugPlayerState();

        uiManager.ChangeDurabilityFill((float)durability / maxDurability);
        uiManager.ChangeFuelFill(fuel / maxFuel);

        uiManager.ChangeRepairSkillCount(repairSkillCount);
        uiManager.ChangeBombSkillCount(bombSkillCount);

        uiManager.ChangeRepairSkillFill(repairSkillTimer / skillCoolTime);
        uiManager.ChangeBombSkillFill(bombSkillTimer / skillCoolTime);

        SetWeaponDamage(weaponLevel);

        uiManager.ChangeScore(score);

        StartCoroutine(RepairSkillCoolDownCoroutine());
        StartCoroutine(BombSkillCoolDownCoroutine());
        StartCoroutine(FlyCoroutine());
    }

    private void Instance_MoveNextScene()
    {
        SavePlayer();
    }

    private void Instance_InitFuel()
    {
        FillFuel(maxFuel);

        warning.PopUpMessage("Initialize fuel");
    }

    private void Instance_InitDurability()
    {
        Repair(maxDurability);

        warning.PopUpMessage("Initialize durability");
    }

    private void Instance_InitSkillCountAndCoolTime()
    {
        InitSkillsCount();
        InitSkillsCoolTime();

        warning.PopUpMessage("Initialize skills count, coolTime");
    }

    private void Instance_WeaponMaxLevel()
    {
        for(int i = weaponLevel; i <= maxWeaponLevel; i++)
        {
            UpgradeWeapon();
        }
        SoundManager.instance.PlaySound(powerUpClip);
        warning.PopUpMessage("Weapon Max Level");
    }

    void Update()
    {
        Move();

        if(Input.GetKeyDown(KeyCode.K))
        {
            LossFuel(30f);

        }

        if(attackCoolTime < attackTime)
        {
            attackCoolTime += Time.deltaTime;
        }

        if(Input.GetKey(KeyCode.Space))
        {
            Attack();
        }

        if(Input.GetKeyDown(KeyCode.B))
        {
            UseBombSkill();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            UseRepairSkill();
        }
    }

    private void OnDestroy()
    {
        GameManager.instance.WeaponMaxLevel -= Instance_WeaponMaxLevel;
        GameManager.instance.InitSkillCountAndCoolTime -= Instance_InitSkillCountAndCoolTime;
        GameManager.instance.InitDurability -= Instance_InitDurability;
        GameManager.instance.InitFuel -= Instance_InitFuel;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case "Enemy":
                collision.GetComponent<IEntity>().GetDamage(weaponDamage);

                break;
            case "EnemyBullet":
                var bullet = collision.GetComponent<Bullet>();
                GetDamage(bullet.damage);
                Destroy(bullet.gameObject);
                break;
            case "WeaponUpgradeItem":
                UpgradeWeapon();
                SoundManager.instance.PlaySound(powerUpClip);
                AddScore(itemScore);
                Destroy(collision.gameObject);
                break;
            case "InvincibleItem":
                if(isInvincible)
                {
                    invincibleTimer = invincibleTime;
                    Destroy(collision.gameObject);
                    return;
                }
                AddScore(itemScore);
                Destroy(collision.gameObject);
                StartCoroutine(InvincibleCoroutine());
                break;
            case "RepairItem":
                Repair(repairItemRepairAmount);
                AddScore(itemScore);
                Destroy(collision.gameObject);
                break;
            case "FuelItem":
                FillFuel(fuelItemAmount);
                AddScore(itemScore);
                Destroy(collision.gameObject);
                break;
        }
    }
    private void Move()
    {
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        switch(horizontal)
        {
            case -1:
                animator.SetBool("IsRightPressed", true);
                animator.SetBool("IsLeftPressed", false);
                break;
            case 0:
                animator.SetBool("IsRightPressed", false);
                animator.SetBool("IsLeftPressed", false);
                break;
            case 1:
                animator.SetBool("IsRightPressed", false);
                animator.SetBool("IsLeftPressed", true);
                break;
        }

        Vector2 moveDir = new Vector2(horizontal, vertical);
        rigidbody.velocity = moveDir * moveSpeed;
    }

    private void Attack()
    {
        if (attackCoolTime < attackTime)
        {
            return;
        }
        var spawnedBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        spawnedBullet.InitBullet(weaponDamage, bulletSpeed);
        attackCoolTime = 0;
    }

    public void UseRepairSkill()
    {
        if(repairSkillTimer < skillCoolTime || repairSkillCount == 0)
        {
            warning.PopUpMessage("you can't use now.");
            return;
        }

        uiManager.ChangeRepairSkillCount(--repairSkillCount);
        Repair(repairSkillRepairAmount);

        repairSkillTimer = 0f;
        StartCoroutine(RepairSkillCoolDownCoroutine());
    }

    private IEnumerator RepairSkillCoolDownCoroutine()
    {
        uiManager.ChangeRepairSkillFill(repairSkillTimer / skillCoolTime);

        while(repairSkillTimer < skillCoolTime)
        {
            repairSkillTimer += Time.deltaTime;
            uiManager.ChangeRepairSkillFill(repairSkillTimer / skillCoolTime);
            yield return null;
        }

        repairSkillTimer = skillCoolTime;
        uiManager.ChangeRepairSkillFill(repairSkillTimer / skillCoolTime);
    }

    public void Repair(int repairAmount)
    {
        durability = Mathf.Clamp(durability + repairAmount, 0, maxDurability);
        uiManager.ChangeDurabilityFill((float)durability / maxDurability);
        SoundManager.instance.PlaySound(healClip);
    }

    public void UseBombSkill()
    {
        if(bombSkillTimer < skillCoolTime || bombSkillCount == 0)
        {
            warning.PopUpMessage("you can't use now.");
            return;
        }

        uiManager.ChangeBombSkillCount(--bombSkillCount);

        Instantiate(bombGameObject, Vector3.zero, Quaternion.identity);

        GameManager.instance.RemoveAllBullet();
        GameManager.instance.DamageAllEnemy(bombDamage);

        bombSkillTimer = 0f;
        StartCoroutine(BombSkillCoolDownCoroutine());
    }

    private IEnumerator BombSkillCoolDownCoroutine()
    {
        uiManager.ChangeBombSkillFill(bombSkillTimer / skillCoolTime);

        while (bombSkillTimer < skillCoolTime)
        {
            bombSkillTimer += Time.deltaTime;
            uiManager.ChangeBombSkillFill(bombSkillTimer / skillCoolTime);
            yield return null;
        }

        bombSkillTimer = skillCoolTime;
        uiManager.ChangeBombSkillFill(bombSkillTimer / skillCoolTime);
    }

    public void InitSkillsCoolTime()
    {
        repairSkillTimer = skillCoolTime;
        bombSkillTimer = skillCoolTime;
    }

    public void InitSkillsCount()
    {
        repairSkillCount = maxSkillCount;
        uiManager.ChangeRepairSkillCount(repairSkillCount);
        bombSkillCount = maxSkillCount;
        uiManager.ChangeBombSkillCount(bombSkillCount);
    }

    public void GetDamage(int damage)
    {
        if(isInvincible)
        {
            return;
        }

        durability = Mathf.Clamp(durability - damage, 0, maxDurability);
        uiManager.ChangeDurabilityFill((float)durability / maxDurability);
        SoundManager.instance.PlaySound(hitClip);
        if(durability <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        
        GameManager.instance.MoveScene(3);
    }

    public void FillFuel(float fillAmount)
    {
        fuel = Mathf.Clamp(fuel + fillAmount, 0, maxFuel);
        uiManager.ChangeFuelFill(fuel / maxFuel);
    }

    public void LossFuel(float loseAmount)
    {
        fuel = Mathf.Clamp(fuel - loseAmount, 0, maxFuel);
        uiManager.ChangeFuelFill(fuel / maxFuel);

        if(fuel <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlyCoroutine()
    {
        LossFuel(0.5f);
        yield return new WaitForSeconds(1f);
        StartCoroutine(FlyCoroutine());
    }

    public void UpgradeWeapon()
    {
        if(weaponLevel >= maxWeaponLevel)
        {
            return;
        }
        weaponLevel++;
        SetWeaponDamage(weaponLevel);
    }

    public void SetWeaponDamage(int weaponLevel)
    {
        weaponDamage = 5 + weaponLevel * 3;
    }

    public IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;
        invincibleTimer = invincibleTime;
        warning.PopUpMessage("Invincible");
        while(invincibleTimer > 0)
        {
            invincibleTimer -= Time.deltaTime;
            yield return null;
        }

        warning.PopUpMessage("Invincible off");
        isInvincible = false;
        invincibleTimer = 0f;
    }

    public void AddScore(int score)
    {
        this.score += score;
        uiManager.ChangeScore(this.score);
    }

    public void ResultScore() // 더 추가할 거 있으면 넣기
    {
        AddScore(durability * 100);
    }

    private void LoadPlayer()
    {
        durability = PlayerPrefs.GetInt("Durability", maxDurability);
        fuel = PlayerPrefs.GetFloat("Fuel", maxFuel);

        weaponLevel = PlayerPrefs.GetInt("WeaponLevel", 1);

        repairSkillCount = PlayerPrefs.GetInt("RepairSkillCount", maxSkillCount);
        bombSkillCount = PlayerPrefs.GetInt("BombSkillCount", maxSkillCount);

        repairSkillTimer = PlayerPrefs.GetFloat("RepairSkillTimer", skillCoolTime);
        bombSkillTimer = PlayerPrefs.GetFloat("BombSkillTimer", skillCoolTime);

        score = PlayerPrefs.GetInt("Score", 0);

    }

    private void SavePlayer()
    {
        PlayerPrefs.SetInt("Durability", durability);
        PlayerPrefs.SetFloat("Fuel", fuel);

        PlayerPrefs.SetInt("WeaponLevel", weaponLevel);

        PlayerPrefs.SetInt("RepairSkillCount", repairSkillCount);
        PlayerPrefs.GetInt("BombSkillCount", bombSkillCount);

        PlayerPrefs.SetFloat("RepairSkillTimer", repairSkillTimer);
        PlayerPrefs.SetFloat("BombSkillTimer", bombSkillTimer);

        PlayerPrefs.SetInt("Score", score);
    }

    private void DebugPlayerState()
    {
        Debug.Log($"Durability = {durability}");
        Debug.Log($"fuel = {fuel}");
        Debug.Log($"weaponLevel = {weaponLevel}");
        Debug.Log($"repairSkillCount = {repairSkillCount}");
        Debug.Log($"BombSkillCount = {bombSkillCount}");
        Debug.Log($"repairSkillTimer = {repairSkillTimer}");
        Debug.Log($"bombSkillTimer = {bombSkillTimer}");
        Debug.Log($"score: {score}");
    }
}
