using System;
using Oversmith.Scripts.Utils;
using UnityEngine;

namespace Oversmith.Scripts.Managers
{
    public static class PlayerPrefsManager
    {
        public static bool HasKey(PlayerPrefsKeys key)
        {
            return PlayerPrefs.HasKey(key.ToString());
        }

        public static void SetString(PlayerPrefsKeys key, string value)
        {
            PlayerPrefs.SetString(key.ToString(), value);
        }

        public static void SetInt(PlayerPrefsKeys key, int value)
        {
            PlayerPrefs.SetInt(key.ToString(), value);
        }

        public static void SetFloat(PlayerPrefsKeys key, float value)
        {
            PlayerPrefs.SetFloat(key.ToString(), value);
        }

        public static string GetString(PlayerPrefsKeys key, string value)
        {
            return HasKey(key) ? PlayerPrefs.GetString(key.ToString(), value) : null;
        }

        public static int GetInt(PlayerPrefsKeys key, int value)
        {
            return HasKey(key) ? PlayerPrefs.GetInt(key.ToString(), value) : -1;
        }

        public static float GetFloat(PlayerPrefsKeys key, float value)
        {
            return HasKey(key) ? PlayerPrefs.GetFloat(key.ToString(), value) : -1;
        }
    }
}