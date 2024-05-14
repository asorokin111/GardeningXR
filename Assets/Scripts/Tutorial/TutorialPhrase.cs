using UnityEngine;

[System.Serializable]
public struct TutorialPhrase 
{
    [TextArea(1, 3)]
    public string text;
    public bool isFillerText; // Filler == not manually triggered
}
