using System;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(menuName = "Dialogue/Dialogue Content", fileName = "New Dialogue Content")]
    public class DialogueContent : ScriptableObject
    {
        [Serializable]
        public class DialoguePage
        {
            public string speaker;
            public Sprite speakerSprite;


            [TextArea]
            public string message;
        }

        public DialoguePage[] pages;

        public string[] choices;

        public bool isLastOptionCancel = false;
    }
}
