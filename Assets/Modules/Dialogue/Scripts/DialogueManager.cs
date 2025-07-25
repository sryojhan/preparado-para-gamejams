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



        class Sentence
        {
            public int beginning;
            public int end;

            public enum Separator
            {
                Dot, Coma, QuestionMark, ExclamationMark, End, None
            }

            public Separator separator;
        }


        IEnumerator PlayDialogue(DialogueParams dialogueParams)
        {
            currentlyUsedParams = dialogueParams;

            DialogueContent content = dialogueParams.content;
            DialogueSettings settings = dialogueParams.settings;
            DialogueEventDispatcher events = dialogueParams.eventDispatcher;

            if (content.pages.Length <= 0)
            {
                Debug.LogError("Dialogue System: Trying to start empty dialogue");
                yield break;
            }


            int currentPageIdx = 0;

            while (currentPageIdx < content.pages.Length)
            {
                DialogueContent.DialoguePage page = content.pages[currentPageIdx];

                string rawText = page.message;




                text.text = rawText;
                text.maxVisibleCharacters = 0;

                int previousCharacterCount = 0;

                Sentence[] sentences = GenerateSentences(rawText);


                foreach (Sentence sentence in sentences)
                {
                    int sentenceLength = sentence.end - sentence.beginning;

                    float revealDuration = sentenceLength * settings.baseRevealTimePerChar;
                    float revealSpeed = 1.0f / revealDuration;

                    for (float rawProgress = 0; rawProgress < 1; rawProgress += Time.deltaTime * revealSpeed)
                    {
                        float interpolated = settings.textRevealInterpolation.Interpolate(rawProgress);

                        int characterCount = Mathf.FloorToInt(Mathf.Lerp(sentence.beginning, sentence.end, interpolated));

                        if (characterCount > previousCharacterCount)
                        {
                            for (int newCharacterIdx = previousCharacterCount; newCharacterIdx < characterCount; newCharacterIdx++)
                            {
                                events.ProcessEvent(DialogueEvent.OnCharacterRevealed, rawText[newCharacterIdx].ToString());
                            }
                        }

                        previousCharacterCount = characterCount;

                        text.maxVisibleCharacters = characterCount;

                        yield return null;
                    }

                    text.maxVisibleCharacters = sentence.end;


                    if (sentence.separator != Sentence.Separator.End)
                        yield return new WaitForSeconds(sentence.separator == Sentence.Separator.Coma ? settings.comaWaitTime : settings.dotWaitTime);
                }


                currentPageIdx += 1;

                yield return null;

            }



            print("END");

            currentlyUsedParams = null;
        }


        private Sentence[] GenerateSentences(string text)
        {
            List<Sentence> sentences = new();

            int beginning = 0;
            int current = 0;

            foreach (char c in text)
            {
                Sentence.Separator separator = Sentence.Separator.None;

                if (c == '.')
                {
                    separator = Sentence.Separator.Dot;
                }
                else if (c == ',')
                {
                    separator = Sentence.Separator.Coma;
                }
                else if (c == '?')
                {
                    separator = Sentence.Separator.QuestionMark;
                }
                else if (c == '!')
                {
                    separator = Sentence.Separator.ExclamationMark;
                }


                if (separator != Sentence.Separator.None)
                {
                    Sentence newSentence = new()
                    {
                        beginning = beginning,
                        end = current + 1,
                        separator = separator
                    };
                    sentences.Add(newSentence);

                    beginning = current + 1;
                }

                current++;
            }


            if (beginning < text.Length) //the text doesnt end with punctuation
            {
                Sentence lastSentence = new ()
                {
                    beginning = beginning,
                    end = current,
                    separator = Sentence.Separator.End
                };
                sentences.Add(lastSentence);
            }


            return sentences.ToArray();
        }


        public DialogueSettings GetCurrentDialogueSettings()
        {
            if (!currentlyUsedParams.HasValue) return null;

            return currentlyUsedParams.Value.settings;
        }

    }
}
