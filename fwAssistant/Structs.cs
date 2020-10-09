using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace fwAssistant
{
    [Serializable]
    public struct Routine
    {
        public string time;
        public string RoutineCommand;

        public Routine(string t, string cmd)
        {
            time = t;
            RoutineCommand = cmd;
        }
    }

    [Serializable]
    public struct Routines
    {
        public Routine[] AllRoutines;

        public Routines(Routine[] routines)
        {
            AllRoutines = routines;
        }
        public Routines(List<Routine> routines)
        {
            AllRoutines = routines.ToArray();
        }
    }

    public abstract class Command
    {
        public abstract void Run(string cmd, KeyValuePair<List<string>, Command> kvCmd);

        public void TTS(string msg, bool append = false, float volume = 1f, string lang = "pl-PL") => Program.TTS(msg, append, volume, lang);
        public void TTSThreaded(string msg, bool append = false, float volume = 1f, string lang = "pl-PL") => new Thread(() => { Program.TTS(msg, append, volume, lang); }).Start();

        public string ReplaceCmdPrefix(string cmd, List<string> keys)
		{
            string cmdRet = cmd;
            for(int i = 0; i < keys.Count; i++)
			{
				if (cmdRet.ToLower().StartsWith(keys[i]))
				{
                    cmdRet = cmdRet.Remove(0, keys[i].Length);

                    if (cmdRet.StartsWith(" ")) cmdRet = cmdRet.Remove(0, 1);

                    break;
				}
			}
            return cmdRet;
		}

        public bool _Interactivity => Program.Interactivity;
        public void RegisterInteractivity(KeyValuePair<List<string>, Command> kvCmd)
        {
            Program.commandForInteractivity = kvCmd;
            Program.Interactivity = true;
            new WebClient().DownloadString("http://localhost:21378/start");
        }
        public void UnRegisterInteractivity()
        {
            Program.Interactivity = false;
        }
    }
}