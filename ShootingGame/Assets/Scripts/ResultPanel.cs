using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI clearTimeText;
    [SerializeField] private TextMeshProUGUI clearScoreText;

    private void OnEnable()
    {
        int clearTime = Mathf.RoundToInt(GameManager.instance.ClearTime);
        clearTimeText.text = $"ClearTime: {clearTime / 60}m {clearTime % 60}s";
        var player = FindObjectOfType<Player>();
        player.ResultScore();
        clearScoreText.text = $"Score: {player.score}";

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            GameManager.instance.Stage1ClearTime = clearTime;
        }
        else if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            GameManager.instance.Stage2ClearTime = clearTime;
        }
    }

    public void MoveScene(int sceneIndex)
    {
        GameManager.instance.MoveScene(sceneIndex);
    }
}
