using UnityEngine;

public class FadeUI : UIBase
{
    public void Fade()
    {
        ActivateUI();
        GetComponent<Animator>().SetTrigger(Constants.TriggerFade);
    }
}