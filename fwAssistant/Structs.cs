using System;
using System.Collections.Generic;
using System.Text;

namespace fwAssistant.Structs
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
}