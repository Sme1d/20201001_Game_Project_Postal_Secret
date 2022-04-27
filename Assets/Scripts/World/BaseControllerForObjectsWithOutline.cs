using UnityEngine;
using UnityEngine.EventSystems;

public class BaseControllerForObjectsWithOutline : MonoBehaviour
{
    public Animator animator;
    public bool     outlineIsActive;

    private void Start()
    {
        outlineIsActive = true;
    }

    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject() || !outlineIsActive) return;

        animator.ResetTrigger(Constants.TriggerDeactivateOutline);
        animator.SetTrigger(Constants.TriggerActivateOutline);
    }

    private void OnMouseExit()
    {
        animator.ResetTrigger(Constants.TriggerActivateOutline);
        animator.SetTrigger(Constants.TriggerDeactivateOutline);
    }
}