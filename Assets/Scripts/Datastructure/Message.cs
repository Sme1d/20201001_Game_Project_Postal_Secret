using System;
using SQLite4Unity3d;

[Serializable]
public class Message
{
    [AutoIncrement] [PrimaryKey] public int    Key      { get; set; }
    public                              int    Sender   { get; set; }
    public                              int    Receiver { get; set; }
    public                              string MsgText  { get; set; }
}