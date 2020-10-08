using fwAssistant.Structs;
using Google.Cloud.TextToSpeech.V1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace fwAssistant
{
	class Services
	{
        public static void RunRoutinesServie()
		{
            Console.WriteLine("RUNNING ROUTINES SERVICE!");

            if (!File.Exists("routines.txt"))
            {
                Routines r = new Routines(new List<Routine>() { new Routine("25:54", "przykładowa komenda głosowa, która może się wykonać"), new Routine("HH:mm", "inna komenda głosowa, która ma się wykonać o danej godzinie") });
                File.WriteAllText("routines.txt", JsonConvert.SerializeObject(r, Formatting.Indented));
            }

            new Thread(() =>
            {
                DateTime now = DateTime.Now;
                string currenthhmm = $"{(now.Hour < 10 ? new string(now.Hour.ToString().Prepend('0').ToArray()) : now.Hour.ToString())}:{(now.Minute < 10 ? new string(now.Minute.ToString().Prepend('0').ToArray()) : now.Minute.ToString())}";
                DateTime myDate = DateTime.ParseExact(currenthhmm, "HH:mm",
                                       CultureInfo.GetCultureInfo("pl-PL")).AddMinutes(1);

                Thread.Sleep((int)(myDate - now).TotalMilliseconds+1000);
                Console.WriteLine("ROUTINE SERVICE FINALLY INITLIZED!");

				while (true)
				{
                    string routines = File.ReadAllText("routines.txt");

                    Routines r = JsonConvert.DeserializeObject<Routines>(routines);
                    foreach (Routine routine in r.AllRoutines)
                    {
                        if (DateTime.Now.ToString("HH:mm") == routine.time)
                        {
                            KeyValuePair<List<string>, Command> cmd = Program.CommandRegistered(routine.RoutineCommand);
                            if (!cmd.Equals(default(KeyValuePair<List<string>, Command>)))
                            {
                                Program.RunCommand(routine.RoutineCommand, cmd);
                            }
                        }
                    }

                    Thread.Sleep(60000);
				}
            }).Start();
        }

        public static void RunTTSSerivice()
		{
            Console.WriteLine("RUNNING TTS SERVICE!");

            var c = new TextToSpeechClientBuilder
            {
                JsonCredentials = File.ReadAllText("key.json")
            };
            Program.client = c.Build();

            Program.TTS("Initalized", volume: 0f);
        }
    }
}