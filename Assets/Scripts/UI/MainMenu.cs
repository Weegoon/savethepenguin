using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void OnOpen()
    {
        gameObject.SetActive(true);
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }

    public void OnPlayButtonClick()
    {
        UIController.instance.Gameplay.OnOpen();
        UIController.instance.Levelplay.OnStartGame();
        OnClose();
    }
}
