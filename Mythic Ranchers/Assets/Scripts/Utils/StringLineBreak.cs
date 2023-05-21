using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public static class StringLineBreak
{
    public static string InsertLineBreaks(string input, int interval)
    {
        if (input.Length <= interval)
            return input;

        StringBuilder stringBuilder = new StringBuilder();
        int currentIndex = 0;

        while (currentIndex < input.Length)
        {
            if (currentIndex + interval >= input.Length)
            {
                stringBuilder.Append(input.Substring(currentIndex));
                break;
            }

            int spaceIndex = input.LastIndexOf(' ', currentIndex + interval, interval);
            if (spaceIndex != -1 && spaceIndex >= currentIndex)
            {
                stringBuilder.Append(input.Substring(currentIndex, spaceIndex - currentIndex));
                stringBuilder.Append("\n");
                currentIndex = spaceIndex + 1;
            }
            else
            {
                stringBuilder.Append(input.Substring(currentIndex, interval));
                stringBuilder.Append("\n");
                currentIndex += interval;
            }
        }

        return stringBuilder.ToString();
    }
}
