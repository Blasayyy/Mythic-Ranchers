using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class TooltipFormater : MonoBehaviour
{
    public static TooltipFormater Instance { get; set; }

    [SerializeField]
    private Item[] itemList;
    [SerializeField]
    private Ability[] abilityList;
    [SerializeField]
    private int characterLimitPerLine;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        foreach (Item item in itemList)
        {
            item.tooltip = FormatTooltip(item.tooltip, characterLimitPerLine);   
        }
        foreach (Ability ability in abilityList)
        {
            ability.tooltip = FormatTooltip(ability.tooltip, characterLimitPerLine);   
        }
    }

    public string FormatTooltip(string input, int interval)
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
            int hyphenIndex = input.IndexOf('-', currentIndex, interval);

            if (hyphenIndex != -1 && hyphenIndex < currentIndex + interval)
            {
                stringBuilder.Append(input.Substring(currentIndex, hyphenIndex - currentIndex));
                stringBuilder.Append("\n");
                currentIndex = hyphenIndex + 1;
            }
            else if (spaceIndex != -1 && spaceIndex >= currentIndex)
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
