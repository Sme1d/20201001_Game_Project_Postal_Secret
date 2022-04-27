using System.Collections.Generic;
using UnityEngine;

public class IslandUIController : UIWithRelation
{
    public Transform  islandCharacter;
    public GameObject relationUI;

    public void ActivateUI(int islandIndex, List<Relation> relations)
    {
        gameObject.SetActive(true);
        PopulateIslandUI(islandIndex, relations);
    }

    public void LeaveIsland()
    {
        if (!gameObject.activeSelf) return;

        gameObject.GetComponent<Animator>().SetTrigger(Constants.TriggerCloseIslandUi);
    }

    private void PopulateIslandUI(int islandIndex, IReadOnlyList<Relation> relations)
    {
        UpdateCharacter(islandCharacter, islandIndex);

        var characterIndex = 1;

        for (var i = 0; i < relations.Count; i++)
        {
            var relationGO = relationUI.transform.GetChild(i).gameObject;
            UpdateScale(relationGO, relations[i]);

            if (characterIndex == islandIndex) characterIndex++;
            UpdateCharacter(relationGO.transform.GetChild(0), characterIndex);
            characterIndex++;
        }
    }
}