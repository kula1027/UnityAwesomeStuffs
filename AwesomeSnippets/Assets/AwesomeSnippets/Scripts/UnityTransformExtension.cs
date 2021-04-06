using System.Collections.Generic;
using UnityEngine;

public static class UnityTransformExtension {

    /// <summary>
    /// find descendant by name matches first
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Transform FindDescendant(this Transform tr, string name) {
        Queue<Transform> queue = new Queue<Transform>();

        queue.Enqueue(tr);

        while (queue.Count > 0) {
            Transform current = queue.Dequeue();
            if (current.name == name) {
                return current;
            }

            foreach (Transform t in current) {
                queue.Enqueue(t);
            }
        }

        return null;
    }

    /// <summary>
    /// finds all of descendants by name
    /// </summary>
    /// <param name="tr"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static List<Transform> FindDescendantsAll(this Transform transform, string name) {
        Queue<Transform> queue = new Queue<Transform>();
        List<Transform> retList = new List<Transform>();

        queue.Enqueue(transform);

        while (queue.Count > 0) {
            Transform current = queue.Dequeue();

            if (current.name == name) {
                retList.Add(current);
            }

            foreach (Transform t in current) {
                queue.Enqueue(t);
            }
        }

        return retList;
    }

    /// <summary>
    /// returns all of descendants including itself
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static List<Transform> GetDescendantAll(this Transform transform) {
        Queue<Transform> queue = new Queue<Transform>();
        List<Transform> retList = new List<Transform>();

        queue.Enqueue(transform);

        while (queue.Count > 0) {
            Transform current = queue.Dequeue();

            retList.Add(current);

            foreach (Transform t in current.transform) {
                queue.Enqueue(t);
            }
        }

        return retList;
    }
}