using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PsOnClick : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IPointerClickHandler
{
    public  Text  ownPs;
    public  Text  otherPs;
    private Color _previousColor;

    public void OnPointerClick(PointerEventData eventData)
    {
        SoundController.PlaySound(Constants.LetterPSSound);

        if (_previousColor == Color.grey)
        {
            ownPs.color    = Color.black;
            otherPs.color  = Color.grey;
            _previousColor = Color.black;
        }
        else
        {
            ownPs.color    = Color.grey;
            _previousColor = Color.grey;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _previousColor = ownPs.color;
        ownPs.color    = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ownPs.color = _previousColor;
    }
}