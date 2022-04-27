using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IslandController : BaseControllerForObjectsWithOutline
{
    [HideInInspector] public MainController main;
    public                   GameObject     notifier;
    public                   int            islandIndex;
    public                   Animator       reactionAnimator;
    public                   List<Sprite>   infoSprites;

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        main.ClickOnIsland(islandIndex, transform.position);
    }

    public void SetNotifier(bool notifierState, bool isInfo = false, bool withSound = false)
    {
        if (islandIndex == 0) return;

        notifier.SetActive(notifierState);

        if (!notifierState && !isInfo && withSound)
            SoundController.PlaySound(Constants.BoatLoadingSound);

        if (!notifierState) return;

        notifier.GetComponent<Animator>().SetBool(Constants.BoolMsgIsInfo, isInfo);
        notifier.GetComponent<Animator>().SetTrigger(Constants.TriggerSpawnNotifier);
    }

    public void TriggerReaction(int reactionIndex, int infoIndex = -1)
    {
        if (infoIndex != -1) reactionAnimator.transform.GetChild(2).GetComponent<SpriteRenderer>().sprite = infoSprites[infoIndex];

        reactionAnimator.SetTrigger(Constants.TriggerReactions[reactionIndex]);
        SoundController.PlaySound(Constants.ReactionSounds[reactionIndex]);
    }

    public void SetFlashingActive(bool flashingIsActive)
    {
        if (flashingIsActive)
        {
            GetComponent<Animator>().SetTrigger(Constants.TriggerStartFlashingOutline);
            GetComponent<Animator>().ResetTrigger(Constants.TriggerStopFlashingOutline);
        }
        else
        {
            GetComponent<Animator>().ResetTrigger(Constants.TriggerStartFlashingOutline);
            GetComponent<Animator>().SetTrigger(Constants.TriggerStopFlashingOutline);
        }
    }
}