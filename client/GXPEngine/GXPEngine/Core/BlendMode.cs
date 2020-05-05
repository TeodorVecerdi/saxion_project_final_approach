using System;
using GXPEngine.OpenGL;

namespace GXPEngine {
    /// <summary>
    ///     Defines different BlendModes, only two present now, but you can add your own.
    /// </summary>
    public class BlendMode {
        #region Delegates
        public delegate void Action();
        #endregion

        public static readonly BlendMode NORMAL = new BlendMode(
            "Normal", () => { GL.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA); }
        );

        public static readonly BlendMode MULTIPLY = new BlendMode(
            "Multiply", () => { GL.BlendFunc(GL.DST_COLOR, GL.ZERO); }
        );

        /// <summary>
        ///     This should point to an anonymous function updating the blendfunc
        /// </summary>
        public readonly Action enable;

        /// <summary>
        ///     A label for this blendmode
        /// </summary>
        public readonly string label;

        public BlendMode(string pLabel, Action pEnable) {
            if (pEnable == null)
                throw new Exception("Enabled action cannot be null");
            enable = pEnable;

            label = pLabel;
        }

        public override string ToString() {
            return label;
        }
    }
}