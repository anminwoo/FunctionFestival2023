using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WarningText : MonoBehaviour
{
    [SerializeField] private GameObject warningObject;
    [SerializeField] private TextMeshProUGUI warningText;

    private float activeTime = 1.5f;

    public void PopUpMessage(string content)
    {
        SetMessage(content);

        if (warningObject.activeSelf)
        {
            return;
        }

        OnMessage();
        StartCoroutine(PopUp());
    }

    private void OnMessage()
    {
        warningObject.SetActive(true);
    }

    private void OffMessage()
    {
        warningObject.SetActive(false);
    }

    private void SetMessage(string content)
    {
        warningText.text = content;
    }

    private IEnumerator PopUp()
    {
        yield return new WaitForSeconds(activeTime);
        OffMessage();
    }
}
