using UnityEngine;

public class ControllersToggle : MonoBehaviour
{
    public GameObject toggleOn;
    public GameObject toggleOff;

    public void ToggleKeyboard()
    {
        GameManager.Instance.isKeyboard = !GameManager.Instance.isKeyboard;

        bool value = !GameManager.Instance.isKeyboard;

        toggleOn.SetActive(value);
        toggleOff.SetActive(!value);
    }
}
