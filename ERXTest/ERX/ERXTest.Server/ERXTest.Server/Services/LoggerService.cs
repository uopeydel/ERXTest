using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ERXTest.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ERXTest.Server.Services
{
    public interface ILoggerService
    {
        //void LogInfo(string key, string value, string className, LogLevel logLevel = LogLevel.Information);
        //void LogObj(string key, object obj, string className, LogLevel logLevel = LogLevel.Information);
        void SetLogDesc(LogDesc logDesc);
        LogDesc GetLogDesc();
    }
    public class LoggerService : ILoggerService
    {
        private readonly ILogger<LoggerService> _logger;
        private LogDesc _logDesc = null;

        public LoggerService(ILogger<LoggerService> logger)
        {
            _logger = logger;
        }

        //private void LogData(LogLevel logLevel, string key, string value, string className)
        //{
        //    if (_logDesc == null)
        //    {
        //        _logger.LogInformation($" KEY [{key}] VALUE [{value}] CLASS[{className}] ");
        //    }
        //    else
        //    {
        //        var logMessage = (
        //            $@" {logLevel} APPNAME: {
        //                    _logDesc.AppName} IPCON: {
        //                    _logDesc.IpConnection} HOST: {
        //                    _logDesc.Host} THREAD: {
        //                    _logDesc.ThreadName} CLASS: {
        //                    className} REIP: {
        //                    _logDesc.RemoteIpAddress} BASEURI: {
        //                    _logDesc.BaseApiUrl} UUID: {
        //                    _logDesc.Uuid} METH: {
        //                    _logDesc.Method} PATH: {
        //                    _logDesc.Path} KEY: {
        //                    key} USERID: {
        //                    _logDesc.UserId} {
        //                    value} ");
        //        _logger.LogInformation(logMessage);
        //    }
        //}

        //public void LogInfo(string key, string value, string className, LogLevel logLevel = LogLevel.Information)
        //{
        //    value = string.IsNullOrEmpty(value) ? value : value.Replace(Environment.NewLine, " ");
        //    LogData(logLevel, key, value, className);
        //}

        //public void LogObj(string key, object obj, string className, LogLevel logLevel = LogLevel.Information)
        //{
        //    var jsonContent = JsonConvert.SerializeObject(obj, Formatting.None).Replace(Environment.NewLine, " ");
        //    jsonContent = Regex.Replace(jsonContent, "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");
        //    LogInfo(key, jsonContent, className, logLevel);
        //}

        public void SetLogDesc(LogDesc logDesc)
        {
            _logDesc = logDesc;
        }

        public LogDesc GetLogDesc()
        {
            return _logDesc;
        }
    }
}
