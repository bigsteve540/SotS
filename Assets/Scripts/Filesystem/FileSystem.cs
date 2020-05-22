using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FileSystem
{
    #region Character read write
    public static void WriteCharacterFile(string _charIDFileName, string _charName, int _level, float[] _colour, int _charClass)
    {
        try
        {
            string filePath = Path.Combine(Application.persistentDataPath, "userdata");
            filePath = Path.Combine(filePath, _charIDFileName);

            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            _charIDFileName += ".char";
            filePath = Path.Combine(filePath, _charIDFileName);

            string fileData = string.Join("|", _charName, _level.ToString(), _charClass.ToString(), _colour[0].ToString(), _colour[1].ToString(), _colour[2].ToString());
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write(fileData);
            }
        }
        catch (Exception _ex)
        {
            Logger.Print($"Failed to write save file: {_ex}", LogLevel.error);
        }
    }
    public static Dictionary<string, PlayerSave> ReadCharacterFiles()
    {
        Dictionary<string, PlayerSave> characters = new Dictionary<string, PlayerSave>();

        try
        {
            string filePath = Path.Combine(Application.persistentDataPath, "userdata");

            Logger.Print($"Reading saves from: {filePath}...", LogLevel.info);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
                return characters;
            }

            string[] directories = Directory.GetDirectories(filePath);

            for (int i = 0; i < directories.Length; i++)
            {
                string directoryName = Path.GetFileName(directories[i]);
                string newFilePath = Path.Combine(directories[i], directoryName);
                newFilePath += ".char";

                using (StreamReader sw = new StreamReader(newFilePath))
                {
                    DeserializeCharacterFromFile(newFilePath, sw.ReadLine(), out string guid, out PlayerSave data);
                    characters.Add(guid, data);
                }
            }

            return characters;
        }
        catch (Exception _ex)
        {
            Logger.Print($"Failed to read save file: {_ex}", LogLevel.error);
            return null;
        }

    }
    #endregion

    public static void WriteWorldFile(string _worldFileName)
    {
        try
        {
            string filePath = Path.Combine(Application.persistentDataPath, "world");

            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            _worldFileName += ".wrld";
            filePath = Path.Combine(filePath, _worldFileName);

            string fileData = string.Join("|",
                WorldSettings.DayDuration.ToString(),
                WorldSettings.NightDuration.ToString(),
                WorldSettings.CurrentWorldTime.ToString(),
                WorldSettings.BuildingStates[BuildingType.blacksmith].ToString(),
                WorldSettings.BuildingStates[BuildingType.church].ToString(),
                WorldSettings.BuildingStates[BuildingType.auctionHouse].ToString(),
                WorldSettings.BuildingStates[BuildingType.apothecary].ToString(),
                WorldSettings.BuildingStates[BuildingType.farm].ToString(),
                WorldSettings.BuildingStates[BuildingType.bank].ToString(),
                WorldSettings.BuildingStates[BuildingType.house].ToString(),
                WorldSettings.BuildingStates[BuildingType.wizardsTower].ToString(),
                WorldSettings.BuildingStates[BuildingType.archeryRange].ToString(),
                WorldSettings.BuildingStates[BuildingType.thievesGuild].ToString(),
                WorldSettings.BuildingStates[BuildingType.outpost].ToString(),
                WorldSettings.BuildingStates[BuildingType.artisansGuild].ToString()
                );

            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write(fileData);
            }
        }
        catch(Exception _ex)
        {
            Logger.Print($"Failed to write world save: {_ex}", LogLevel.error);
        }
    }

    public static string[] ReadWorldFile()
    {
        string[] data;

        try
        {
            string filePath = Path.Combine(Application.persistentDataPath, "world");

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
                return null;
            }

            string directoryName = Path.GetFileName(filePath);
            string newFilePath = Path.Combine(filePath, directoryName);
            newFilePath += ".wrld";

            if (File.Exists(newFilePath))
            {
                using (StreamReader sr = new StreamReader(newFilePath))
                {
                    data = sr.ReadLine().Split('|');
                }
                return data;
            }
            return null;
        }
        catch (Exception _ex)
        {
            Logger.Print($"Failed to read world file: {_ex}", LogLevel.error);
            return null;
        }
    }


    private static void DeserializeCharacterFromFile(string _fileName, string _data, out string _guid, out PlayerSave _userData)
    {
         _guid = Path.GetFileName(_fileName).Split('.')[0].Trim();

        string[] values = _data.Split('|');

        _userData = new PlayerSave(values[0], Convert.ToInt32(values[1]), Convert.ToInt32(values[2]), 
            new float[3] 
            {
                (float)Convert.ToDouble(values[3]),
                (float)Convert.ToDouble(values[4]),
                (float)Convert.ToDouble(values[5])
            }
        );
    }
}
