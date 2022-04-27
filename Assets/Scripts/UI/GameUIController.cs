using System.Collections;
using UnityEngine;

public class GameUIController : UIBase
{
    // Variables
    public Transform seasonWheel;

    public void RotateSeasonPointer()
    {
        StartCoroutine(RotateOnZ(seasonWheel.GetChild(2).transform, GetRotationValue(1), Constants.SeasonPointerRotationTime));
    }

    public void RotateSeasonWheel()
    {
        SoundController.PlaySound(Constants.SeasonWheelSound);
        StartCoroutine(RotateOnZ(seasonWheel.GetChild(0).transform, Constants.SeasonWheelRotationValue, Constants.SeasonWheelRotationTime));
        StartCoroutine(RotateOnZ(seasonWheel.GetChild(2).transform, -Constants.SeasonPointerRotationRange,
                                 Constants.SeasonWheelRotationTime));
    }

    public void SetSeasonWheel(int seasonCounter, int msgCounter)
    {
        seasonWheel.GetChild(0).eulerAngles = new Vector3(0, 0, -90 + Constants.SeasonWheelRotationValue * seasonCounter);
        seasonWheel.GetChild(2).eulerAngles = new Vector3(0, 0, Constants.SeasonPointerRotationStart + GetRotationValue(msgCounter));
    }

    private float GetRotationValue(int msgCounter)
    {
        return msgCounter / (Constants.MsgPerSeason - 1) * Constants.SeasonPointerRotationRange;
    }

    private IEnumerator RotateOnZ(Transform temp, float rotationValue, float rotationTime)
    {
        var passedTime         = 0.0f;
        var startRotation      = temp.eulerAngles.z;
        var endRotation        = startRotation + rotationValue;
        var soundAlreadyPlayed = false;

        while (passedTime < rotationTime)
        {
            passedTime += Time.deltaTime;

            var fraction = temp.eulerAngles;
            fraction.z       = Mathf.Lerp(startRotation, endRotation, passedTime / rotationTime);
            temp.eulerAngles = fraction;

            if (passedTime > rotationTime * 0.85f && !soundAlreadyPlayed)
            {
                SoundController.PlaySound(Constants.SeasonPointerSound);
                soundAlreadyPlayed = true;
            }

            yield return null;
        }
    }
}