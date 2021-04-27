using System;
using System.Collections.Concurrent;
using UnityEngine;

namespace AwesomeSnippets {
    public class MainThreadDispatcher : SingletonPreloaded<MainThreadDispatcher> {
        private readonly ConcurrentQueue<Action> queueActions = new ConcurrentQueue<Action>();

        protected override void Awake() {
            base.Awake();

            DontDestroyOnLoad(gameObject);
        }

        public void Update() {
            while (queueActions.TryDequeue(out Action action)) {
                action.Invoke();
            }
        }

        /// <summary>
        /// Invokes input action in main thread
        /// </summary>
        /// <param name="action">action to be invoked in main thread</param>
        public void Dispatch(Action action) {
            if (action != null) {
                queueActions.Enqueue(action);
            } else {
                Debug.LogError("cannot enqueue null action");
            }
        }
    }
}