using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gameplay : MonoBehaviour
{
    [SerializeField] GameObject levelPlay;

    [SerializeField] GameObject tutorialObj;

    [SerializeField] List<GameObject> heartObjs;

    [SerializeField] Text scoreText;

    public bool tutorialShowed;

    public void OnOpen()
    {
        gameObject.SetActive(true);
        levelPlay.SetActive(true);

        if (!tutorialShowed)
            ShowTutorial();
    }

    public void OnClose()
    {
        levelPlay.SetActive(false);
        gameObject.SetActive(false);

    }

    void ShowTutorial()
    {
        tutorialObj.SetActive(true);
        Utility.Delay(this, delegate
        {
            tutorialShowed = true;
            tutorialObj.SetActive(false);
        }, 3f);
    }

    public void UpdateLifeUI()
    {
        for (int i = 0; i < heartObjs.Count; i++)
        {
            heartObjs[i].SetActive((i + 1) <= UIController.instance.Levelplay.CurrentLife);
        }
    }

    public void UpdateScoreUI()
    {
        scoreText.text = "Score \n" + UIController.instance.Levelplay.CurrentScore.ToString();

        if (UIController.instance.Levelplay.CurrentScore > UIController.instance.Levelplay.HighScore)
            UIController.instance.Levelplay.HighScore = UIController.instance.Levelplay.CurrentScore;
    }
}
