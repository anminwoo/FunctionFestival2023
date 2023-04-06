using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScene : MonoBehaviour
{
    public void GoMainMenu()
    {
        GameManager.instance.MoveScene(0);
    }
}
