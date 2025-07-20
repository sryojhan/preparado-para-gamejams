using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace DialogueSystem
{
    public class DialogueEventDispatcher : MonoBehaviour
    {
        public void ProcessEvent(DialogueEvent eventType, string args)
        {
            foreach (var evtHdlr in eventHandlers)
            {
                if (evtHdlr.type == eventType)
                    evtHdlr.ProcessEvent(args);
            }
        }

        [Serializable]
        public class DialogueEventHandler
        {
            public DialogueEvent type;
            public UnityEvent<string> onEvent;
            public virtual void ProcessEvent(string args)
            {
                onEvent?.Invoke(args);
            }

            public DialogueEventHandler(DialogueEvent type, UnityAction<string> action)
            {
                this.type = type;
                onEvent = new UnityEvent<string>();

                onEvent.AddListener(action);
            }

        }

        public List<DialogueEventHandler> eventHandlers;
    }
}
