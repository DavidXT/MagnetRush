using System.Collections.Generic;
using UnityEngine;
public static class MyUtils {
    public static T GetRandom<T>(this List<T> list) {
        return list[Random.Range(0, list.Count)];
    }
    public static bool IsNullOrEmpty<T>(this List<T> list) {
        return list == null || list.Count == 0;
    }
}