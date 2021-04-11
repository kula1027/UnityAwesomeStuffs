using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace AwesomeSnippets {

    public class MainThreadDispatcher : SingletonPreloaded<MainThreadDispatcher> {
        private readonly ConcurrentQueue<Action> queueActions = new ConcurrentQueue<Action>();

        public void Update() {
            while (queueActions.TryDequeue(out Action action)) {
                action.Invoke();
            }
        }

        /// <summary>
        /// Invokes input action in main thread
        /// </summary>
        /// <param name="action">aciton to be invoked in main thread</param>
        public void Dispatch(Action action) {
            if (action != null) {
                queueActions.Enqueue(action);
            } else {
                Debug.LogError("cannot enqeue null action");
            }
        }

        protected override void Awake() {
            base.Awake();

            DontDestroyOnLoad(gameObject);
        }
    }
}