using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Scenes")]
    public string mainMenuScene = "MainMenu";

    [Tooltip("Add all minigame scene names here")]
    public List<string> gameScenes = new List<string>();

    private List<string> shuffledScenes = new List<string>();
    private int currentGameIndex = 0;

    public bool soloMode = false;

    [Header("Round Settings")]
   
    public float roundTime = 60f;

    public float timer;
    public bool roundActive = false;
    public bool roundEnded = false;

    [Header("UI")]
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;

    public TextMeshProUGUI player1RoundsText;
    public TextMeshProUGUI player2RoundsText;

    public TextMeshProUGUI timerText;

    public GameObject Player2UI;
    public GameObject SplitScreenLine;


    private int player1Rounds = 0;
    private int player2Rounds = 0;


    public PlayerController player1;
    public PlayerController player2;

    public GameObject TutorialPanel;
    public GameObject WinnerPanel;
    public GameObject SettingsPanel;

    public TextMeshProUGUI winnerText;

    public bool isKeyboard = false;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(Instance.gameObject);
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        roundActive = false;
        roundEnded = false;

        if (scene.name == mainMenuScene)
        {
            GameObject startButtonObj = GameObject.FindWithTag("Challenge");

            if (startButtonObj != null)
            {
                Button startButton = startButtonObj.GetComponent<Button>();

                if (startButton != null)
                {
                    startButton.onClick.RemoveAllListeners();
                    startButton.onClick.AddListener(StartGame);
                }
            }

            soloMode = false;

            return;
        }

        FindPlayers();
    }

    void Update()
    {
        // Existing round timer update
        if (roundActive && !roundEnded)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
                EndRound();
        }

        UpdateUI();

        // Only allow settings toggle if NOT in main menu
        if (SceneManager.GetActiveScene().name != mainMenuScene)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (SettingsPanel != null)
                {
                    bool isActive = SettingsPanel.activeSelf;
                    if (isActive)
                        CloseSettings();
                    else
                        OpenSettings();
                }
            }
        }
    }




    public void StartGame()
    {
        ShuffleGames();

        if (shuffledScenes.Count > 0)
        {
            currentGameIndex = 0;
            SceneManager.LoadScene(shuffledScenes[currentGameIndex]);
        }
    }

    void ShuffleGames()
    {
        shuffledScenes = new List<string>(gameScenes);

        for (int i = 0; i < shuffledScenes.Count; i++)
        {
            int randomIndex = Random.Range(i, shuffledScenes.Count);
            string temp = shuffledScenes[i];
            shuffledScenes[i] = shuffledScenes[randomIndex];
            shuffledScenes[randomIndex] = temp;
        }
    }

    public void LoadNextScene()
    {
        currentGameIndex++;

        if (currentGameIndex >= shuffledScenes.Count)
        {
            SceneManager.LoadScene(mainMenuScene);
        }
        else
        {
            SceneManager.LoadScene(shuffledScenes[currentGameIndex]);
        }
    }

    public void LoadGame(string sceneName)
    {
        if (gameScenes.Contains(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Scene not in gameScenes list: " + sceneName);
        }
    }

    public void LoadSoloGame(string sceneName)
    {
        if (gameScenes.Contains(sceneName))
        {
            soloMode = true;
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Scene not in gameScenes list: " + sceneName);
        }
    }

    public void StartRound()
    {
        timer = roundTime;
        roundActive = true;
    }

    void FindPlayers()
    {
        if (SceneManager.GetActiveScene().name == mainMenuScene)
            return;

        PlayerController[] players = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);

        player1 = null;
        player2 = null;

        foreach (var p in players)
        {
            if (p.name.Contains("1"))
                player1 = p;

            if (p.name.Contains("2"))
                player2 = p;
            PlayerMovement pm = p.gameObject.GetComponent<PlayerMovement>();
            pm.useKeyboard = isKeyboard;
        }

        if (player1 == null || player2 == null)
            Debug.LogWarning("Players not found!");

        if (soloMode && player2 != null)
        {
            player2.transform.parent.gameObject.SetActive(false);
            if (Player2UI != null)
                Player2UI.SetActive(false);
            if (SplitScreenLine != null)
                SplitScreenLine.SetActive(false);
            


            player1.transform.parent.GetChild(0).GetComponent<Camera>().rect = new Rect(0f, 0f, 1f, 1f);

        }
        else if (!soloMode && player2 != null)
        {
            player2.transform.parent.gameObject.SetActive(true);
            if (Player2UI != null)
                Player2UI.SetActive(true);
            if (SplitScreenLine != null)
                SplitScreenLine.SetActive(true);

            player1.transform.parent.GetChild(0).GetComponent<Camera>().rect = new Rect(0f, 0f, 0.5f, 1f);
        }

        if (TutorialPanel != null)
            TutorialPanel.SetActive(true);
    }

    void EndRound()
    {
        roundEnded = true;
        roundActive = false;

        PlayerController winner = null;

        if (player1 != null && player2 != null)
        {
            if (player1.playerScore > player2.playerScore)
            {
                winner = player1;
                player1Rounds++;
            }
            else if (player2.playerScore > player1.playerScore)
            {
                winner = player2;
                player2Rounds++;
            }
        }

        if (winner != null)
            winnerText.text = "Winner: " + winner.name;
        else
            winnerText.text = "Draw";

        if (WinnerPanel != null)
            WinnerPanel.SetActive(true);
    }


    void UpdateUI()
    {
        if (player1 != null)
            player1ScoreText.text = player1.playerScore.ToString();

        if (player2 != null)
            player2ScoreText.text = player2.playerScore.ToString();

        player1RoundsText.text = player1Rounds.ToString();
        player2RoundsText.text = player2Rounds.ToString();

        float time = GetTimer();
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }


    public float GetTimer()
    {
        return Mathf.Max(0f, timer);
    }


    // Call this to open settings
    public void OpenSettings()
    {
        if (SettingsPanel != null)
            SettingsPanel.SetActive(true);

        Time.timeScale = 0f; // pause the game
    }

    // Call this to close settings
    public void CloseSettings()
    {
        if (SettingsPanel != null)
            SettingsPanel.SetActive(false);

        Time.timeScale = 1f; // resume the game
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // ensure game is not paused
        SceneManager.LoadScene(mainMenuScene);
    }

}


