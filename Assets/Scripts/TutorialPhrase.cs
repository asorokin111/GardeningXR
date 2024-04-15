using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Tutorial/Tutorial Phrase")]
[Serializable]
public class TutorialPhrase : ScriptableObject
{
    public string text;
    public float phraseDuraction;
}
