using System;
using System.Collections.Generic;

[Serializable]
public class Save
{
    public int SeasonCounter { get; set; }

    public List<Message>                    NextSeasonMsg           { get; set; }
    public List<Message>                    PickedUpMsg             { get; set; }
    public Message                          ProceededMsg            { get; set; }
    public List<Message>                    WaitingMsg              { get; set; }
    public List<Information>                WaitingInfo             { get; set; }
    public List<int>                        UnveiledInfo            { get; set; }
    public List<int>                        AlreadyLoadedInfoKeys   { get; set; }
    public List<int>                        AlreadyLoadedLetterKeys { get; set; }
    public Dictionary<(int, int), Relation> Relations               { get; set; }
    public int                              MsgAppearedCounter      { get; set; }
    public int                              MsgDeliveredCounter     { get; set; }
    public bool                             NewPickedUpMsg          { get; set; }
    public bool                             TutorialIsActive        { get; set; }
    public int                              TutorialStep            { get; set; }
}