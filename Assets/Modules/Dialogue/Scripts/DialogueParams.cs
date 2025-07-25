using System;
using UnityEngine;

namespace DialogueSystem
{

    [Serializable]
    public struct DialogueParams
    {
        public DialogueContent content;
        public DialogueSettings settings;
        public DialogueEventDispatcher eventDispatcher;
    }

}
