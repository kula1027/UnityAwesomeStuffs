using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

/// <summary>
/// Modifies someModelData periodically
/// </summary>
namespace AwesomeSnippets {

    public class SomeLogicModule : MonoBehaviour {
        [SerializeField] private SomeModelData someModelData;

        private void Start() {
            StartCoroutine(RoutineInteger());
            StartCoroutine(RoutineFloat());
            StartCoroutine(RoutineVector());
            StartCoroutine(RoutineStrings());
        }

        private IEnumerator RoutineInteger() {
            while (true) {
                someModelData.IntegerData.Data++;

                yield return new WaitForSeconds(1);
            }
        }

        private IEnumerator RoutineFloat() {
            while (true) {
                someModelData.FloatData.Data += Time.deltaTime;

                yield return null;
            }
        }

        private IEnumerator RoutineVector() {
            while (true) {
                someModelData.VectorData.Data = new Vector2(someModelData.FloatData.Data, someModelData.FloatData.Data);

                yield return null;
            }
        }

        private IEnumerator RoutineStrings() {
            int count = 0;
            while (true) {
                someModelData.StringCollection.Add((count++).ToString());

                yield return new WaitForSeconds(1);
            }
        }
    }
}