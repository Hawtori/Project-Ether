using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioGameState { None, Gameplay, GameOver, MainMenu, Paused};

public enum AudioSoundState { None, LevelStart, LevelLose, MainMenu, Paused, LevelWin };

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;
    static bool bIsInitialized = false;

    [Header("Startup SoundBanks")]
    [SerializeField] private List<AK.Wwise.Bank> SoundBanks;

    
    [Header("Game States")]
    [SerializeField] private AK.Wwise.State Game_Gameplay;
    [SerializeField] private AK.Wwise.State Game_GameOver;
    [SerializeField] private AK.Wwise.State Game_MainMenu;
    [SerializeField] private AK.Wwise.State Game_Paused;
    [SerializeField] private AK.Wwise.State Game_None;

    private AudioGameState currentGameState;

    [Header("Sound States")]
    [SerializeField] private AK.Wwise.State Sound_LevelStart;
    [SerializeField] private AK.Wwise.State Sound_LevelLose;
    [SerializeField] private AK.Wwise.State Sound_MainMenu;
    [SerializeField] private AK.Wwise.State Sound_Paused;
    [SerializeField] private AK.Wwise.State Sound_LevelWin;
    [SerializeField] private AK.Wwise.State Sound_None;

    private AudioSoundState currentSoundState;

    [Header("Wwise Sound Events")]
    [SerializeField] public AK.Wwise.Event MainSound_Play;
    [SerializeField] public AK.Wwise.Event MainSound_Stop;

    private void Awake()
    {
        Initialize();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetWwiseGameState(AudioGameState.MainMenu);
        SetWwiseMusicState(AudioSoundState.MainMenu);
        MainSound_Play.Post(gameObject);
    }

    void Initialize()
    {
        if(Instance == null)
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
            LoadSoundBanks();
        }


        SetWwiseGameState(AudioGameState.None);

        bIsInitialized = true;
    }

    void LoadSoundBanks()
    {
        if (SoundBanks.Count > 0)
        {
            foreach (AK.Wwise.Bank bank in SoundBanks)
                bank.Load();
            Debug.Log("Startup Soundbanks have been loaded.");
        }
        else
            Debug.LogError("Soundbanks list is empty. Are banks assigned to the Audio Manager?");
    }

    public void SetWwiseGameState(AudioGameState GameState)
    {
        if (GameState == currentGameState)
        {
            Debug.Log("GameState is already " + GameState + ".");
            return;
        }
        switch(GameState)
        {
            default:
            case (AudioGameState.MainMenu):
                Game_MainMenu.SetValue();
                break;
            case (AudioGameState.Gameplay):
                Game_Gameplay.SetValue();
                break;
            case (AudioGameState.GameOver):
                Game_MainMenu.SetValue();
                break;
            case (AudioGameState.Paused):
                Game_Paused.SetValue();
                break;
            case (AudioGameState.None):
                Game_None.SetValue();
                break;
        }

        Debug.Log("New Wwise GameState: " + GameState + ".");

        currentGameState = GameState;
    }

    public void SetWwiseMusicState(AudioSoundState SoundState)
    {
        if (SoundState == currentSoundState)
        {
            Debug.Log("SoundState is already " + SoundState + ".");
            return;
        }
        switch (SoundState)
        {
            default:
            case (AudioSoundState.MainMenu):
                Sound_MainMenu.SetValue();
                break;
            case (AudioSoundState.LevelStart):
                Sound_LevelStart.SetValue();
                break;
            case (AudioSoundState.LevelLose):
                Sound_LevelLose.SetValue();
                break;
            case (AudioSoundState.Paused):
                Sound_Paused.SetValue();
                break;
            case (AudioSoundState.None):
                Sound_None.SetValue();
                break;
            case (AudioSoundState.LevelWin):
                Sound_LevelWin.SetValue();
                break;
        }

        Debug.Log("New Wwise SoundState: " + SoundState + ".");

        currentSoundState = SoundState;
    }
}
