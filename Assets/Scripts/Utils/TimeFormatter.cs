using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TimeFormatterUtil
{
    public class TimeFormatter : MonoBehaviour
    {
        public static string GetTextFromTime(float seconds)
        {

            if(seconds <= 0)
                return "00:00";

            string formattedText = "";
            string formattedHours = "";
            string formattedMins = "";
            string formattedSecs = "00";
            string formattedRemainder = "00";

            if(seconds == 0)
                return "00:00";

            if(true)
            {
                float remainder = seconds % 1;
                remainder *= 100;

                if(remainder >= 10)
                {
                    formattedRemainder = Math.Truncate(remainder).ToString();
                }
                else
                {
                    formattedRemainder = "0" + Math.Truncate(remainder).ToString();
                }
            }

            if(seconds % 60 != 0)
            {
                float secs = seconds % 60;

                if(secs >= 10)
                    formattedSecs = Math.Truncate(secs).ToString();
                else
                    formattedSecs = "0" + Math.Truncate(secs).ToString();
            }

            if(seconds % 3600 != 0)
            {
                float cleanMins = seconds % 3600;
                float mins = cleanMins / 60;

                if(mins >= 10)
                    formattedMins = Math.Truncate(mins).ToString();
                else
                    formattedMins = "0" + Math.Truncate(mins).ToString();
            }

            if(seconds >= 3600)
            {
                float hours = seconds / 3600;
                formattedHours = Math.Truncate(hours).ToString();
            }

            if(formattedHours != "")
                formattedText = formattedHours + ":" + formattedMins + ":" + formattedSecs + ":" + formattedRemainder;
            else
                formattedText = formattedMins + ":" + formattedSecs + ":" + formattedRemainder;

            return formattedText;
        }
    }
}
