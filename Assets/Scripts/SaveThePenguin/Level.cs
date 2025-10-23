using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public GameObject HeartPrefab;

    public GameObject TrapPrefab;

    public Penguin PlayerPenguin;

    public GameObject TargetPenguin;

    public GameObject TargetPenguin_Back;

    public List<Transform> PenguinPaths;

    public Laddle Laddle;

    public float TimeToSpawnItem = 3f;

    public int CurrentScore;

    public int CurrentLife;

    public int scoreIndex;

    public bool IsReachPerfectScore;

    bool isLose;
    public Vector3 originalPenguinPos;

    Coroutine coSpawnItem;

    public int HighScore
    {
        get => PlayerPrefs.GetInt("HighScore", 0);
        set => PlayerPrefs.SetInt("HighScore", value);
    }

    public void OnStartGame()
    {
        gameObject.SetActive(true);
        CurrentLife = 3;
        CurrentScore = 0;
        scoreIndex = 0;
        isLose = false;

        itemDrops.Clear();

        TargetPenguin.transform.position = originalPenguinPos;
        TargetPenguin_Back.SetActive(false);

        PlayerPenguin.ResetToOriginal();

        UIController.instance.Gameplay.UpdateLifeUI();
        UIController.instance.Gameplay.UpdateScoreUI();

        if (coSpawnItem != null)
        {
            StopCoroutine(coSpawnItem);
            coSpawnItem = null;
        }
        coSpawnItem = StartCoroutine(IESpawnItem());

        SoundManager.instance.PlayBackgroundSound(SoundManager.instance.listMusicGamePlay);
    }

    public void OnEndGame()
    {
        gameObject.SetActive(false);
    }

    IEnumerator IESpawnItem()
    {
        yield return new WaitUntil(() => UIController.instance.Gameplay.tutorialShowed);
        while (!isLose)
        {
            yield return new WaitForSeconds(GetTimeToSpawnItem());
            yield return new WaitUntil(() => !PlayerPenguin.isDie);
            SpawnItem();
        }
    }

    float GetTimeToSpawnItem()
    {
        if (CurrentScore < 100f)
            return 4;
        else if (CurrentScore < 300f)
            return 3f;
        else if (CurrentScore < 500f)
            return 2f;
        else if (CurrentScore < 700f)
            return 1f;
        else return 0.5f;
    }

    void SpawnItem()
    {
        float ranInt = Random.value;
        if (ranInt >= 0.5f)
        {
            GameObject obj = Instantiate(HeartPrefab, transform);
            obj.transform.position = new Vector3(Random.Range(-2.2f, 2.2f), 2.5f, 0);
        }
        else
        {
            GameObject obj = Instantiate(TrapPrefab, transform);
            obj.transform.position = new Vector3(Random.Range(-2.2f, 2.2f), 2.5f, 0);
            itemDrops.Add(obj);
        }
    }

    public List<GameObject> itemDrops = new List<GameObject>();

    public void EarnItem()
    {
        CurrentScore += 10;
        scoreIndex += 1;
        UIController.instance.Gameplay.UpdateScoreUI();

        if (!IsReachPerfectScore)
            Laddle.PlaceLaddle();

        if (scoreIndex == 10)
        {
            scoreIndex = 0;
            ReachToPerfectScore();
        }
    }

    void ReachToPerfectScore()
    {
        IsReachPerfectScore = true;

        Vector3[] vecs = new Vector3[PenguinPaths.Count];
        for (int i = 0; i < PenguinPaths.Count; i++) 
        {
            vecs[i] = PenguinPaths[i].position;
        }

        TargetPenguin.transform.DOPath(vecs, 3f, pathMode: PathMode.Ignore).OnComplete(delegate
        {
            CurrentScore += 100;
            UIController.instance.Gameplay.UpdateScoreUI();

            TargetPenguin.transform.position = originalPenguinPos;
            Laddle.PlaceLaddle();
            IsReachPerfectScore = false;
        }).OnWaypointChange(index =>
        {
            Debug.Log("index " + index);

            if (index == 1)
            {
                TargetPenguin_Back.SetActive(true);
            }
            else TargetPenguin_Back.SetActive(false);
        });
    }

    public void CollisionWithTrap()
    {
        CurrentLife -= 1;
        UIController.instance.Gameplay.UpdateLifeUI();

        for (int i = itemDrops.Count - 1; i >= 0; i--)
        {
            Destroy(itemDrops[i]);
        }

        if (CurrentLife <= 0)
        {
            CurrentLife = 0;
            Utility.Delay(this, delegate
            {
                SoundManager.instance.PlayAudioClip(SoundManager.instance.loseSound);
                Utility.Delay(this, delegate
                {
                    DoLose();
                }, 3f);
            }, 1f);
            
        }
    }

    public void DoLose()
    {
        
        UIController.instance.MainMenu.OnOpen();
        UIController.instance.Gameplay.OnClose();
    }
}
