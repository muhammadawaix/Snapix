using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlay : MonoBehaviour
{
    static public GamePlay instance;
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private TextMeshProUGUI levelText;

    void Awake()
    {
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // PlayerPrefs.SetInt("CurrentLevelNumber", 1);
        // PlayerPrefs.SetInt("PreviousLevelIndex", 0);
        // PlayerPrefs.SetInt("LevelClearIndex", 0);
        WinPanel.SetActive(false);
        
        if (!PlayerPrefs.HasKey("CurrentLevelNumber")) PlayerPrefs.SetInt("CurrentLevelNumber", 1);
        if (!PlayerPrefs.HasKey("PreviousLevelIndex")) PlayerPrefs.SetInt("PreviousLevelIndex", 0);

        levelText.text = "Level " + PlayerPrefs.GetInt("CurrentLevelNumber", 1).ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
 
    public void YouWin()
    {
        WinPanel.SetActive(true);
        LevelWin();
    }
    

    public void Button(string name)
    {
        if (name == "Home")
        {
            SceneManager.LoadScene(0);
        }
        else if (name == "Next")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void LevelWin()
    {

        // Advance to next level
        int newCurrent = PlayerPrefs.GetInt("CurrentLevelNumber", 1) + 1;
        PlayerPrefs.SetInt("CurrentLevelNumber", newCurrent);
        PlayerPrefs.SetInt("currentImageNumber", PlayerPrefs.GetInt("currentImageNumber", 0) + 1);

        // The level just completed is newCurrent - 1
        int justCleared = newCurrent - 1;
        PlayerPrefs.SetInt("PreviousLevelIndex", justCleared);
        PlayerPrefs.SetInt("LevelClearIndex", justCleared);
        
    }
}
