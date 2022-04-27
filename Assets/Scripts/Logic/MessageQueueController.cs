using System.Collections.Generic;
using System.Linq;

public class MessageQueueController
{
    public List<int>     AlreadyLoadedLetterKeys = new List<int>();
    public List<Message> NextSeasonMsg           = new List<Message>();
    public Message       OpenedMsg;
    public List<Message> PickedUpMsg = new List<Message>();
    public Message       ProceededMsg;
    public List<Message> WaitingMsg = new List<Message>();

    public void LoadMsgQueue(Save save)
    {
        NextSeasonMsg           = save.NextSeasonMsg;
        PickedUpMsg             = save.PickedUpMsg;
        WaitingMsg              = save.WaitingMsg;
        OpenedMsg               = null;
        ProceededMsg            = save.ProceededMsg;
        AlreadyLoadedLetterKeys = save.AlreadyLoadedLetterKeys;
    }

    public void AddWaitingMsg(Message newMsg)
    {
        WaitingMsg.Add(newMsg);

        if (newMsg is Letter) AlreadyLoadedLetterKeys.Add(newMsg.Key);
    }

    public bool PickUpMsg(int islandIndex)
    {
        var temp = WaitingMsg.Where(msg => msg.Sender == islandIndex).ToList();
        WaitingMsg.RemoveAll(msg => msg.Sender == islandIndex);
        PickedUpMsg.AddRange(temp);
        return temp.Count != 0;
    }

    public Message OpenMsg(int msgIndex)
    {
        OpenedMsg = PickedUpMsg[msgIndex];
        return OpenedMsg;
    }

    public void ProceedMsg()
    {
        ProceededMsg = OpenedMsg;
        PickedUpMsg.Remove(OpenedMsg);
        OpenedMsg = null;
    }

    public Message DeliverMsg()
    {
        var msg = ProceededMsg;
        ProceededMsg = null;
        return msg;
    }

    public void SelectPs(int psIndex)
    {
        if (OpenedMsg is Letter letter) letter.SetMsgEffect(psIndex);
    }
}