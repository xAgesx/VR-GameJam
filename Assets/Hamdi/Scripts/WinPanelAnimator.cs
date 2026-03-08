using UnityEngine;
using System.Collections;
using TMPro; // Ensure you have TextMeshPro installed

public class WinPanelAnimator : MonoBehaviour
{
    [Header("UI References")]
    public GameObject imageCharacter;
    public GameObject chatBubble;
    public TextMeshProUGUI chatText;

    [Header("Animation Settings")]
    public float initialDelay = 1.0f;
    public float typingSpeed = 0.05f;
    public string[] winMessage ;

    void OnEnable()
    {
        // Reset states
        imageCharacter.SetActive(false);
        chatBubble.SetActive(false);
        chatText.text = "";

        StartCoroutine(PlayWinSequence());
    }

    IEnumerator PlayWinSequence()
    {
        // 1. Wait for the initial delay
        yield return new WaitForSeconds(initialDelay);

        // 2. Appear the Character and Chat Bubble
        imageCharacter.SetActive(true);
        chatBubble.SetActive(true);

        // 3. Start the Typewriter effect
        yield return StartCoroutine(TypeText(winMessage[Random.Range(0,winMessage.Length)]));
    }

    IEnumerator TypeText(string message)
    {
        chatText.text = "";
        foreach (char letter in message.ToCharArray())
        {
            chatText.text += letter;
            // Optional: Play a tiny "blip" SFX here if you have one
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}