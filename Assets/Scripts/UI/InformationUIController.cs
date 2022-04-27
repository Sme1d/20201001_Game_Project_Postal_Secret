using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InformationUIController : UIBase
{
    public GameObject        reportGO;
    public List<ToggleGroup> questionToggleGroups;
    public GameObject        reportPage0;
    public GameObject        reportPage1;
    public Text              resultText0;
    public Text              resultText1;

    public Transform    infoUi;
    public GameObject   openedInfoGO;
    public Text         infoText;
    public List<string> givenAnswers;

    public void ActivateUI(List<int> unveiledInfo)
    {
        ActivateUI();

        for (var i = 0; i < infoUi.childCount; i++)
        {
            var isInteractable = unveiledInfo.Contains(i);
            var infoGO         = infoUi.GetChild(i);
            infoGO.GetComponent<Button>().interactable = isInteractable;
            infoGO.tag                                 = isInteractable ? Constants.CursorTags[1] : Constants.CursorTags[0];
        }
    }

    public void ActivateReport()
    {
        reportPage0.SetActive(true);
        reportPage1.SetActive(false);
        reportGO.SetActive(true);
    }

    public void DeactivateReport()
    {
        reportGO.SetActive(false);
    }

    private void UpdateGivenAnswers()
    {
        givenAnswers = new List<string> {"-", "-", "-", "-"};

        for (var i = 0; i < givenAnswers.Count; i++)
            if (questionToggleGroups[i].AnyTogglesOn())
                givenAnswers[i] = questionToggleGroups[i].ActiveToggles().ElementAt(0).GetComponentInChildren<Text>().text;
    }

    public int AnalyzeResult(int numOfSeasons)
    {
        UpdateGivenAnswers();
        var numOfCorrectAnswers = givenAnswers.Count(givenAnswer => Constants.CorrectAnswers.Contains(givenAnswer));
        var temp = numOfCorrectAnswers == Constants.NumOfQuestions
                       ? "You solved the case"
                       : $"You have {(Constants.MaxSeasons - numOfSeasons - 1).ToString()} more seasons.";

        resultText0.text =
            $"Who?   {givenAnswers.ElementAt(0)}\nWhat?  {givenAnswers.ElementAt(1)}\nWhy?   {givenAnswers.ElementAt(2)}\nWhere? {givenAnswers.ElementAt(3)}";

        resultText1.text =  $"{numOfCorrectAnswers.ToString()}/{Constants.NumOfQuestions} were correct.\n\n\n";
        resultText1.text += temp;

        // Reset questionToggleGroups
        foreach (var activeToggle in questionToggleGroups.Where(toggleGroup => toggleGroup.AnyTogglesOn())
            .SelectMany(toggleGroup => toggleGroup.ActiveToggles()))
            activeToggle.GetComponent<Toggle>().isOn = false;

        reportPage0.SetActive(false);
        reportPage1.SetActive(true);

        return numOfCorrectAnswers;
    }

    public void OpenInfo(Information information)
    {
        SoundController.PlaySound(Constants.InformationOpenSound);
        openedInfoGO.SetActive(true);
        infoText.text = information.MsgText;
    }

    public override void DeactivateUI()
    {
        openedInfoGO.SetActive(false);
        base.DeactivateUI();
    }
}