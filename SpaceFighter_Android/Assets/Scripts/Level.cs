using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    [SerializeField] private float delayInSeconds = 2f;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    public void LoadGameOver()
    {
        StartCoroutine(DelayGameOverScreen());
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("Level_01");
        if(FindObjectOfType<Score>() != null)
        {
            FindObjectOfType<Score>().ResetGame();
        }
    }

    public void LoadStartMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator DelayGameOverScreen()
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene("GameOver");
    }
}
