using UnityEngine;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour
{
    // клас, який керує меню гри
    [SerializeField]
    private Text timerText, scoreText, timeLeftText;

    [SerializeField]
    private GameObject endGamePanel;

    [SerializeField]
    private Text endGameText;

    [SerializeField]
    private Text[] bestText;

    public void SetTimerText(string value)
    {
        timerText.text = $"Timer: {value}";
    }

    public void SetScoreText(string value)
    {
        scoreText.text = $"Targets left {value}";
    }
    public void SetTimeLeftText(string value)
    {
        timeLeftText.text = $"Time left: {value}";
    }

    public void ShowWinPanel(string[] bestScore, int winIndex)
    {
        endGameText.text = "Congratulations! You win :)";

        ShowEndGamePanel(bestScore, winIndex);
    }

    public void ShowLosePanel(string[] bestScore)
    {
        endGameText.text = "Sorry! You lose :(";

        ShowEndGamePanel(bestScore);
    }

    private void ShowEndGamePanel(string[] bestScore, int winIndex = 100)
    {
        // виніс однаковий код в один метод
        endGamePanel.SetActive(true);

        for (int i = 0; i < bestScore.Length; i++)
        {
            if (i < bestText.Length)
            {
                bestText[i].text = bestScore[i];

                // змінюю колір для поточного рекорду, для програшу індекс ніколи не стане 100
                if (i == winIndex)
                    bestText[i].color = Color.red;
                else
                    bestText[i].color = Color.black;
            }
        }
    }

    public void PlayButtonSound()
    {
        SoundManager.Instance?.PlayButtonSound();
    }
}
