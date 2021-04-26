using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // клас для керування головним меню
    [SerializeField]
    private GameObject mainMenu, optionsPanel, quitPanel;
    [SerializeField]
    private Toggle musicToggle, soundToggle;


    // Start is called before the first frame update
    void Start()
    {
        // вимкнув додаткові меню, ввімкнув головне меню, для виключення помилок
        mainMenu.SetActive(true);
        optionsPanel.SetActive(false);
        quitPanel.SetActive(false);

        // перемикнув перемикачі звуку і музики згідно зі збереженними даними
        musicToggle.SetIsOnWithoutNotify(PlayerPrefsManager.GetMusic());
        soundToggle.SetIsOnWithoutNotify(PlayerPrefsManager.GetSound());
    }

    public void StarGame()
    {
        // загрузити наступну сцену і програти звук кнопки
        SoundManager.Instance?.PlayButtonSound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ShowQuitPanel(bool value)
    {
        SoundManager.Instance?.PlayButtonSound();
        ShowPanel(quitPanel, value);
    }

    public void ShowOptionsPanel(bool value)
    {
        SoundManager.Instance?.PlayButtonSound();
        ShowPanel(optionsPanel, value);
    }

    private void ShowPanel(GameObject panel, bool value)
    {
        //є два методи, які мають одаковий  код з різними параметрами, виніс одаковий код в один метод
        mainMenu.SetActive(!value);
        panel.SetActive(value);
    }

    public void SetMusic(bool value)
    {
        SoundManager.Instance?.PlayButtonSound();
        PlayerPrefsManager.SetMusic(value);
        SoundManager.Instance?.SetMusic(value);
    }

    public void SetSound(bool value)
    {
        PlayerPrefsManager.SetSound(value);
        SoundManager.Instance?.PlayButtonSound();
    }

    public void QuitGame()
    {
        SoundManager.Instance?.PlayButtonSound();
        Application.Quit();
    }
}
