using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BusEvent : Singleton<BusEvent>
{
    private Dictionary<string, UnityAction> eventTable = new();

    public static void Call(string eventName)
    {
        if (!instance.eventTable.ContainsKey(eventName)) return;

        instance.eventTable[eventName].Invoke();
    }

    public static void Subscribe(string eventName, UnityAction callback)
    {
        if (!instance.eventTable.ContainsKey(eventName))
        {
            instance.eventTable.Add(eventName, callback);
            return;
        }

        instance.eventTable[eventName] += callback;
    }

    public static void Unsubscribe(string eventName, UnityAction callback)
    {
        if (instance.eventTable.ContainsKey(eventName))
        {
            instance.eventTable[eventName] -= callback;


            if(instance.eventTable[eventName] == null)
            {
                instance.eventTable.Remove(eventName);
            }
        }
    }


}
