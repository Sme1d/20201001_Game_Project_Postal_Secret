using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWithRelation : UIBase
{
    public List<Sprite> characterSprites;

    protected void UpdateCharacter(Transform character, int characterIndex)
    {
        character.GetComponentInChildren<Image>().sprite = characterSprites[characterIndex - 1];
        character.GetComponentInChildren<Text>().text    = Constants.Characters[characterIndex];
    }

    protected static void UpdateScale(GameObject relationGO, Relation relation)
    {
        relationGO.GetComponentInChildren<ScaleController>().UpdateScale(relation);
    }
}