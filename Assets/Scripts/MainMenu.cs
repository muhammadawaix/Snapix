using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject MainMenuPanel;
    [SerializeField] private GameObject levelPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
            MainMenuPanel.SetActive(true);
            levelPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void Button(string name)
    {
        if (name == "Play")
        {
            MainMenuPanel.SetActive(false);
            levelPanel.SetActive(true);
        }
        else if (name == "Settings")
        {
            Debug.Log("Setting Button Clicked");
        }
    }
}
