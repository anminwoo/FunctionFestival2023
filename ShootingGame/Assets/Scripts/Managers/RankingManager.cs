using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] ranks;

    public void ShowRanking()
    {
        int i = 0;
        foreach (var playerInfo in GameManager.instance.playerInformations)
        {
            ranks[i++].text = $"{i}.{playerInfo.playerName}: {playerInfo.playerScore}";
        }
    }
}
