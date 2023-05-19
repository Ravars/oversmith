using System;
using Oversmith.Scripts.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Audio;

namespace Oversmith.Scripts.Managers
{
    enum AudioGroups
    {
        MasterVolume,
        MusicVolume,
        SFXVolume
    }
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private FloatEventChannelSO masterVolumeChannel;
        [SerializeField] private FloatEventChannelSO musicVolumeChannel;
        [SerializeField] private FloatEventChannelSO sfxVolumeChannel;
        public static readonly int MaxVolume = 10;
        private void OnEnable()
        {
            masterVolumeChannel.OnEventRaised += SetMasterVolume;
            musicVolumeChannel.OnEventRaised += SetMusicVolume;
            sfxVolumeChannel.OnEventRaised += SetSfxVolume;
        }

        private void OnDisable()
        {
            masterVolumeChannel.OnEventRaised -= SetMasterVolume;
            musicVolumeChannel.OnEventRaised -= SetMusicVolume;
            sfxVolumeChannel.OnEventRaised -= SetSfxVolume;
        }


        public void SetMasterVolume(float newVolume)
        {
            Debug.Log("SetMasterVolume: " + newVolume);
            SetGroupVolume(AudioGroups.MasterVolume.ToString(),newVolume);
        }
        public void SetMusicVolume(float newVolume)
        {
            SetGroupVolume(AudioGroups.MusicVolume.ToString(),newVolume);
        }
        public void SetSfxVolume(float newVolume)
        {
            SetGroupVolume(AudioGroups.SFXVolume.ToString(),newVolume);
        }

        private void SetGroupVolume(string groupName, float volume)
        {
            volume /= MaxVolume;
            // Debug.Log("Volume real: " + volume  + " " + LogarithmicDbTransform(volume) + " " + groupName);
            
            bool volumeSet = audioMixer.SetFloat(groupName, LogarithmicDbTransform(volume));
            if (!volumeSet)
            {
                Debug.LogError("AudioMixer not found.");
            }
        }


        public static float LogarithmicDbTransform(float volume)
        {
            volume = Mathf.Clamp01(volume);
            volume = (Mathf.Log(89 * volume + 1) / Mathf.Log(90)) * 80;
            float retorno = volume - 80;
            return retorno;
        }
    }
}