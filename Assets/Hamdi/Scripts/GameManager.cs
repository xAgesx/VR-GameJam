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

    [Header("Round Settings")]
    public float roundTime = 60f;

    public float timer;
    public bool roundActive = false;
    public bool roundEnded = false;

    public PlayerController player1;
    public PlayerController player2;

    public GameObject TutorialPanel;
    public GameObject WinnerPanel;

    public TextMeshProUGUI winnerText;

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
            Destroy(gameObject);
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

            return;
        }

        FindPlayers();
    }

    void Update()
    {
        if (!roundActive || roundEnded)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
            EndRound();
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
        }

        if (player1 == null || player2 == null)
            Debug.LogWarning("Players not found!");

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
                winner = player1;
            else if (player2.playerScore > player1.playerScore)
                winner = player2;
        }

        if (winner != null)
            winnerText.text = "Winner: " + winner.name;
        else
            winnerText.text = "Draw";

        if (WinnerPanel != null)
            WinnerPanel.SetActive(true);
    }

    public float GetTimer()
    {
        return Mathf.Max(0f, timer);
    }
}
