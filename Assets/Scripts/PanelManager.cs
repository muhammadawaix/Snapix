using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Button[] levels;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            // Sirf first button active, baqi disable
            if (i == 0)
                levels[i].interactable = true;
            else
                levels[i].interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void ClickButton()
    {
        SceneManager.LoadScene(1);
    }
}
