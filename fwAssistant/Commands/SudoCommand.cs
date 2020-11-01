using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace fwAssistant.Commands
{
    class SudoCommand : Command
    {
        public override void Run(string cmd, KeyValuePair<List<string>, Command> kvCmd)
        {
            //Console.WriteLine(ReplaceCmdPrefix(cmd, kvCmd.Key));
            ExecuteCommand(ReplaceCmdPrefix(cmd, kvCmd.Key));
        }

        string ExecuteCommand(string command)
        {
            //command = command.Replace("\"", "\"\"");

            Process p = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{command}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            p.Start();
            p.WaitForExit();

            return p.StandardOutput.ReadToEnd();
        }
    }
}