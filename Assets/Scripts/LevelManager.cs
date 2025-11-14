using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Button[] levels;
    [SerializeField] private LevelBluePrint[] levelBluePrints;
    int currentLevelNumber;
    int currentImageNumber;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach(Button level in levels)
        {
            level.interactable = false;
        }
        int clearLevel = PlayerPrefs.GetInt("LevelClearIndex",0);
        Debug.Log("Clear Level: " + clearLevel);

        for(int n = 0; n<clearLevel+1; n++)
        {
            levels[n].interactable = true;
            Debug.Log(n);
        }

        for (int i = 0; i < levels.Length; i++)
        {
            int levelIndex = i;
            levels[i].onClick.AddListener(()=> ClickButton(levelIndex));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void ClickButton(int levelIndex)
    {
        MainMenu.instance.SoundSettingBtn();

        currentLevelNumber = levelBluePrints[levelIndex].levelNumber;
        PlayerPrefs.SetInt("CurrentLevelNumber", currentLevelNumber);

        currentImageNumber = levelBluePrints[levelIndex].imageNumber;
        PlayerPrefs.SetInt("currentImageNumber", currentImageNumber);
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(1);
    }
}
