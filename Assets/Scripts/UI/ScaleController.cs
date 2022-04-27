using UnityEngine;
using UnityEngine.UI;

public class ScaleController : MonoBehaviour
{
    public Slider sliderBar;
    public Slider infoMarker;

    public void UpdateScale(Relation relation)
    {
        infoMarker.gameObject.SetActive(relation.IsMarkerSet);
        infoMarker.value = relation.MarkerValue;
        var value = relation.RelationValue;

        if (value >= -100 && value <= 100) sliderBar.value = value;
    }
}