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
        // PlayerPrefs.SetInt("LevelClearIndex",0);
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
        currentLevelNumber = levelBluePrints[levelIndex].levelNumber;
        PlayerPrefs.SetInt("CurrentLevelNumber", currentLevelNumber);

        currentImageNumber = levelBluePrints[levelIndex].imageNumber;
        PlayerPrefs.SetInt("currentImageNumber", currentImageNumber);

        SceneManager.LoadScene(1);
    }
}
