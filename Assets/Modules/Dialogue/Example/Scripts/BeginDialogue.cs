using UnityEngine;

namespace DialogueSystem.Examples
{
    public class BeginDialogue : MonoBehaviour
    {
        public DialogueParams dialogueParams;

        private void Start()
        {
            START();
        }


        [EasyButtons.Button]
        public void START()
        {
            DialogueManager.instance.EndDialogue();
            DialogueManager.instance.BeginDialogue(dialogueParams);
        }
    }
}
