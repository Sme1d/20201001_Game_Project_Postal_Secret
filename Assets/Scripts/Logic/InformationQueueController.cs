using System.Collections.Generic;
using System.Linq;

public class InformationQueueController
{
    public List<int>         AlreadyLoadedInfoKeys = new List<int>();
    public List<int>         UnveiledInfo          = new List<int>();
    public List<Information> WaitingInfo           = new List<Information>();

    public void LoadInfoQueue(Save save)
    {
        UnveiledInfo          = save.UnveiledInfo;
        WaitingInfo           = save.WaitingInfo;
        AlreadyLoadedInfoKeys = save.AlreadyLoadedInfoKeys;
    }

    public Information GetInfo(int p1, int p2)
    {
        var info =
            WaitingInfo.Find(x => x.Sender == p1 && x.Receiver == p2 || x.Sender == p2 && x.Receiver == p1);

        WaitingInfo.Remove(info);
        return info;
    }

    public List<(int, int)> GetPairsWithWaitingInfo()
    {
        return WaitingInfo.Select(info => (info.Sender, info.Receiver)).ToList();
    }

    public bool UnveilInfo(int key)
    {
        UnveiledInfo.Add(key);
        return UnveiledInfo.Count == Constants.MaxInfo;
    }

    public void AddWaitingInfo(Information info)
    {
        WaitingInfo.Add(info);
        AlreadyLoadedInfoKeys.Add(info.Key);
    }
}