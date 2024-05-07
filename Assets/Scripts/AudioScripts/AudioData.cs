using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "ScriptableObjects/AudioData", order = 3)]
public class AudioData : ScriptableObject
{
    public List<AudioObject> AudioObjects;
}
