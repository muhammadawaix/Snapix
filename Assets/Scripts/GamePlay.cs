using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlay : MonoBehaviour
{
    static public GamePlay instance;
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource backgroundAudio;
    [SerializeField] private AudioClip btnSound;
    [SerializeField] private AudioClip dragSound;
    [SerializeField] private AudioClip winSound;

    void Awake()
    {
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.GetInt("Sounds", 0) == 1)
        {
            backgroundAudio.Play();
        }
        else
        {
            backgroundAudio.Stop();
        }


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
        WinSound();
        WinPanel.SetActive(true);
        LevelWin();
    }
    

    public void Button(string name)
    {
        if (name == "Home")
        {
            SoundSettingBtn();
            StartCoroutine(HomeBtn());
        }
        else if (name == "Next")
        {
            SoundSettingBtn();
            StartCoroutine(NextBtn());
        }
    }

    IEnumerator HomeBtn()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(0);
    }
    IEnumerator NextBtn()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LevelWin()
    {

        
        int newCurrent = PlayerPrefs.GetInt("CurrentLevelNumber", 1) + 1;
        PlayerPrefs.SetInt("CurrentLevelNumber", newCurrent);
        PlayerPrefs.SetInt("currentImageNumber", PlayerPrefs.GetInt("currentImageNumber", 0) + 1);

        
        int justCleared = newCurrent - 1;
        PlayerPrefs.SetInt("PreviousLevelIndex", justCleared);
        PlayerPrefs.SetInt("LevelClearIndex", justCleared);
        
    }

    internal void SoundSettingBtn()
    {
        if (PlayerPrefs.GetInt("Sounds", 0) == 1)
        {
            audioSource.PlayOneShot(btnSound);
        }
    }
    internal void DragSound()
    {
        if (PlayerPrefs.GetInt("Sounds", 0) == 1)
        {
            audioSource.PlayOneShot(dragSound);
        }
    }

    internal void WinSound()
    {
        if (PlayerPrefs.GetInt("Sounds", 0) == 1)
        {
            audioSource.PlayOneShot(winSound);
        }
    }
}
