using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public enum DialogueEvent
    {
        OnDialogueStart, OnDialogueEnd,
        OnPageStart, OnPageEnd,
        OnSpeakerChange,
        OnSelectionPresented, OnSelectionChosen,
        OnDialogueSkip,
        OnCharacterRevealed
    }

    public class DialogueManager : Singleton<DialogueManager>
    {
        public DialogueSettings defaultDialogueSettings;

        public TextMeshProUGUI speaker;
        public TextMeshProUGUI text;

        public RectTransform textBackground;
        public RectTransform speakerBackground;

        public Image leftSpeakerSprite;
        public Image rightSpeakerSprite;


        private Coroutine dialogueCoroutine;

        DialogueParams? currentlyUsedParams = null;

        public void BeginDialogue(DialogueParams dialogueParams)
        {
            dialogueParams.settings =
            dialogueParams.settings != null ? dialogueParams.settings : defaultDialogueSettings;


            dialogueCoroutine = StartCoroutine(PlayDialogue(dialogueParams));
        }


        public void EndDialogue()
        {
            if (dialogueCoroutine != null)
            {
                StopCoroutine(dialogueCoroutine);
                dialogueCoroutine = null;
            }
        }


        IEnumerator PlayDialogue(DialogueParams dialogueParams)
        {
            currentlyUsedParams = dialogueParams;

            DialogueContent content = dialogueParams.content;
            DialogueSettings settings = dialogueParams.settings;
            DialogueEventDispatcher events = dialogueParams.eventDispatcher;

            if(content.pages.Length <= 0)
            {
                Debug.LogError("Dialogue System: Trying to start empty dialogue");
                yield break;
            }


            int currentPageIdx = 0;

            while(currentPageIdx < content.pages.Length)
            {
                DialogueContent.DialoguePage page = content.pages[currentPageIdx];

                string rawText = page.message;

                text.text = rawText;
                text.maxVisibleCharacters = 0;

                int textLength = rawText.Length;

                int previousCharacterCount = 0;


                List<float> ponderedTimeline = CreatePonderedTimeLine(rawText, settings);

                float maxPonderedTime = ponderedTimeline[^1];

                float revealDuration = maxPonderedTime * settings.baseRevealTimePerChar;
                float revealSpeed = 1.0f / revealDuration;

                for(float rawProgress = 0; rawProgress < 1; rawProgress += Time.deltaTime * revealSpeed)
                {
                    float interpolated = settings.textRevealInterpolation.Interpolate(rawProgress);

                    int ponderedPosition = ponderedTimeline.BinarySearch(interpolated * maxPonderedTime);

                    if (ponderedPosition < 0) ponderedPosition = ~ponderedPosition;

                    int characterCount = ponderedPosition;

                    if(characterCount > previousCharacterCount)
                    {
                        for(int newCharacterIdx = previousCharacterCount; newCharacterIdx < characterCount; newCharacterIdx++)
                        {
                            events.ProcessEvent(DialogueEvent.OnCharacterRevealed, rawText[newCharacterIdx].ToString());
                        }
                    }

                    previousCharacterCount = characterCount;

                    text.maxVisibleCharacters = characterCount;

                    yield return null;
                }

                text.maxVisibleCharacters = textLength;


                currentPageIdx += 1;


                yield return null;

            }






            currentlyUsedParams = null;
            
        }



        List<float> CreatePonderedTimeLine(string text, DialogueSettings settings)
        {
            List<float> timeline = new();


            float totalTime = 0;

            foreach (char character in text)
            {
                timeline.Add(totalTime);

                float weight = 1;

                if (character == ',') weight = settings.comaExtraPauseMultiplier;
                else if (character == '.') weight = settings.dotExtraPauseMultiplier;

                totalTime += weight;
            }

            return timeline;
        }


        IEnumerator PlayTextEffects()
        {
            yield return null;
        }


        public DialogueSettings GetCurrentDialogueSettings()
        {
            if (!currentlyUsedParams.HasValue) return null;

            return currentlyUsedParams.Value.settings;
        }

    }
}
