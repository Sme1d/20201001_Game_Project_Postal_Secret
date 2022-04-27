using UnityEngine;

public class UIBase : MonoBehaviour
{
    public virtual void ActivateUI()
    {
        gameObject.SetActive(true);
    }

    public virtual void DeactivateUI()
    {
        gameObject.SetActive(false);
    }
}