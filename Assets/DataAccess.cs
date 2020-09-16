using Mono.Data.Sqlite;
using System.IO;
using UnityEngine;

public class DataAccess
{
    private static readonly string sqliteFile = $"{Application.dataPath}/Gamedata.sqlite";
    static SqliteConnection dbConnection;

    /// <summary>
    /// Connects to sqlite database or creates one if it does not exist
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        if (!File.Exists(sqliteFile))
        {
            Debug.Log($"Creating sqlite database in folder: {Application.dataPath}");
            File.Create(sqliteFile).Close();
        }

        dbConnection = new SqliteConnection($"URI=file:{sqliteFile}");
        dbConnection.Open();

        var createTable = "CREATE TABLE IF NOT EXISTS Games(id INTEGER PRIMARY KEY,gametype INTEGER NOT NULL, complete INTEGER NOT NULL, time INTEGER NOT NULL,numhits INTEGER NOT NULL,numpowerups INTEGER NOT NULL, win INTEGER NOT NULL)";

        var command = dbConnection.CreateCommand();
        command.CommandText = createTable;
        command.ExecuteNonQuery();
    }

    public static void SaveGameData(GameData gameData)
    {
        var command = dbConnection.CreateCommand();
        command.CommandText = "INSERT INTO Games(gameType,complete,time,numhits,numpowerups,win) VALUES (@type,@complete,@time,@numhits,@numpowerups,@win)";
        command.Parameters.Add(new SqliteParameter("@type", (int)gameData.GameType));
        command.Parameters.Add(new SqliteParameter("@complete", gameData.Complete ? 1 : 0));
        command.Parameters.Add(new SqliteParameter("@time", gameData.NumOfSeconds));
        command.Parameters.Add(new SqliteParameter("@numhits", gameData.NumPaddleHits));
        command.Parameters.Add(new SqliteParameter("@numpowerups", gameData.NumPowerUps));
        command.Parameters.Add(new SqliteParameter("@win", gameData.Win ? 1 : 0));
        command.ExecuteNonQuery();
    }
}
