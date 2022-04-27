using System.Collections;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    private static EventInstance soundscapeInstance;
    private static EventInstance scoreInstance;
    private static EventInstance boatSoundInstance;

    public static void SetSoundscape(int islandIndex)
    {
        soundscapeInstance.setParameterByName(Constants.ParameterSoundscapeTransition, islandIndex);
    }

    public static IEnumerator FadeRain()
    {
        var passedTime = 0f;
        soundscapeInstance.getParameterByName(Constants.ParameterRain, out var startValue);
        var fadeDir                    = Mathf.Sign(startValue);
        if (startValue > 0.5f) fadeDir = -1;

        while (passedTime <= Constants.RainFadeTime)
        {
            var fraction = startValue + Mathf.Min(1, passedTime / Constants.RainFadeTime) * fadeDir;
            soundscapeInstance.setParameterByName(Constants.ParameterRain, fraction);
            passedTime += Time.deltaTime;
            yield return null;
        }

        soundscapeInstance.setParameterByName(Constants.ParameterRain, (startValue + 1) % 2);
        soundscapeInstance.getParameterByName(Constants.ParameterRain, out var final);
    }

    // Menu Sounds

    public void PlayMenuButtonSound(bool withWhoosh)
    {
        PlaySound(Constants.MenuButtonSound);
        if (withWhoosh) PlaySound(Constants.MenuWhooshSound);
    }

    public void PlayCheckboxSound()
    {
        PlaySound(Constants.ReportCheckSound);
    }

    // Start & Stop Instances

    public static EventInstance PlaySound(string eventPath)
    {
        var instance = RuntimeManager.CreateInstance(eventPath);
        instance.start();
        instance.release();
        return instance;
    }

    public static void StartSoundscape()
    {
        soundscapeInstance = PlaySound(Constants.Soundscape);
        SetSoundscape(Constants.OverviewIndex);
    }

    public static void StartScore(int scoreIndex)
    {
        StopScore();
        scoreInstance = PlaySound(Constants.Score);
        scoreInstance.setParameterByName(Constants.ParameterScoreTransition, scoreIndex);
    }

    public static void StopScore()
    {
        StopInstance(scoreInstance);
    }

    public static void StartBoatSound()
    {
        StopBoatSound();
        boatSoundInstance = PlaySound(Constants.BoatMovingSound);
    }

    private static void StopInstance(EventInstance instance)
    {
        if (!instance.Equals(null)) instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public static void StopBoatSound()
    {
        StopInstance(boatSoundInstance);
    }

    // Global Parameter

    public void SetGlobalSfxVolume(Slider slider)
    {
        RuntimeManager.StudioSystem.setParameterByName(Constants.ParameterSfxVolume, slider.value);
    }

    public void SetGlobalMusicVolume(Slider slider)
    {
        RuntimeManager.StudioSystem.setParameterByName(Constants.ParameterScoreVolume, slider.value);
    }

    public void SetGlobalSoundscapeVolume(Slider slider)
    {
        RuntimeManager.StudioSystem.setParameterByName(Constants.ParameterSoundscapeVolume, slider.value);
    }
}