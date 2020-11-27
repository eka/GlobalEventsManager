using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using YakDogGames.Events;
using YakDogGames.Tools;

namespace YakDogGames.Events
{
    public class GlobalEventsManager : Singleton<GlobalEventsManager>
    {
        private Dictionary<string, ArgEvent> _eventsRegistry = new Dictionary<string, ArgEvent>();

        public void TriggerEvent(string eventName)
        {
            Debug.Log($"TriggerEvent {eventName}");
            TriggerEvent(eventName, null);
        }

        public void TriggerEvent(string eventName, object arg0)
        {
            ArgEvent currentEvent;

            if (_eventsRegistry.TryGetValue(eventName, out currentEvent))
            {
                currentEvent.Invoke(arg0);
            }
            else
            {
                Debug.Log($"Event {eventName} doesn't exists.");
            }
        }

        public void ListenEvent(string eventName, UnityAction action)
        {
            var currentEvent = GetOrCreateEvent(eventName);
            currentEvent.AddListener((arg0) => { action(); });
        }

        public void ListenEvent(string eventName, UnityAction<object> action)
        {
            var currentEvent = GetOrCreateEvent(eventName);
            currentEvent.AddListener(action);
        }

        private ArgEvent GetOrCreateEvent(string eventName)
        {
            ArgEvent currentEvent;

            if (!_eventsRegistry.TryGetValue(eventName, out currentEvent))
            {
                currentEvent = new ArgEvent();
                _eventsRegistry[eventName] = currentEvent;
                Debug.Log($"Event {eventName} doesn't exist. Creating.");
            }

            return currentEvent;
        }
    }
}