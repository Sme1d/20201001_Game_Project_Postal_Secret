using System;
using System.Collections.Generic;

[Serializable]
public class Letter : Message
{
    public string PsText0       { get; set; }
    public string PsText1       { get; set; }
    public int    DefaultEffect { get; set; }
    public int    PsEffect0     { get; set; }
    public int    PsEffect1     { get; set; }
    public int    MsgEffect     { get; set; }

    public List<string> GetPs()
    {
        return new List<string>
        {
            PsText0,
            PsText1
        };
    }

    public void SetMsgEffect(int psIndex)
    {
        var psEffects = new List<int>
        {
            PsEffect0,
            PsEffect1
        };

        MsgEffect = MsgEffect == psEffects[psIndex] ? DefaultEffect : psEffects[psIndex];
    }

    public void ResetMsgEffect()
    {
        MsgEffect = DefaultEffect;
    }
}