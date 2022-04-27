using UnityEngine;

public class TutorialUIController : UIBase
{
    public int _currentStep = -1;

    public void NextTutorialStep(bool isActive = true)
    {
        gameObject.SetActive(isActive);
        CloseCurrentBubble(isActive);
        _currentStep++;
        transform.GetChild(_currentStep).gameObject.SetActive(true);
    }

    public void CloseCurrentBubble(bool isActive)
    {
        // First bubble (welcome letter is deactivated by button press)
        if (_currentStep < Constants.TutorialStepFirstMsg || _currentStep == Constants.TutorialStepReport) return;

        var bubble = transform.GetChild(_currentStep);

        if (isActive)
            bubble.GetComponent<Animator>().SetTrigger(Constants.TriggerCloseTutorialBubble);
        else
            bubble.gameObject.SetActive(false);
    }

    private void CloseAllBubbles()
    {
        for (var i = 0; i < transform.childCount; i++) transform.GetChild(i).gameObject.SetActive(false);
    }

    public void ResetTutorial()
    {
        _currentStep = -1;
        CloseAllBubbles();
        gameObject.SetActive(false);
    }
}