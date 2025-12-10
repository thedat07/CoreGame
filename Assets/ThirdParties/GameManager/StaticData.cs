using UnityEngine;
using UnityUtilities;

public static class StaticData
{
    public static float DelayTimeDefault = 0.2f;

    private const string Key = "player_id";

    public static string GetPlayerId()
    {
        if (!PlayerPrefs.HasKey(Key))
        {
            string newId = System.Guid.NewGuid().ToString();
            PlayerPrefs.SetString(Key, newId);
            PlayerPrefs.Save();
        }
        return PlayerPrefs.GetString(Key);
    }

    public static string GetPlayerIdSubstring()
    {
        string fullId = GetPlayerId();

        return fullId.Length > 10
            ? fullId.Substring(fullId.Length - 10)
            : fullId;
    }

    public static int IAPCount
    {
        get
        {
            return PlayerPrefs.GetInt("IAPCount", 0);
        }
        set
        {
            PlayerPrefs.SetInt("IAPCount", value);
            PlayerPrefs.Save();
        }
    }

    public static int CurrentLevelIndex
    {
        get
        {
            return PlayerPrefs.GetInt("CurrentLevelIndex", 0);
        }
        set
        {
            PlayerPrefs.SetInt("CurrentLevelIndex", value);
            PlayerPrefs.Save();
        }
    }

    public static int SeassonGame
    {
        get
        {
            return PlayerPrefs.GetInt("SeassonGame", 0);
        }
        set
        {
            PlayerPrefs.SetInt("SeassonGame", value);
            PlayerPrefs.Save();
        }
    }

    public static int InterTimestepRw
    {
        get
        {
            return 180;
        }
    }

    public static int LevelStartShowingInter
    {
        get
        {
            return 10;
        }
    }

    public static int MinGameTimeForInter
    {
        get
        {
            return 60;
        }
    }

    public static int InterTimestep
    {
        get
        {
            return 180;
        }
    }
}
