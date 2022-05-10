using System;
using System.Collections.Generic;
using UnityEngine;

namespace IngameConsole {

    public class PoolableObject : MonoBehaviour {
        public ObjectPooler MotherPool { get; set; }

        public void ReturnSelf() {
            MotherPool.GiveMeBack(gameObject);
        }
    }

    public class ObjectPooler : MonoBehaviour {
        private const int DefaultCountPerSpawn = 64;
        private const int MaxCountPool = 256;

        private readonly Queue<GameObject> queue = new Queue<GameObject>();

        private GameObject poolTarget;
        private Action<GameObject> onSpawnObject;
        private int countPerSpawn;

        public void Initialize(GameObject poolTarget, Action<GameObject> onSpawnObject, int countPerSpawn = DefaultCountPerSpawn) {
            this.poolTarget = poolTarget;
            this.onSpawnObject = onSpawnObject;
            this.countPerSpawn = countPerSpawn;

            gameObject.SetActive(false);
        }

        public GameObject TakeThis() {
            if (queue.Count <= 0) {
                SpawnObjects();
            }

            return queue.Dequeue();
        }

        public void GiveMeBack(GameObject go) {
            if (queue.Count <= MaxCountPool) {
                go.transform.SetParent(transform);

                queue.Enqueue(go);
            } else {
                Destroy(go);
            }
        }

        private void SpawnObjects() {
            for (int loop = 0; loop < countPerSpawn; loop++) {
                PoolableObject obj = Instantiate(poolTarget, transform).GetComponent<PoolableObject>();

                obj.MotherPool = this;

                queue.Enqueue(obj.gameObject);

                onSpawnObject?.Invoke(obj.gameObject);
            }
        }
    }
}