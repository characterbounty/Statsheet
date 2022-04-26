using System.Collections.Generic;
using UnityEngine;

namespace StatSheet.Extensions
{
    public static class TransformExt
    {
        public static List<T> GetComponentsInFirstChildrenLayer<T>(this Transform parent)
        {
            var result = new List<T>();
            for (int i = 0; i < parent.childCount; i++)
            {
                var elem = parent.GetChild(i).GetComponent<T>();
                if (elem != null)
                {
                    result.Add(elem);
                }
            }
            return result;
        }
    }
}