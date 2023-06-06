using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void SetScore(int score)
    {
        text.text = "Game over. Your score:" + score.ToString() + " turns";
    }

    public void Restart()
    {
        this.gameObject.active = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        this.gameObject.active = false;
        SceneManager.LoadScene(0);
    }

}
