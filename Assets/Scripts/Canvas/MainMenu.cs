using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject backGroundAudio;

    [SerializeField] private AudioMixer audioMixer;

    private void Awake()
    {
        DontDestroyOnLoad(backGroundAudio);
    }

    public void StartGame()
    {
        mainMenu.SetActive(false);
        SceneManager.LoadScene(1);
    }

    public void GoToOptions()
    {
        mainMenu.SetActive(false);
        options.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
