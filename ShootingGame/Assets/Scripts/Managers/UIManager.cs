using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Player UI")]
    [SerializeField] private Image durabilityFill;
    [SerializeField] private Image fuelFill;

    [SerializeField] private Image repairSkill;
    [SerializeField] private Image bombSkill;

    [SerializeField] private TextMeshProUGUI repairSkillCount;
    [SerializeField] private TextMeshProUGUI bombSkillCount;

    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Boss UI")]
    [SerializeField] private GameObject bossDurabilityGage;
    [SerializeField] private Image bossDurabilityFill;

    [Header("Result UI")]
    [SerializeField] private GameObject resultPanel;

    public void ChangeDurabilityFill(float fill) => durabilityFill.fillAmount = fill;

    public void ChangeFuelFill(float fill) => fuelFill.fillAmount = fill;

    public void ChangeRepairSkillFill(float fill) => repairSkill.fillAmount = fill;

    public void ChangeBombSkillFill(float fill) => bombSkill.fillAmount = fill;

    public void ChangeRepairSkillCount(int skillCount) => repairSkillCount.text = skillCount.ToString();

    public void ChangeBombSkillCount(int skillCount) => bombSkillCount.text = skillCount.ToString();

    public void ChangeScore(int score) => scoreText.text = "Score: " + score;

    public void ActivateBossDurabilityGage() => bossDurabilityGage.SetActive(true);

    public void ChangeBossDurabilityFill(float fill) => bossDurabilityFill.fillAmount = fill;

    public void ActivateResultPanel() => resultPanel.SetActive(true);
}
