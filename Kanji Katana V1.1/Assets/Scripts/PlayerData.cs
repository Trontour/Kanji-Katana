using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;



[Serializable]
public class PlayerData : MonoBehaviour
{
    
    private void Start()
    {
        //Save();
        //Load();
        //saveNewHiragana("が", "ga");
        //saveNewHiragana("ら", "ra");
        //saveNewHiragana("ぽ", "po");
    }
    private void Update()
    {

    }
    private void Save()
    {

        List<HiraganaObject> hiraganas = new List<HiraganaObject>();
        //hiraganas.Add(new HiraganaObject("か", "ka"));
        string json = JsonHelper.ToJson<HiraganaObject>(hiraganas.ToArray());
        //Debug.Log(json);

        File.WriteAllText(Application.dataPath + "/hiragana.txt", json);
        //Debug.Log("Data Path: " + Application.dataPath);

    }
    public void clearAllHiragana()
    {

        string path = Application.dataPath + "/hiragana.txt";

        // Check if file exists to avoid exceptions
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("File deleted successfully.");
        }
        else
        {
            Debug.Log("File not found.");
        }
        Save();

    }
    public void saveNewHiragana(string hiragana, string romaji, int daysTillDue)//Appends a new hiragana to the player data on hiraganas
    {

        //READS EXISTING HIRAGANA LIST
        string content = File.ReadAllText(Application.dataPath + "/hiragana.txt");
        List<HiraganaObject> hiraganas = JsonHelper.FromJson<HiraganaObject>(content).ToList<HiraganaObject>();


        //CREATES NEW HIRA
        HiraganaObject newHira = new HiraganaObject(hiragana, romaji, daysTillDue);
        if (!hiraganas.Contains(newHira)) //ONLY ADDS IF DOES NOT YET EXIST
            hiraganas.Add(newHira);
        string json = JsonHelper.ToJson<HiraganaObject>(hiraganas.ToArray());
        //Debug.Log(json);

        //OVERRIDES OLD HIRAGANAS WITH NEW HIRAGANAS
        File.WriteAllText(Application.dataPath + "/hiragana.txt", json);
        //Debug.Log("Data Path: " + Application.dataPath);

    }
    public List<HiraganaObject> getHiraganas()
    {
        string content = File.ReadAllText(Application.dataPath + "/hiragana.txt");
        List<HiraganaObject> hiraganas = JsonHelper.FromJson<HiraganaObject>(content).ToList<HiraganaObject>();
        //Debug.Log("TEST: "+hiraganas[0].romaji);
        return hiraganas;
    }

    


    
}

[Serializable]
public class SaveObject
{
    //public List<HiraganaObject> hiraganas = new List<HiraganaObject>();
    public HiraganaObject[] hiraganas = { new HiraganaObject("a", "b"), new HiraganaObject("c", "d"), new HiraganaObject("e", "f") };
    //public void addHiragana(string hiragana, string romaji)
    //{
    //    HiraganaObject newHiragana = new HiraganaObject();
    //    newHiragana.hiragana = hiragana;
    //    newHiragana.romaji = romaji;
    //    hiraganas.Add(newHiragana);

    //}
    public void addHiragana(string hiragana, string romaji)
    {
        hiraganas[0].hiragana = hiragana;
        hiraganas[0].romaji = romaji;

    }
}
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}

[Serializable]
public class HiraganaObject
{
    public string hiragana;
    public string romaji;
    public int daysTillDue;
    private int previousDaysTillDue;
    public float easyInterval;
    public float hardInterval;
    

    public HiraganaObject()
    {
        romaji = "";
        hiragana = "";
        daysTillDue = -1;
    }
    public HiraganaObject(string hir, string rom)
    {
        romaji = rom;
        hiragana = hir;
        daysTillDue = 1;
        easyInterval = 2;
        hardInterval = 0.5f;
        previousDaysTillDue = 1;
    }
    public HiraganaObject(string hir, string rom, int daysDue)
    {
        romaji = rom;
        hiragana = hir;
        daysTillDue = daysDue;
        previousDaysTillDue = daysDue;
        easyInterval = 2;
        hardInterval = 0.5f;

    }
    public void setEasyInterval(float _easyInterval)
    {
        easyInterval = _easyInterval;
    }
    public void setHardInterval(float _hardInterval)
    {
        hardInterval = _hardInterval;
    }
    public void applyEasyInterval()
    {
        float temp = previousDaysTillDue * easyInterval;
        daysTillDue = (int)temp;
        previousDaysTillDue = daysTillDue;
    }
    public void applyHardInterval()
    {
        float temp = previousDaysTillDue * hardInterval;
        daysTillDue = (int)temp;
        previousDaysTillDue = daysTillDue;
    }
    public bool Equals(HiraganaObject other)
    {
        if (this.hiragana == other.hiragana && this.romaji == other.romaji)
            return true;
        return false;
    }

}