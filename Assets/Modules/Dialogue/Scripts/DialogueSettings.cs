using System;
using TMPro;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(menuName = "Dialogue/Dialogue Settings", fileName = "New Dialogue Options")]
    public class DialogueSettings : ScriptableObject
    {
        [Header("Text")]
        public TMP_FontAsset font;
        public float fontSize = 32;

        [Header("Reveal")]
        public Interpolation textRevealInterpolation;

        [Header("Timing")]
        public float baseRevealTimePerChar = 0.1f;
        public float comaWaitTime = 6;
        public float dotWaitTime = 12;
        public float timeToWaitBeforeSkip = 0.1f;

        [Header("Sound")]
        public AudioClip[] letterRevealSound;
        public bool playSoundOnEachCharacter = false;

        [Header("Effects")]
        public DialogueCommand[] commands;

        [Serializable]
        public class DialogueCommand
        {
            public string command;
            public DialogueEffect associatedEffect;
        }

    }
}
