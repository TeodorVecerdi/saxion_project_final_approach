using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using GXPEngine;

namespace physics_programming.final_assignment.Utils {
    /// <summary>
    /// Debug class with utility methods: Log, LogWarning, LogError and Assert
    /// </summary>
    public static class Debug {
        private const string LogFormat = " at {0}.{1}:{2} ({3}:line {2})";

        private static string GetString(object message) {
            if (message == null) return "Null";

            var formattable = message as IFormattable;
            return formattable != null ? formattable.ToString(null, CultureInfo.InvariantCulture) : message.ToString();
        }

        public static void Log(object message, string messageTitle = "LOG") {
#if DEBUG
            var stack = new StackFrame(1, true);
            var mth = stack.GetMethod();
            var fname = stack.GetFileName();
            var lineNumber = stack.GetFileLineNumber();
            var fileName = fname?.Substring(fname.LastIndexOf("\\", StringComparison.InvariantCulture)+1);
            var className = mth.ReflectedType?.Name;
            var method = new StringBuilder();
            method.Append(mth.Name);
            method.Append("(");
            var methodParameters = mth.GetParameters();
            for (var i = 0; i < methodParameters.Length; i++) {
                method.Append(methodParameters[i].ParameterType);
                if (i != methodParameters.Length - 1) method.Append(", ");
            }

            method.Append(")");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Write("[" + messageTitle + "]");
            Console.ResetColor();
            Console.Write(" " + GetString(message));
            Console.ResetColor();
            Console.WriteLine(LogFormat.format(className, method, lineNumber, fileName));
#endif
        }

        public static void LogWarning(object message, string messageTitle = "WARN") {
#if DEBUG
            var stack = new StackFrame(1, true);
            var mth = stack.GetMethod();
            var fname = stack.GetFileName();
            var lineNumber = stack.GetFileLineNumber();
            var fileName = fname?.Substring(fname.LastIndexOf("\\", StringComparison.InvariantCulture)+1);
            var className = mth.ReflectedType?.Name;
            var method = new StringBuilder();
            method.Append(mth.Name);
            method.Append("(");
            var methodParameters = mth.GetParameters();
            for (var i = 0; i < methodParameters.Length; i++) {
                method.Append(methodParameters[i].ParameterType);
                if (i != methodParameters.Length - 1) method.Append(", ");
            }

            method.Append(")");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.Write("[" + messageTitle + "]");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(" " + GetString(message));
            Console.ResetColor();
            Console.WriteLine(LogFormat.format(className, method, lineNumber, fileName));
            Console.ResetColor();
#endif
        }

        public static void LogError(object message, string messageTitle = "ERROR") {
#if DEBUG
            var stack = new StackFrame(1, true);
            var mth = stack.GetMethod();
            var fname = stack.GetFileName();
            var lineNumber = stack.GetFileLineNumber();
            var fileName = fname?.Substring(fname.LastIndexOf("\\", StringComparison.InvariantCulture)+1);
            var className = mth.ReflectedType?.Name;
            var method = new StringBuilder();
            method.Append(mth.Name);
            method.Append("(");
            var methodParameters = mth.GetParameters();
            for (var i = 0; i < methodParameters.Length; i++) {
                method.Append(methodParameters[i].ParameterType);
                if (i != methodParameters.Length - 1) method.Append(", ");
            }

            method.Append(")");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.Write("[" + messageTitle + "]");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(" " + GetString(message));
            Console.ResetColor();
            Console.WriteLine(LogFormat.format(className, method, lineNumber, fileName));
            Console.ResetColor();
#endif
        }

        public static void Assert(bool condition, object message) {
#if ASSERT || DEBUG
            if (condition) return;
            var stack = new StackFrame(1, true);
            var fname = stack.GetFileName();
            var lineNumber = stack.GetFileLineNumber();
            var fileName = fname?.Substring(fname.LastIndexOf("\\", StringComparison.InvariantCulture)+1);
            Fail(message, "ASSERTION FAILED", lineNumber, fileName);
#endif
        }

        public static void Fail(object message, string messageTitle = "FAIL", int lineNumber = 0, string filePath = "") {
#if ASSERT || DEBUG
            var stack = new StackFrame(1, true);
            var mth = stack.GetMethod();
            var fname = stack.GetFileName();
            var fileName = fname?.Substring(fname.LastIndexOf("\\", StringComparison.InvariantCulture)+1);
            var className = mth.ReflectedType?.Name;
            if(lineNumber == 0)
                lineNumber = stack.GetFileLineNumber();
            var method = new StringBuilder();
            method.Append(mth.Name);
            method.Append("(");
            var methodParameters = mth.GetParameters();
            for (var i = 0; i < methodParameters.Length; i++) {
                method.Append(methodParameters[i].ParameterType);
                if (i != methodParameters.Length - 1) method.Append(", ");
            }

            method.Append(")");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.Write("[" + messageTitle + "]");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(" " + GetString(message));
            Console.ResetColor();
            Console.WriteLine(LogFormat.format(className, method, lineNumber, fileName));
            Console.ResetColor();
            Environment.Exit(4);
#endif
        }
    }
}