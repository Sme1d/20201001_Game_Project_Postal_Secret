using System.Collections.Generic;
using UnityEngine;

public class OutroUIController : UIBase
{
    public List<Transform> info0;
    public List<Transform> info1;
    public List<Transform> info2;
    public List<Transform> info3;
    public List<Transform> info4;
    public List<Transform> info5;
    public List<Transform> info6;
    public List<Transform> info7;
    public List<Transform> info8;
    public List<Transform> info9;
    public GameObject      EndTextGo;
    public GameObject      FailedEndTextGo;

    public void PrepareOutro(List<int> unveiledInfoKeys, bool allCorrect)
    {
        ActivateUI();

        var infos = new List<List<Transform>> {info0, info1, info2, info3, info4, info5, info6, info7, info8, info9};

        foreach (var transforms in infos)
        {
            var unveiled = unveiledInfoKeys.Contains(infos.IndexOf(transforms)) || allCorrect;

            foreach (var info in transforms)
            {
                info.gameObject.SetActive(true);
                info.GetChild(0).gameObject.SetActive(unveiled);
                info.GetChild(1).gameObject.SetActive(!unveiled);
            }
        }

        for (var i = 0; i < transform.childCount - 3; i++)
        {
            var storyPiece = transform.GetChild(i);
            var unveiled   = true;

            for (var j = 0; j < storyPiece.childCount - 1; j++) unveiled &= storyPiece.GetChild(j).GetChild(0).gameObject.activeSelf;
            storyPiece.GetComponent<MeshRenderer>().enabled = unveiled;
            storyPiece.GetChild(storyPiece.childCount - 1).gameObject.SetActive(!unveiled);
        }

        EndTextGo.SetActive(allCorrect);
        FailedEndTextGo.SetActive(!allCorrect);
    }
}