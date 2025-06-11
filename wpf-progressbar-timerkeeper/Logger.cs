// <copyright file="Logger.cs" company="MeterTimeKeeper">
// Copyright (c) MeterTimeKeeper. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace ProgressBarTimerKeeper
{
    /// <summary>
    /// Simple logger for debugging purposes.
    /// </summary>
    public static class Logger
    {
        private static readonly object LockObject = new object();
        private static readonly string LogDirectory;
        private static readonly string LogFileInternal;

        /// <summary>
        /// Initializes static members of the <see cref="Logger"/> class.
        /// </summary>
        static Logger()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            LogDirectory = Path.Combine(appDataPath, "ProgressBarTimerKeeper");

            if (!Directory.Exists(LogDirectory))
            {
                Directory.CreateDirectory(LogDirectory);
            }

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            LogFileInternal = Path.Combine(LogDirectory, $"debug_{timestamp}.log");
        }

        /// <summary>
        /// Gets the current log file path.
        /// </summary>
        public static string LogFile => LogFileInternal;

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="memberName">The calling member name.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        public static void Debug(
            string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            WriteLog("DEBUG", message, memberName, sourceFilePath, sourceLineNumber);
        }

        /// <summary>
        /// Logs an info message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="memberName">The calling member name.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        public static void Info(
            string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            WriteLog("INFO", message, memberName, sourceFilePath, sourceLineNumber);
        }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="memberName">The calling member name.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        public static void Error(
            string message,
            Exception? exception = null,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            string fullMessage = exception != null ? $"{message}: {exception}" : message;
            WriteLog("ERROR", fullMessage, memberName, sourceFilePath, sourceLineNumber);
        }

        /// <summary>
        /// Writes a log entry to the log file.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="memberName">The calling member name.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        private static void WriteLog(
            string level,
            string message,
            string memberName,
            string sourceFilePath,
            int sourceLineNumber)
        {
            try
            {
                lock (LockObject)
                {
                    string fileName = Path.GetFileName(sourceFilePath);
                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    string logEntry = $"[{timestamp}] [{level}] {fileName}:{sourceLineNumber} {memberName}() - {message}";

                    File.AppendAllText(LogFileInternal, logEntry + Environment.NewLine);
                }
            }
            catch
            {
                // Ignore logging errors to prevent cascading failures
            }
        }
    }
}