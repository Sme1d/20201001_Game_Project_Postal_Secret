using System;
using UnityEngine;

[Serializable]
public class Relation
{
    public Relation((int, int) partners, int value = 0)
    {
        RelationValue = value;
        Partners      = partners;
    }

    public int        RelationValue { get; set; }
    public int        MarkerValue   { get; set; }
    public bool       IsMarkerSet   { get; set; }
    public (int, int) Partners      { get; set; }

    public bool UpdateRelation(int relationDelta)
    {
        var temp = RelationValue;
        RelationValue = Mathf.Min(100, Mathf.Max(-100, RelationValue + relationDelta));

        if (!IsMarkerSet || (temp <= MarkerValue || RelationValue > MarkerValue) && (temp >= MarkerValue || RelationValue < MarkerValue)
        ) return false;

        IsMarkerSet = false;
        return true;
    }

    public void ActivateMarker(int markerValue)
    {
        MarkerValue = markerValue == RelationValue ? markerValue : markerValue + 10;
        IsMarkerSet = true;
    }
}