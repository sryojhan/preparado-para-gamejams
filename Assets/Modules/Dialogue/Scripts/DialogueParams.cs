using System;
using UnityEngine;

namespace DialogueSystem
{

    [Serializable]
    public struct DialogueParams
    {
        public DialogueContent content;
        public DialogueEventDispatcher eventDispatcher;
        public DialogueSettings settings;
    }

}
