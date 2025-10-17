using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Text _highscoreText;

    private void Start()
    {
        _highscoreText.text = "HighScore : " + UIController.instance.Levelplay.HighScore.ToString();
    }

    public void OnOpen()
    {
        gameObject.SetActive(true);

        _highscoreText.text = "HighScore : " + UIController.instance.Levelplay.HighScore.ToString();
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
