using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI infoText;

    void OnEnable()
    {
        UpdateTutorialText();
    }

    void UpdateTutorialText()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        switch (sceneName)
        {
            case "TrashAndCans":
                titleText.text = "Game 1 – Trash Sorting";
                infoText.text =
@"Trash is scattered everywhere and it's your job to clean it up.
Walk over trash items to pick them up and carry them above your head.

Find the correct trash bin and throw the trash inside.

<color=""green""> Correct bin: +10 points</color>
<color=""red""> Wrong bin: −30 points</color>";
                break;

            case "RescueTurtles":
                titleText.text = "Game 2 – Free the Turtles";
                infoText.text =
@"Some turtles are trapped in nets.
Stand near them to free them.

<color=""green""> Free a turtle: +50 points</color>";
                break;

            case "FixThePipes":
                titleText.text = "Game 3 – Fix the Pipes";
                infoText.text =
@"Pipes are damaged and leaking water.
Stand near damaged holes to fix them.

<color=""green""> Fix a hole: +20 points</color>";
                break;

            default:
                titleText.text = "Tutorial";
                infoText.text = "Follow the instructions to learn the game.";
                break;
        }
    }
}
