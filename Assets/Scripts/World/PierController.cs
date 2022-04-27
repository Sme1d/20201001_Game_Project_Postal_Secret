using UnityEngine.EventSystems;

public class PierController : BaseControllerForObjectsWithOutline
{
    public MainController main;
    public int            islandIndex;

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        main.ClickOnPier(islandIndex);
    }
}