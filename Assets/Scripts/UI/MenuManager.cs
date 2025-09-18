using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject settingsPanel;

    public void OpenSettings()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
}
