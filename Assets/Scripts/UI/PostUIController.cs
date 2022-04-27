using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PostUIController : UIWithRelation
{
    // Variables
    public Text       msgText;
    public GameObject openedMsgUI;
    public GameObject msgRelationUI;
    public Transform  psUI;
    public Transform  msgUI;

    public Sprite msgLetterSprite;
    public Sprite msgInfoSprite;

    public void OpenMsg(Message msg, Relation relation)
    {
        openedMsgUI.SetActive(true);
        msgText.text = msg.MsgText;

        if (msg is Letter letter)
        {
            psUI.gameObject.SetActive(true);
            var ps = letter.GetPs();

            for (var i = 0; i < ps.Count; i++)
            {
                var psGO = psUI.GetChild(i).gameObject;
                psGO.SetActive(true);

                var psText = psGO.transform.GetChild(0).GetComponent<Text>();
                psText.color = Color.gray;
                psText.text  = ps[i];
            }
        }

        msgRelationUI.SetActive(true);
        UpdateCharacter(msgRelationUI.transform.GetChild(0).GetChild(0), msg.Sender);
        UpdateCharacter(msgRelationUI.transform.GetChild(0).GetChild(1), msg.Receiver);
        UpdateScale(msgRelationUI, relation);
    }

    public void ProceedMsg()
    {
        psUI.gameObject.SetActive(false);
        openedMsgUI.GetComponent<Animator>().SetTrigger(Constants.TriggerCloseMsg);
        msgRelationUI.SetActive(false);
    }

    public void SetProceedButtonInteractable(bool isInteractable)
    {
        openedMsgUI.GetComponentInChildren<Button>().interactable = isInteractable;
    }

    public void DisplayMsg(List<Message> pickedUpMsg)
    {
        var numbersOfDisplayedMsg = Mathf.Min(Constants.NumberOfDisplayedMsg, pickedUpMsg.Count);

        for (var i = 0; i < Constants.NumberOfDisplayedMsg; i++)
        {
            var msgGO = msgUI.GetChild(i).gameObject;
            msgGO.SetActive(i < numbersOfDisplayedMsg);

            if (i >= numbersOfDisplayedMsg) continue;

            var msg = pickedUpMsg[i];
            msgGO.transform.GetChild(0).GetChild(3).GetComponent<Image>().color  = Constants.CharacterColors[msg.Sender - 1];
            msgGO.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = msg is Information ? msgInfoSprite : msgLetterSprite;
        }
    }

    public void SetFreezeOnMsg(bool state)
    {
        msgUI.gameObject.GetComponentsInChildren<Button>().ToList().ForEach(x => x.interactable = !state);
    }
}