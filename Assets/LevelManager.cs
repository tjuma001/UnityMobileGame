using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public static LevelManager instance;
    public static LevelManager Instance { get { return instance; } }

    public GameObject pauseMenu;

    private float startTime;
    public float silverTime;
    public float goldTime;

    private void Start()
    {
        instance = this;
        pauseMenu.SetActive(false);
        startTime = Time.time;
    }

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Victory()
    {
        float duration = Time.time - startTime;

        if(duration < goldTime)
        {
            GameManager.Instance.currency += 50;
        }
        else if(duration < silverTime)
        {
            GameManager.Instance.currency += 25;
        }
        else
        {
            GameManager.Instance.currency += 10;
        }

        GameManager.Instance.Save();

        // "30&60&45"
        string saveString = "";
        saveString += duration.ToString();
        saveString += '&';
        saveString += silverTime.ToString();
        saveString += '&';
        saveString += goldTime.ToString();
        PlayerPrefs.SetString(SceneManager.GetActiveScene().name, saveString);

        SceneManager.LoadScene("MainMenu");
    }
}
