using UnityEngine;

namespace DialogueSystem.Examples
{
    public class PlayDialogueSoundOnCharacterReveal : MonoBehaviour
    {
        public float maxPitchVariation = 0.2f;

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

            var settings = DialogueManager.instance.GetCurrentDialogueSettings();

            if (settings.letterRevealSound.Length == 0) return;

            AudioClip clip = settings.letterRevealSound[Random.Range(0, settings.letterRevealSound.Length)];

            audioSource.pitch = 1 + Random.insideUnitCircle.x * maxPitchVariation;
            audioSource.PlayOneShot(clip);
        }
    }
}
