using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class RandomMsgBackground : MonoBehaviour
{
    public List<Color>  msgBackgroundColors;
    public List<Sprite> msgStains;
    public Transform    msgStain;
    public List<Sprite> msgBackgrounds;

    private void OnEnable()
    {
        var rnd = new Random();
        GetComponent<Image>().sprite                            = msgBackgrounds[rnd.Next(0, msgBackgrounds.Count)];
        GetComponent<Image>().color                             = msgBackgroundColors[rnd.Next(0, msgBackgroundColors.Count)];
        msgStain.GetComponent<Image>().sprite                   = msgStains[rnd.Next(0, msgStains.Count)];
        msgStain.GetComponent<RectTransform>().anchoredPosition = new Vector3(rnd.Next(-740, 300), rnd.Next(-1060, 600));
    }
}