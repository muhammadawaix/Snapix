using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlay : MonoBehaviour
{
    static public GamePlay instance;
    [SerializeField] private GameObject WinPanel;

    void Awake()
    {
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        WinPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void YouWin()
    {
        if (WinPanel == null)
        {
            Debug.LogError("WinPanel is NULL in GamePlay.YouWin() - assign it in the Inspector.");
            return;
        }

        WinPanel.SetActive(true);
    }
    

    public void Button(string name)
    {
        if (name == "Home")
        {
            SceneManager.LoadScene(0);
        }
        else if (name == "Next")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
