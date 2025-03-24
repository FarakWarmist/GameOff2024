using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Localization : MonoBehaviour
{
    static Dictionary<string, string> english = new();
    static Dictionary<string, string> french = new();

    static bool isEnglish = false;

    private void Awake()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "TP3 - Languages.tsv");
        string[] allText = File.ReadAllLines(filePath);

        for (int i = 1; i < allText.Length; i++)
        {
            string[] columns = allText[i].Split('\t');
            string key = columns[0];
            string enWord = columns[2];
            string frWord = columns[3];
            english.Add(key, enWord);
            french.Add(key, frWord);
        }
    }

    private void Start()
    {
        
    }

    internal static string GetString(string key)
    {
        return isEnglish ? english[key] : french[key];
    }
}
