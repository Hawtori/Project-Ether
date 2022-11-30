using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public static InputManager Instance;

    private bool bIsInitialized = false;

    [Header("Event Input Key Assignments")]
    [SerializeField] private KeyCode CheckGameState;
    [SerializeField] private KeyCode CheckSoundState;
    [SerializeField] private KeyCode StartMainSound;
    [SerializeField] private KeyCode StopMainSound;

    [Header("GameState Input Key Assignments")]
    [SerializeField] private KeyCode Game_MainMenu;
    [SerializeField] private KeyCode Game_Gameplay;
    [SerializeField] private KeyCode Game_GameOver;
    [SerializeField] private KeyCode Game_Paused;

    [Header("SoundState Input Key Assignments")]
    [SerializeField] private KeyCode Sound_MainMenu;
    [SerializeField] private KeyCode Sound_LevelStart;
    [SerializeField] private KeyCode Sound_LevelWin;
    [SerializeField] private KeyCode Sound_LevelLose;
    [SerializeField] private KeyCode Sound_PauseMenu;

    private void Awake()
    {
        Initialize();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputs();

    }

    void Initialize()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.LogWarning("Audio Manager already exists! Destroying new instance.");
            Destroy(this);
        }

        if (!bIsInitialized)
        {
            bIsInitialized = true;
        }
    }

    void CheckInputs()
    {
        if (!Input.anyKey)
            return;


        //Game States
        if(Input.GetKeyDown(Game_MainMenu))
        {
            SoundManager.Instance.SetWwiseGameState(AudioGameState.MainMenu);
        }
        if (Input.GetKeyDown(Game_Gameplay))
        {
            SoundManager.Instance.SetWwiseGameState(AudioGameState.Gameplay);
        }
        if (Input.GetKeyDown(Game_GameOver))
        {
            SoundManager.Instance.SetWwiseGameState(AudioGameState.GameOver);
        }
        if (Input.GetKeyDown(Game_Paused))
        {
            SoundManager.Instance.SetWwiseGameState(AudioGameState.Paused);
        }

        //Sound States
        if (Input.GetKeyDown(Sound_MainMenu))
        {
            SoundManager.Instance.SetWwiseMusicState(AudioSoundState.MainMenu);
        }
        if (Input.GetKeyDown(Sound_LevelStart))
        {
            SoundManager.Instance.SetWwiseMusicState(AudioSoundState.LevelStart);
        }
        if (Input.GetKeyDown(Sound_LevelLose))
        {
            SoundManager.Instance.SetWwiseMusicState(AudioSoundState.LevelLose);
        }
        if (Input.GetKeyDown(Sound_LevelWin))
        {
            SoundManager.Instance.SetWwiseMusicState(AudioSoundState.LevelWin);
        }
        if (Input.GetKeyDown(Sound_PauseMenu))
        {
            SoundManager.Instance.SetWwiseMusicState(AudioSoundState.Paused);
        }
    }
}
