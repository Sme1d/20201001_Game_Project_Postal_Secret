using UnityEngine;

public class WeatherSounds : MonoBehaviour
{
    public void StartStopRainSound()
    {
        StartCoroutine(SoundController.FadeRain());
    }
}