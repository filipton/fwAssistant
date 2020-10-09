using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using Google.Cloud.TextToSpeech.V1;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Net;
using System.Threading.Tasks;
using System.Text;
using System.Web;
using System.Runtime.CompilerServices;
using System.Globalization;
using Octokit;
using Swan;
using fwAssistant.Commands;

namespace fwAssistant
{
	class Program
	{
		public static TextToSpeechClient client;
        public static string Username = "Filip";
        public static bool speaking = false;
        public static Process ttsProcess;

        public static bool MusicWasPlaying = false;

        public static bool Interactivity = false;
        public static KeyValuePair<List<string>, Command> commandForInteractivity;

        public static Dictionary<List<string>, Command> commands = new Dictionary<List<string>, Command>()
        {
            [GetArgs("dzień dobry", "good morning")] = new GoodMorning(),
            [GetArgs("pogoda na")] = new Weather(),
            [GetArgs("powiedz", "powtórz", "zjebany jesteś")] = new SudoSay(),
            [GetArgs("czy możesz coś powiedzieć", "czy możesz coś dla mnie powiedzieć", "możesz coś powiedzieć")] = new InteractiveSudoSay(),
            [GetArgs("jak jest teraz na zewnątrz", "jak jest na zewnątrz")] = new ActualWeather(),
            [GetArgs("spotify")] = new SpotifySettings()
        };


        static void Main(string[] args)
        {
            HttpServer.RunHttpServerService(21377);
            Services.RunRoutinesServie();
            Services.RunTTSSerivice();
            Spotify.RunSpotifyService();
		}

        public static List<string> GetArgs(params string[] allArgs)
		{
            return allArgs.ToList();
		}

        public static bool RunCommand(string cmd, KeyValuePair<List<string>, Command> cmdKey)
		{
            if (string.IsNullOrEmpty(cmd))
                return false;

            Console.WriteLine($"==========================TREQUEST==========================");
            Console.WriteLine($"{cmd}");
            Console.WriteLine($"=======================END OF REQUEST=======================");
            Console.WriteLine();

            cmdKey.Value.Run(cmd, cmdKey);

            if (MusicWasPlaying && !Interactivity)
            {
                //Spotify.PausePlayback(false);
                MusicWasPlaying = false;
            }

            //int percent = int.Parse(cmd.ToLower().Replace("ustaw głośność spotify na ", "").Replace("%", ""));
            //Spotify.SetVolume(percent);
            //Spotify.PausePlayback(false);

            return true;
        }

        public static KeyValuePair<List<string>, Command> CommandRegistered(string cmd)
		{
            if (!Interactivity)
            {
                foreach (var cm in commands)
                {
                    foreach(string key in cm.Key)
					{
                        if (cmd.ToLower().StartsWith(key + " ") || cmd.ToLower() == key || cmd.ToLower().Equals(key))
                        {
                            return cm;
                        }
                    }
                }
            }
			else
			{
                return commandForInteractivity;
			}

            return default;
        }

        public static void TTS(string msg, bool append = false, float volume = 1f, string lang = "pl-PL")
        {
            if (speaking && append)
            {
                ttsProcess.Kill();
                speaking = false;
            }

            if (speaking) return;
            speaking = true;

            Console.WriteLine($"==========================RESPONSE==========================");
            Console.WriteLine($"{msg}");
            Console.WriteLine($"=======================END OF RESPONSE======================");
            Console.WriteLine();

            var input = new SynthesisInput
            {
                Text = msg
            };

            var voiceSelection = new VoiceSelectionParams
            {
                LanguageCode = lang,
                SsmlGender = SsmlVoiceGender.Female
            };

            var audioConfig = new AudioConfig
            {
                AudioEncoding = AudioEncoding.Mp3
            };

            var response = client.SynthesizeSpeech(input, voiceSelection, audioConfig);

            byte[] r = new byte[response.AudioContent.Length];
            response.AudioContent.CopyTo(r, 0);

            using (var output = File.Create("out.mp3"))
            {
                output.Write(r, 0, r.Length);
            }

            ExecuteMpg("out.mp3", volume);
            speaking = false;
        }

        static string ExecuteMpg(string command, float volume = 1f)
        {
            command = command.Replace("\"", "\"\"");

            ttsProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/usr/bin/mpg123",
                    Arguments = $"-q -f -{(int)(32768*volume)} " + command,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            ttsProcess.Start();
            ttsProcess.WaitForExit();

            return ttsProcess.StandardOutput.ReadToEnd();
        }
    }
}