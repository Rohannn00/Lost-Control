using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool ispaused = false;
    public GameObject pausemenuUI;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        AssignPauseMenuUI();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ispaused)
            {
                Resume();
            }
            else
            {
                pause();
            }
        }
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignPauseMenuUI();
    }

    void AssignPauseMenuUI()
    {
        if (pausemenuUI == null)
        {
            pausemenuUI = GameObject.Find("PauseMenuUI");
        }
    }

    public void Resume()
    {
        if (pausemenuUI != null)
        {
            pausemenuUI.SetActive(false);
        }
        Time.timeScale = 1;
        ispaused = false;
    }

    public void pause()
    {
        if (pausemenuUI != null)
        {
            pausemenuUI.SetActive(true);
        }
        Time.timeScale = 0f;
        ispaused = true;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void mainmenu()
    {
      //  SceneManager.LoadScene("Level1");
    }

    public void Quit()
    {
        Debug.Log("Game is quitting");
        Application.Quit();
    }
}
