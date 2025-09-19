using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    private void Awake()
    {
        instance = this;

        Application.targetFrameRate = 60;
    }

    public MainMenu MainMenu;

    public Gameplay Gameplay;

    public Level Levelplay;
}
