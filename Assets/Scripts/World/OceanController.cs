using UnityEngine;
using UnityEngine.EventSystems;

public class OceanController : MonoBehaviour
{
    public MainController main;

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        main.ClickOnOcean();
    }
}