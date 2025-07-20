using UnityEngine;

namespace DialogueSystem.Examples
{
    public class PlayDialogueSound : MonoBehaviour
    {
        private AudioSource audioSource;
        private void Start()
        {
            audioSource = gameObject.AddComponent<AudioSource>();


            GetComponent<DialogueEventDispatcher>().eventHandlers.Add(
                new DialogueEventDispatcher.DialogueEventHandler(DialogueEvent.OnCharacterRevealed, PlayAudio)
            );
        }

        public void PlayAudio(string args)
        {
            if (args[0] == ' ') return;

            audioSource.PlayOneShot(DialogueManager.instance.GetCurrentDialogueSettings().letterRevealSound);
        }
    }
}
