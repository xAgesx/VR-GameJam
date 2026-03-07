using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Round Settings")]
    public float roundTime = 60f; // editable in inspector
    public int mainMenuSceneIndex = 0; // index of main menu
    public int firstGameSceneIndex = 1; // index of first game
    public int lastGameSceneIndex = 3;  // index of last game

    public float timer;
    public bool roundActive = false;
    public bool roundEnded = false;

    public PlayerController player1;
    public PlayerController player2;

    public GameObject TutorialPanel;
    public GameObject WinnerPanel;

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
        // Reset round state
        roundActive = false;
        roundEnded = false;

        if (scene.buildIndex == mainMenuSceneIndex)
        {
            // Find the button tagged "Challenge"
            GameObject startButtonObj = GameObject.FindWithTag("Challenge");

            if (startButtonObj != null)
            {
                Button startButton = startButtonObj.GetComponent<Button>();

                if (startButton != null)
                {
                    // Clear previous listeners just in case
                    startButton.onClick.RemoveAllListeners();

                    // Assign StartGame to button
                    startButton.onClick.AddListener(StartGame);

                    Debug.Log("Start button assigned to StartGame()");
                }
                else
                {
                    Debug.LogWarning("Object with tag 'Challenge' does not have a Button component!");
                }
            }
            else
            {
                Debug.LogWarning("No GameObject found with tag 'Challenge' in Main Menu!");
            }

            return; // no players in main menu, skip the rest
        }

        // For game scenes, find players and start round
        FindPlayers();
        //StartRound();
    }

    void Update()
    {
        if (!roundActive || roundEnded)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            EndRound();
        }
    }


    // Called from Main Menu button
    public void StartGame()
    {
        SceneManager.LoadScene(firstGameSceneIndex);
    }

    // Starts the timer once in a game scene
    public void StartRound()
    {
        timer = roundTime;
        roundActive = true;
        Debug.Log("Round started! Timer running.");
    }

    void FindPlayers()
    {
        // Skip main menu
        if (SceneManager.GetActiveScene().buildIndex == mainMenuSceneIndex)
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

        if (player1 == null || player2 == null )
            Debug.LogWarning("Player1 or Player2 not found in scene!");

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
            WinnerPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Winner: " + winner.name;
        else
            WinnerPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Draw";

        WinnerPanel.SetActive(true);

    }
    public void LoadNextScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;

        if (currentScene >= lastGameSceneIndex)
            SceneManager.LoadScene(mainMenuSceneIndex);
        else
            SceneManager.LoadScene(currentScene + 1);
    }

    // Optional: return current timer for UI display
    public float GetTimer()
    {
        return Mathf.Max(0f, timer);
    }
}
