using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Singleton
    // створюю сінглтон, якщо він вже є, знищую об'єкт
    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [SerializeField]
    private AudioClip music, buttonClip;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // додаю компонент, який буде програвати музику
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = music;
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        if (PlayerPrefsManager.GetMusic())
        {
            audioSource.Play();
        }
    }

    public void SetMusic(bool value)
    {
        // перемикаю музику
        if (value)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }

    public void PlayButtonSound()
    {
        // програю звук кнопки
        if (!PlayerPrefsManager.GetSound()) return;

        audioSource.PlayOneShot(buttonClip);
    }

    public void PlayClip(AudioClip audioClip, float volume = 1f)
    {
        // програю звук, який передається з заданою гучністью
        if (!PlayerPrefsManager.GetSound()) return;

        audioSource.PlayOneShot(audioClip, volume);
    }
}
