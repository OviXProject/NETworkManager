﻿using System;
using System.IO;

namespace NETworkManager.Models.PuTTY
{
    public partial class PuTTY
    {
        public static string BuildCommandLine(PuTTYSessionInfo profileInfo)
        {
            var command = string.Empty;

            // Protocol
            switch (profileInfo.Mode)
            {
                case ConnectionMode.SSH:
                    command += "-ssh";
                    break;
                case ConnectionMode.Telnet:
                    command += "-telnet";
                    break;
                case ConnectionMode.Serial:
                    command += "-serial";
                    break;
                case ConnectionMode.Rlogin:
                    command += "-rlogin";
                    break;
                case ConnectionMode.RAW:
                    command += "-raw";
                    break;
            }

            // Profile
            if (!string.IsNullOrEmpty(profileInfo.Profile))
                command += $" -load {'"'}{profileInfo.Profile}{'"'}";

            // Username
            if (!string.IsNullOrEmpty(profileInfo.Username))
                command += $" -l {profileInfo.Username}";

            // Log
            if (profileInfo.EnableLog)
            {
                switch(profileInfo.LogMode)
                {
                    case LogMode.SessionLog:
                        command += $" -sessionlog";
                        break;
                    case LogMode.SSHLog:
                        command += $" -sshlog";
                        break;
                    case LogMode.SSHRawLog:
                        command += $" -sshrawlog";
                        break;
                }

                command += $" {'"'}{ Environment.ExpandEnvironmentVariables(Path.Combine(profileInfo.LogPath, profileInfo.LogFileName))}{'"'}";
            }                
            
            // Additional commands
            if (!string.IsNullOrEmpty(profileInfo.AdditionalCommandLine))
                command += $" {profileInfo.AdditionalCommandLine}";

            // SerialLine, Baud
            if (profileInfo.Mode == ConnectionMode.Serial)
                command += $" {profileInfo.HostOrSerialLine} -sercfg {profileInfo.PortOrBaud}";

            // Port, Host
            if (profileInfo.Mode != ConnectionMode.Serial)
                command += $" -P {profileInfo.PortOrBaud} {profileInfo.HostOrSerialLine}";

            return command;
        }
    }
}
