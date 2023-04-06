using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    string name = null;

    [SerializeField] private TextMeshProUGUI totalTimeText;
    [SerializeField] private TextMeshProUGUI totalScoreText;

    private void Start()
    {
        int totalClearTime = Mathf.RoundToInt(GameManager.instance.Stage1ClearTime + GameManager.instance.Stage2ClearTime);

        totalTimeText.text = $"Total ClearTime: {totalClearTime / 60}m {totalClearTime % 60}s";
        totalScoreText.text = $"TotalScore: {PlayerPrefs.GetInt("Score")}";
    }

    private void Update()
    {
        name = inputField.text;
        if(name.Length != 0 && Input.GetKeyDown(KeyCode.Return))
        {
            GameManager.instance.AddPlayerInformation(name, PlayerPrefs.GetInt("Score"));
            GameManager.instance.RemovePlayer();
            GameManager.instance.MoveScene(0);
        }
    }
}
