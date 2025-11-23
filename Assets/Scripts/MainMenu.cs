using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    static internal MainMenu instance;
    [SerializeField] private GameObject MainMenuPanel;
    [SerializeField] private GameObject levelPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject onBtn;
    [SerializeField] private GameObject offBtn;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource backgroundAudio;
    [SerializeField] private AudioClip btnSound;

    void Awake()
    {
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MainMenuPanel.SetActive(true);
        levelPanel.SetActive(false);
        settingsPanel.SetActive(false);

        if (PlayerPrefs.GetInt("Sounds", 0) == 1)
        {
            backgroundAudio.Play();
        }
        else
        {
            backgroundAudio.Stop();
        }

        backgroundAudio.Play();
        OffBtn();
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
             SoundSettingBtn();
            AdsManager.Instance.ShowInterstialAd();
        }
        else if (name == "Settings")
        {
            settingsPanel.SetActive(true);
             SoundSettingBtn();
        }
        else if (name == "Cut")
        {
            settingsPanel.SetActive(false);
             SoundSettingBtn();
        }
        else
        {
            Debug.Log("Click Again");
        }
    }

    internal void BackgroundAudio()
    {
        backgroundAudio.Stop();
    }

    public void OnBtn()
    {
        onBtn.SetActive(false);
        offBtn.SetActive(true);
        PlayerPrefs.SetInt("Sounds", 0);
        SoundSetting();
        SoundSettingBtn();
    }

    public void OffBtn()
    {
        onBtn.SetActive(true);
        offBtn.SetActive(false);
        PlayerPrefs.SetInt("Sounds", 1);
        SoundSetting();
    }

    void SoundSetting()
    {
        if (PlayerPrefs.GetInt("Sounds", 0) == 1)
        {
            backgroundAudio.Play();
        }
        else
        {
            backgroundAudio.Stop();
        }
    }
    internal void SoundSettingBtn()
    {
        if (PlayerPrefs.GetInt("Sounds", 0) == 1)
        {
            audioSource.PlayOneShot(btnSound);
        }
    }
}
