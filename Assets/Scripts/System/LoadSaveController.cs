using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class LoadSaveController
{
    private readonly string _filePath = $"{Application.persistentDataPath}{Constants.SaveFilePath}";

    public bool GameSaveExists()
    {
        return File.Exists(_filePath);
    }

    public void DeleteSave()
    {
        if (GameSaveExists()) File.Delete(_filePath);
    }

    public Save LoadGame()
    {
        if (!GameSaveExists()) return null;

        var bf   = new BinaryFormatter();
        var file = File.Open(_filePath, FileMode.Open);
        var save = (Save) bf.Deserialize(file);
        file.Close();
        return save;
    }

    public void SaveGame(int                seasonCounter,    MessageQueueController msgQueue, InformationQueueController infoQueue,
                         RelationController relations,        int msgAppearedCounter, int msgDeliveredCounter, bool newPickedUpMsg,
                         bool               tutorialIsActive, int tutorialStep)
    {
        var save = new Save
        {
            SeasonCounter           = seasonCounter,
            ProceededMsg            = msgQueue.ProceededMsg,
            PickedUpMsg             = msgQueue.PickedUpMsg,
            NextSeasonMsg           = msgQueue.NextSeasonMsg,
            WaitingMsg              = msgQueue.WaitingMsg,
            AlreadyLoadedLetterKeys = msgQueue.AlreadyLoadedLetterKeys,
            WaitingInfo             = infoQueue.WaitingInfo,
            UnveiledInfo            = infoQueue.UnveiledInfo,
            AlreadyLoadedInfoKeys   = infoQueue.AlreadyLoadedInfoKeys,
            Relations               = relations.Relations,
            MsgAppearedCounter      = msgAppearedCounter,
            MsgDeliveredCounter     = msgDeliveredCounter,
            NewPickedUpMsg          = newPickedUpMsg,
            TutorialIsActive        = tutorialIsActive,
            TutorialStep            = tutorialStep
        };

        var binaryFormatter = new BinaryFormatter();
        var file            = File.Create(_filePath);
        binaryFormatter.Serialize(file, save);
        file.Close();
    }
}