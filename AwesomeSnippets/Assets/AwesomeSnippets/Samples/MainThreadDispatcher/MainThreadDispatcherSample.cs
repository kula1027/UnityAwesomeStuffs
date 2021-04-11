using AwesomeSnippets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AwesomeNamespace {

    public class MainThreadDispatcherSample : MonoBehaviour {
        [SerializeField] private TextMeshPro tmp;

        private int num = 0;

        private Thread[] threads;

        private void Start() {
            threads = new Thread[16];

            for (int loop = 0; loop < threads.Length; loop++) {
                int randomInterval = Random.Range(4, 10) * 100;
                threads[loop] = new Thread(() => AddIntegerToTextRandomInterval(randomInterval));
                threads[loop].Start();
            }
        }

        private void AddIntegerToTextRandomInterval(int itv) {
            Debug.Log($"Add to text every {itv}ms");
            while (true) {
                MainThreadDispatcher.Instance.Dispatch(() => {
                    tmp.text = $"{num++}";
                });

                Thread.Sleep(itv);
            }
        }

        private void OnDestroy() {
            if (threads != null) {
                for (int loop = 0; loop < threads.Length; loop++) {
                    threads[loop]?.Abort();
                }
            }
        }
    }
}