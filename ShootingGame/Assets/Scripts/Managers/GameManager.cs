using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public struct PlayerInformation
{
    public string playerName;
    public int playerScore;

    public PlayerInformation(string name, int score)
    {
        playerName = name;
        playerScore = score;
    }
}

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public event Action WeaponMaxLevel;
    public event Action InitSkillCountAndCoolTime;
    public event Action InitDurability;
    public event Action InitFuel;
    public event Action MoveNextScene;

    public float ClearTime;
    public float Stage1ClearTime;
    public float Stage2ClearTime;
    public float TotalClearTime;

    public List<Bullet> bullets;
    
    public List<Enemy> enemies;

    public List<PlayerInformation> playerInformations;


    void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("GameManager is not null");
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            RemoveAllEnemy();
        }

        if(Input.GetKeyDown(KeyCode.F2))
        {
            WeaponMaxLevel?.Invoke();
        }

        if(Input.GetKeyDown(KeyCode.F3))
        {
            InitSkillCountAndCoolTime?.Invoke();
        }

        if(Input.GetKeyDown(KeyCode.F4))
        {
            InitDurability?.Invoke();
        }

        if(Input.GetKeyDown(KeyCode.F5))
        {
            InitFuel?.Invoke();
        }

        if(Input.GetKeyDown(KeyCode.F6)) // Scene ÀüÈ¯
        {
            if(SceneManager.GetActiveScene().buildIndex == 1)
            {
                MoveScene(2);
            }
            else if(SceneManager.GetActiveScene().buildIndex == 2)
            {
                MoveScene(1);
            }
        }

        ClearTime += Time.deltaTime;
    }

    public void RemoveAllBullet()
    {
        foreach(var bullet in bullets)
        {
            Destroy(bullet.gameObject);
        }
    }

    public void RemoveAllEnemy()
    {
        foreach(var enemy in enemies)
        {
            enemy.Die();
        }
    }

    public void DamageAllEnemy(int damage)
    {
        foreach(var enemy in enemies)
        {
            enemy.GetDamage(damage);
        }
    }

    public void MoveScene(int sceneNumber)
    {
        MoveNextScene?.Invoke();
        SceneManager.LoadScene(sceneNumber);
    }

    public void AddPlayerInformation(string name, int score)
    {
        playerInformations.Add(new PlayerInformation(name, score));

        int removeIndex = playerInformations.Count - 1;

        var sorted = playerInformations.OrderByDescending(info => info.playerScore).ToList();

        playerInformations = sorted;

        while(removeIndex >= 5)
        {
            playerInformations.RemoveAt(removeIndex--);
        }
    }

    public void RemovePlayer()
    {
        PlayerPrefs.DeleteAll();

        Stage1ClearTime = 0;
        Stage2ClearTime = 0;
    }
}
