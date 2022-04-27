using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SeasonController : MonoBehaviour
{
    public List<VolumeProfile> seasonProfiles;
    public List<Animator>      seasonAnimators;

    public void SetSeason(int seasonIndex)
    {
        GetComponent<Volume>().profile = seasonProfiles[seasonIndex];

        foreach (var seasonAnimator in seasonAnimators) seasonAnimator.SetInteger("Season", seasonIndex);
    }
}