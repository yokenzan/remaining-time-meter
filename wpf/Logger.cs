// <copyright file="Logger.cs" company="RemMeter">
// Copyright (c) 2025 RemMeter. Licensed under the MIT License.
// </copyright>

using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace RemMeter
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
            try
            {
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                LogDirectory = Path.Combine(appDataPath, "RemMeter");

                if (!Directory.Exists(LogDirectory))
                {
                    Directory.CreateDirectory(LogDirectory);
                }

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                LogFileInternal = Path.Combine(LogDirectory, $"debug_{timestamp}.log");
            }
            catch (UnauthorizedAccessException)
            {
                // Fallback to temp directory if no access to AppData
                LogDirectory = Path.GetTempPath();
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                LogFileInternal = Path.Combine(LogDirectory, $"RemMeter_debug_{timestamp}.log");
            }
            catch (DirectoryNotFoundException)
            {
                // Fallback to temp directory if path doesn't exist (more specific than IOException)
                LogDirectory = Path.GetTempPath();
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                LogFileInternal = Path.Combine(LogDirectory, $"RemMeter_debug_{timestamp}.log");
            }
            catch (IOException)
            {
                // Fallback to temp directory for other I/O issues
                LogDirectory = Path.GetTempPath();
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                LogFileInternal = Path.Combine(LogDirectory, $"RemMeter_debug_{timestamp}.log");
            }
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
            catch (UnauthorizedAccessException)
            {
                // Log to Windows Event Log as fallback if file access is denied
                TryLogToEventLog(level, message, memberName, sourceFilePath, sourceLineNumber);
            }
            catch (DirectoryNotFoundException)
            {
                // Directory was deleted - try to recreate or use Event Log
                TryRecreateLogDirectory();
                TryLogToEventLog(level, message, memberName, sourceFilePath, sourceLineNumber);
            }
            catch (PathTooLongException)
            {
                // Path too long - use Event Log
                TryLogToEventLog(level, message, memberName, sourceFilePath, sourceLineNumber);
            }
            catch (ArgumentException)
            {
                // Invalid path or characters - use Event Log
                TryLogToEventLog(level, message, memberName, sourceFilePath, sourceLineNumber);
            }
            catch (System.Security.SecurityException)
            {
                // Security restrictions - use Event Log
                TryLogToEventLog(level, message, memberName, sourceFilePath, sourceLineNumber);
            }
            catch (IOException)
            {
                // Disk full or other I/O issues - try Event Log fallback
                TryLogToEventLog(level, message, memberName, sourceFilePath, sourceLineNumber);
            }
        }

        /// <summary>
        /// Attempts to log to Windows Event Log as a fallback when file logging fails.
        /// </summary>
        /// <param name="level">The log level.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="memberName">The calling member name.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        private static void TryLogToEventLog(
            string level,
            string message,
            string memberName,
            string sourceFilePath,
            int sourceLineNumber)
        {
            try
            {
                string fileName = Path.GetFileName(sourceFilePath);
                string logMessage = $"[{level}] {fileName}:{sourceLineNumber} {memberName}() - {message}";

                // Write to Windows Event Log - Application log
                System.Diagnostics.EventLog.WriteEntry(
                    "RemMeter",
                    logMessage,
                    level == "ERROR" ? System.Diagnostics.EventLogEntryType.Error : System.Diagnostics.EventLogEntryType.Information);
            }
            catch
            {
                // If Event Log also fails, silently ignore to prevent cascading failures
            }
        }

        /// <summary>
        /// Attempts to recreate the log directory if it was deleted.
        /// </summary>
        private static void TryRecreateLogDirectory()
        {
            try
            {
                if (!Directory.Exists(LogDirectory))
                {
                    Directory.CreateDirectory(LogDirectory);
                }
            }
            catch
            {
                // If we can't recreate the directory, fallback logging will handle it
            }
        }
    }
}
