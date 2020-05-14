using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using GXPEngine;

namespace game.utils {
    /// <summary>
    ///     Debug class with utility methods: Log, LogWarning, LogError and Assert
    /// </summary>
    public static class Debug {
        private const string LogFormat = " at {0}.{1}:{2} ({3}:line {2})";
        public static bool IsFileLoggerEnabled { get; private set; }
        private static StreamWriter logger;

        public static void EnableFileLogger(bool enabled) {
            if (enabled) {
                if (!Directory.Exists("logs")) {
                    Directory.CreateDirectory("logs");
                }

                logger = new StreamWriter($"logs/log_{Time.now}.txt");
                var sb = new StringBuilder();
                sb.AppendLine(new string('=', 64));
                sb.Append(new string('=', 23));
                sb.Append("System Information");
                sb.Append(new string('=', 23));
                sb.Append("\n");
                sb.AppendLine(new string('=', 64));
                sb.AppendLine($"{SystemInformation()}");
                sb.AppendLine(new string('=', 64));
                logger.WriteLine(sb.ToString());
            } else {
                if (logger != null) {
                    logger.Flush();
                    logger.Close();
                    logger = StreamWriter.Null;
                }
            }

            IsFileLoggerEnabled = enabled;
        }

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
            var fileName = fname?.Substring(fname.LastIndexOf("\\", StringComparison.InvariantCulture) + 1);
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
            if (IsFileLoggerEnabled) logger.WriteLine("[" + messageTitle + "]" + " " + GetString(message) + LogFormat.format(className, method, lineNumber, fileName));
#endif
        }

        public static void LogInfo(object message, string messageTitle = "INFO") {
#if DEBUG
            var stack = new StackFrame(1, true);
            var mth = stack.GetMethod();
            var fname = stack.GetFileName();
            var lineNumber = stack.GetFileLineNumber();
            var fileName = fname?.Substring(fname.LastIndexOf("\\", StringComparison.InvariantCulture) + 1);
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
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write("[" + messageTitle + "]");
            Console.ResetColor();
            Console.Write(" " + GetString(message));
            Console.ResetColor();
            Console.WriteLine(LogFormat.format(className, method, lineNumber, fileName));
            if (IsFileLoggerEnabled) logger.WriteLine("[" + messageTitle + "]" + " " + GetString(message) + LogFormat.format(className, method, lineNumber, fileName));
#endif
        }

        public static void LogWarning(object message, string messageTitle = "WARN") {
#if DEBUG
            var stack = new StackFrame(1, true);
            var mth = stack.GetMethod();
            var fname = stack.GetFileName();
            var lineNumber = stack.GetFileLineNumber();
            var fileName = fname?.Substring(fname.LastIndexOf("\\", StringComparison.InvariantCulture) + 1);
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
            if (IsFileLoggerEnabled) logger.WriteLine("[" + messageTitle + "]" + " " + GetString(message) + LogFormat.format(className, method, lineNumber, fileName));
#endif
        }

        public static void LogError(object message, string messageTitle = "ERROR") {
#if DEBUG
            var stack = new StackFrame(1, true);
            var mth = stack.GetMethod();
            var fname = stack.GetFileName();
            var lineNumber = stack.GetFileLineNumber();
            var fileName = fname?.Substring(fname.LastIndexOf("\\", StringComparison.InvariantCulture) + 1);
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
            if (IsFileLoggerEnabled) logger.WriteLine("[" + messageTitle + "]" + " " + GetString(message) + LogFormat.format(className, method, lineNumber, fileName));
#endif
        }

        public static void Assert(bool condition, object message) {
#if ASSERT || DEBUG
            if (condition) return;
            var stack = new StackFrame(1, true);
            var fname = stack.GetFileName();
            var lineNumber = stack.GetFileLineNumber();
            var fileName = fname?.Substring(fname.LastIndexOf("\\", StringComparison.InvariantCulture) + 1);
            Fail(message, "ASSERTION FAILED", lineNumber, fileName);
#endif
        }

        public static void Fail(object message, string messageTitle = "FAIL", int lineNumber = 0, string filePath = "") {
#if ASSERT || DEBUG
            var stack = new StackFrame(1, true);
            var mth = stack.GetMethod();
            var fname = stack.GetFileName();
            var fileName = fname?.Substring(fname.LastIndexOf("\\", StringComparison.InvariantCulture) + 1);
            var className = mth.ReflectedType?.Name;
            if (lineNumber == 0)
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

        private static string SystemInformation() {
            var stringBuilder = new StringBuilder(string.Empty);
            try {
                stringBuilder.AppendFormat("Operation System:  {0}\n", Environment.OSVersion);
                stringBuilder.AppendFormat($"\t\t  {(Environment.Is64BitOperatingSystem ? "64" : "32")} Bit Operating System\n");
                stringBuilder.AppendFormat("SystemDirectory:  {0}\n", Environment.SystemDirectory);
                stringBuilder.AppendFormat("ProcessorCount:  {0}\n", Environment.ProcessorCount);
                stringBuilder.AppendFormat("UserDomainName:  {0}\n", Environment.UserDomainName);
                stringBuilder.AppendFormat("UserName: {0}\n", Environment.UserName);
                //Drives
                stringBuilder.AppendFormat("LogicalDrives:\n");
                foreach (var driveInfo1 in DriveInfo.GetDrives()) {
                    try {
                        stringBuilder.AppendFormat("\t Drive: {0}\n\t\t VolumeLabel: " +
                                                    "{1}\n\t\t DriveType: {2}\n\t\t DriveFormat: {3}\n\t\t " +
                                                    "TotalSize: {4}\n\t\t AvailableFreeSpace: {5}\n",
                            driveInfo1.Name, driveInfo1.VolumeLabel, driveInfo1.DriveType,
                            driveInfo1.DriveFormat, driveInfo1.TotalSize, driveInfo1.AvailableFreeSpace);
                    } catch { }
                }
                stringBuilder.AppendFormat("SystemPageSize:  {0}\n", Environment.SystemPageSize);
                stringBuilder.AppendFormat("Version:  {0}", Environment.Version);
            } catch { }
            return stringBuilder.ToString();
        }
    }
}