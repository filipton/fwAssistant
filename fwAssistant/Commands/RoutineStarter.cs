using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace fwAssistant.Commands
{
	class RoutineStarter : Command
	{
		public override void Run(string cmd, KeyValuePair<List<string>, Command> kvCmd)
		{
            string routines = File.ReadAllText("routines.txt");

            Routines r = JsonConvert.DeserializeObject<Routines>(routines);
            string time = ReplaceCmdPrefix(cmd, kvCmd.Key);
            if (time.Split(":")[0].Length == 1) time = "0" + time;

            foreach (Routine routine in r.AllRoutines)
            {
                if (time == routine.time)
                {
                    KeyValuePair<List<string>, Command> c = Program.CommandRegistered(routine.RoutineCommand);
                    if (!cmd.Equals(default(KeyValuePair<List<string>, Command>)))
                    {
                        new Thread(() =>
                        {
                            Program.RunCommand(routine.RoutineCommand, c);
                        }).Start();
                    }
                }
            }
        }
	}
}
