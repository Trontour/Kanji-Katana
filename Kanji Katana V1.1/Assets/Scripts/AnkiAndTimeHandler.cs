using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnkiAndTimeHandler : MonoBehaviour

{
    public int firstDayYear;
    public int firstDayMonth;
    public int firstDayDay;
    public int currentDaySinceStart;
    public int lastDayOpenedYear;
    public int lastDayOpenedMonth;
    public int lastDayOpenedDay;
    public int todayYear;
    public int todayMonth;
    public int todayDay;


    // Start is called before the first frame update
    void Start()
    {


        initiizeDates();
        
        DateTime now = DateTime.Now;

        

        
        if (lastDayOpenedDay == 0 || lastDayOpenedMonth == 0 || lastDayOpenedYear == 0)
        {
            if (lastDayOpenedDay == 0) lastDayOpenedDay = now.Day;
            if (lastDayOpenedMonth == 0) lastDayOpenedMonth = now.Month;
            if (lastDayOpenedYear == 0) lastDayOpenedYear = now.Year;
            
        }
        if (todayDay == 0 || todayMonth == 0 || todayYear == 0)
        {
            if (todayDay == 0) todayDay = now.Day;
            if (todayMonth == 0) todayMonth = now.Month;
            if (todayYear == 0) todayYear = now.Year;

        }


        DateTime todayDate = new DateTime(todayYear, todayMonth, todayDay); //This would be the same as "now" in normal cases, but I am coding this like this for debugging purposes
        DateTime lastDayOpenedDate = new DateTime(lastDayOpenedYear, lastDayOpenedMonth, lastDayOpenedDay);



        if (firstDayYear == 0 || firstDayMonth == 0 || firstDayYear == 0)
        {
            if (firstDayDay == 0) firstDayDay = now.Day;
            if (firstDayMonth == 0) firstDayMonth = now.Month;
            if (firstDayYear == 0) firstDayYear = now.Year;
        }
        else
        {
            DateTime firstDayDate = new DateTime(firstDayYear, firstDayMonth, firstDayDay);
            currentDaySinceStart = (todayDate - firstDayDate).Days;
        }


        updateHiraganaDueDates((todayDate-lastDayOpenedDate).Days);





        PlayerPrefs.SetInt("lastDayOpenedDay", todayDay);
        PlayerPrefs.SetInt("lastDayOpenedMonth", todayMonth);
        PlayerPrefs.SetInt("lastDayOpenedYear", todayYear);
    }

    private void initiizeDates()
    {
        firstDayDay = PlayerPrefs.GetInt("firstDayDay");
        firstDayMonth = PlayerPrefs.GetInt("firstDayMonth");
        firstDayYear = PlayerPrefs.GetInt("firstDayYear");

        lastDayOpenedDay = PlayerPrefs.GetInt("lastDayOpenedDay");
        lastDayOpenedMonth = PlayerPrefs.GetInt("lastDayOpenedMonth");
        lastDayOpenedYear = PlayerPrefs.GetInt("lastDayOpenedYear");

        todayDay = PlayerPrefs.GetInt("todayDay");
        todayMonth = PlayerPrefs.GetInt("todayMonth");
        todayYear = PlayerPrefs.GetInt("todayYear");

        currentDaySinceStart = PlayerPrefs.GetInt("currentDaySinceStart");
    }
    private void updateHiraganaDueDates(int daysPassed)
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
