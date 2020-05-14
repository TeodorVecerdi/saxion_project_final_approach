using System;
using System.Collections.Generic;

namespace game.utils {
    public static class Extensions {
        public static List<T> Copy<T>(this List<T> list) {
            return new List<T>(list);
        }

        public static List<T> Sorted<T>(this List<T> list) where T : IComparable<T> {
            var copy = list.Copy();
            copy.Sort();
            return copy;
        }

        public static List<string> Sorted(this List<string> list) {
            var copy = list.Copy();
            copy.Sort(string.CompareOrdinal);
            return copy;
        }

        public static string Capitalize(this string s) {
            return new string(char.ToUpperInvariant(s[0]), 1) + s.Substring(1);
        }
    }
}