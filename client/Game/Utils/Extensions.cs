using System.Collections.Generic;

namespace game.utils {
    public static class Extensions {
        public static List<T> Copy<T>(this List<T> list) {
            return new List<T>(list);
        }
    }
}