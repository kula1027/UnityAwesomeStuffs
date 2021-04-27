using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace AwesomeSnippets {
    public class SingletonCaller : MonoBehaviour {
        private void Start() {
            Stopwatch sw = new Stopwatch();

            ////////////////////////////////////////////////////

            AwesomePreloadedSingleton somePreloadedSingleton;
            sw.Start();
            for (int i = 0; i < 100000000; ++i) {
                somePreloadedSingleton = AwesomePreloadedSingleton.Instance;
            }

            sw.Stop();
            Debug.Log($"PreloadedSingleton: {sw.ElapsedMilliseconds}ms");

            ////////////////////////////////////////////////////

            CommonSingleton commonSingleton;
            AwesomeSingleton awesomeSingleton;
            sw.Restart();
            for (int i = 0; i < 100000000; ++i) {
                commonSingleton = CommonSingleton.Instance;
            }

            sw.Stop();
            Debug.Log($"commonSingleton: {sw.ElapsedMilliseconds}ms");
            sw.Restart();
            for (int i = 0; i < 100000000; ++i) {
                awesomeSingleton = AwesomeSingleton.Instance;
            }

            sw.Stop();
            Debug.Log($"awesomeSingleton: {sw.ElapsedMilliseconds}ms");

            ////////////////////////////////////////////////////

            CommonThreadSafeSingleton commonThreadSafeSingleton;
            AwesomeThreadSafeSingleton awesomeThreadSafeSingleton;
            sw.Restart();
            for (int i = 0; i < 100000000; ++i) {
                commonThreadSafeSingleton = CommonThreadSafeSingleton.Instance;
            }

            sw.Stop();
            Debug.Log($"commonThreadSafeSingleton: {sw.ElapsedMilliseconds}ms");

            sw.Restart();
            for (int i = 0; i < 100000000; ++i) {
                awesomeThreadSafeSingleton = AwesomeThreadSafeSingleton.Instance;
            }

            sw.Stop();
            Debug.Log($"awesomeThreadSafeSingleton: {sw.ElapsedMilliseconds}ms");
        }
    }
}