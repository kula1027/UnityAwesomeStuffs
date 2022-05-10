using System;
using System.Collections.Generic;

namespace IngameConsole {

    /// <summary>
    /// https://stackoverflow.com/questions/481603/set-extend-listt-length-in-c-sharp/481658#481658
    /// </summary>
    public static class CollectionsUtil {

        public static List<T> EnsureSize<T>(this List<T> list, int size) {
            return EnsureSize(list, size, default(T));
        }

        public static List<T> EnsureSize<T>(this List<T> list, int size, T value) {
            if (list == null)
                throw new ArgumentNullException("list");
            if (size < 0)
                throw new ArgumentOutOfRangeException("size");

            int count = list.Count;
            if (count < size) {
                int capacity = list.Capacity;
                if (capacity < size)
                    list.Capacity = Math.Max(size, capacity * 2);

                while (count < size) {
                    list.Add(value);
                    ++count;
                }
            }

            return list;
        }
    }
}