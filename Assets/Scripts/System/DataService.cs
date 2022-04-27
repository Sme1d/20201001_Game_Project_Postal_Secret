using System.Collections.Generic;
using System.IO;
using System.Linq;
using SQLite4Unity3d;
using UnityEngine;
using Random = System.Random;

#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif

public class DataService
{
    private readonly SQLiteConnection _connection;

    public DataService()
    {
        //ToDo: Can be removed once db is finished
        File.Delete($"{Application.persistentDataPath}/{Constants.DBName}");

#if UNITY_EDITOR
        var dbPath = $@"Assets/StreamingAssets/{Constants.DBName}";
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, Constants.DBName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID 
            var loadDb =
 new WWW("jar:file://" + Application.dataPath + "!/assets/" + Constants.DBName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb = Application.dataPath + "/Raw/" + Constants.DBName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb =
 Application.dataPath + "/StreamingAssets/" + Constants.DBName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb = Application.dataPath + "/StreamingAssets/" + Constants.DBName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
		
#elif UNITY_STANDALONE_OSX
		var loadDb =
 Application.dataPath + "/Resources/Data/StreamingAssets/" + Constants.DBName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
	var loadDb = Application.dataPath + "/StreamingAssets/" + Constants.DBName;  // this is the path to your StreamingAssets in iOS
	// then save to Application.persistentDataPath
	File.Copy(loadDb, filepath);

#endif

            Debug.Log("Database written");
        }

        var dbPath = filepath;
#endif
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
    }

    public Letter GetLetter(int p1, int p2, List<int> AlreadyLoadedKeys, bool isDirectional = false)
    {
        var random  = new Random();
        var letters = _connection.Table<Letter>();

        letters = isDirectional
                      ? letters.Where(x => x.Sender == p1 && x.Receiver == p2)
                      : letters.Where(x => x.Sender == p1 && x.Receiver == p2 || x.Receiver == p1 && x.Sender == p2);

        if (!letters.All(x => AlreadyLoadedKeys.Contains(x.Key))) letters = letters.Where(x => !AlreadyLoadedKeys.Contains(x.Key));

        var newLetter = letters.ElementAt(random.Next(0, letters.Count()));
        return newLetter;
    }

    public Information GetInformation(int key)
    {
        var temp = _connection.Table<Information>().Where(information => information.Key == key);
        return temp.ElementAt(0);
    }

    public Information GetUnknownInformation(List<(int, int)> pairsWithWaitingInfo, List<int> alreadyLoadedKeys)
    {
        var temp = _connection.Table<Information>().Where(x => !alreadyLoadedKeys.Contains(x.Key));
        var filteredTemp = temp.ToList().Where(x => !pairsWithWaitingInfo.Contains((x.Sender, x.Receiver)) &&
                                                    !pairsWithWaitingInfo.Contains((x.Receiver, x.Sender)));

        if (!filteredTemp.Any()) return null;

        var rdm            = new Random();
        var newInformation = filteredTemp.ElementAt(rdm.Next(0, filteredTemp.Count()));
        return newInformation;
    }
}