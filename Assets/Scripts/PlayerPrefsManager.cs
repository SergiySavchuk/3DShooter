using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerPrefsManager
{
    // окремий клас, який би займався зберіганням данних в PlayerPrefs

    private const string  musicPref = "MUSIC_PREF", soundPref = "SOUND_PREF", bestScorePref = "BEST_PREF_";

    public static void SetMusic(bool value)
    {
        PlayerPrefs.SetInt(musicPref, value ? 1 : 0);
    }

    public static bool GetMusic()
    {
        return PlayerPrefs.GetInt(musicPref, 1) == 1;
    }



    public static void SetSound(bool value)
    {
        PlayerPrefs.SetInt(soundPref, value ? 1 : 0);
    }

    public static bool GetSound()
    {
        return PlayerPrefs.GetInt(soundPref, 1) == 1;
    }



    public static void SetBestScore(float[] value)
    {
        for (int i = 0; i < value.Length; i++)
        {
            if (i < 10)
            {
                PlayerPrefs.SetFloat(bestScorePref + i, value[i]);
            }
        }
    }

    public static float[] GetBestScore(out float defaultValue)
    {
        defaultValue = 100f;
        float[] returnValue = new float[10];

        for (int i = 0; i < returnValue.Length; i++)
        {
            returnValue[i] = PlayerPrefs.GetFloat(bestScorePref + i, defaultValue);
        }

        return returnValue;
    }
}
