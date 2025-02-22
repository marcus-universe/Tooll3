﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using T3.Core.Operator;
using T3.Core.Utils;

namespace T3.Core.Logging
{
    /// <summary>
    /// A singleton that allows to log messages that are forwarded to <see cref="ILogWriter"/>s.
    /// </summary>
    public class Log
    {
        public static void Dispose()
        {
            foreach (var w in _instance._logWriters)
            {
                w.Dispose();
            }
        }

        public static void AddWriter(ILogWriter writer)
        {
            _instance._logWriters.Add(writer);
        }

        #region API for logging

        public static void Debug(string message, params object[] args)
        {
            ProcessAndLog(LogEntry.EntryLevel.Debug, message, args);
        }        
        
        public static void Info(string message, params object[] args)
        {
            ProcessAndLog(LogEntry.EntryLevel.Info, message, args);
        }  
        
        public static void Warning(string message, params object[] args)
        {
            ProcessAndLog(LogEntry.EntryLevel.Warning, message, args);
        }  
        
        public static void Error(string message, params object[] args)
        {
            ProcessAndLog(LogEntry.EntryLevel.Error, message, args);
        }
        
        public static void Assert(string message)
        {
            DoLog(new LogEntry(LogEntry.EntryLevel.Warning, message));
        }
        
        public static void Assert(string message, Guid sourceId)
        {
            DoLog(new LogEntry(LogEntry.EntryLevel.Warning, message, sourceId));
        }

        
        /// <summary>
        /// A helper function to unite different method API 
        /// </summary>
        private static void ProcessAndLog(LogEntry.EntryLevel level, string message, object[] args)
        {
            switch (args)
            {
                case { Length: 1 } when args[0] is Instance instance:
                {
                    DoLog(new LogEntry(level, message, OperatorUtils.BuildIdPathForInstance(instance).ToArray()));
                    break;
                }
                
                case { Length: 1 } when args[0] is List<Guid> idPath:
                    DoLog(new LogEntry(level, message, idPath.ToArray()));
                    break;
                case { Length: 1 } when args[0] is Guid[] idPathArray:
                    DoLog(new LogEntry(level, message, idPathArray));
                    break;
                default:
                {
                    var messageString = FormatMessageWithArguments(message, args);
                    DoLog(new LogEntry(level, messageString));
                    break;
                }
            }
        } 
        

        #endregion

        private static string FormatMessageWithArguments(string messageString, object[] args)
        {
            try
            {
                messageString = args.Length == 0 ? messageString : String.Format(messageString, args);
            }
            catch (FormatException)
            {
                DoLog(new LogEntry(LogEntry.EntryLevel.Info, "Ignoring arguments mal-formated debug message. Did you mess with curly braces?"));
            }
            return messageString;
        }
        
        private static void DoLog(LogEntry entry)
        {
            _instance._logWriters.ForEach(writer => writer.ProcessEntry(entry));
        }

        
        private static readonly Log _instance = new Log();
        private readonly List<ILogWriter> _logWriters = new List<ILogWriter>();
        private Log() { }   // Prevent construction
    }
}
