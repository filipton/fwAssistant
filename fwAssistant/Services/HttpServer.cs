using fwAssistant.SpotifyApi;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace fwAssistant
{
	class HttpServer
	{
		public static HttpListener listener;

		public static void RunHttpServerService(int port)
		{
			string url = $"http://*:{port}/";

			new Thread(() =>
			{
				listener = new HttpListener();
				listener.Prefixes.Add(url);
				listener.Start();
				Console.WriteLine("RUNNING API SERVICE ON: {0}", url);

				Task listenTask = HandleIncomingConnections();
				listenTask.GetAwaiter().GetResult();

				listener.Close();
			}).Start();
		}

        public static async Task HandleIncomingConnections()
        {
            bool runServer = true;

            while (runServer)
            {
                HttpListenerContext ctx = await listener.GetContextAsync();

                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                byte[] data = Encoding.UTF8.GetBytes("NO RESPONSE");

                string[] args = req.Url.AbsolutePath.Remove(0, 1).Split('/');
                if (args.Length == 2)
                {
                    if (args[0] == "stt")
                    {
                        string text = HttpUtility.UrlDecode(args[1]).Replace("&PERCENT&", "%");
                        KeyValuePair<List<string>, Command> cmd = Program.CommandRegistered(text);

                        if (cmd.Equals(default(KeyValuePair<List<string>, Command>)))
						{
                            data = Encoding.UTF8.GetBytes("CMDNF");
                        }
						else
						{
                            new Thread(() =>
                            {
                                Program.RunCommand(text, cmd);
                            }).Start();
                        }
                    }
                }
                else if(args.Length == 1)
				{
                    if(args[0] == "stoptts")
					{
						if (Program.speaking)
						{
                            data = Encoding.UTF8.GetBytes("SPEAKING");

                            Program.speaking = false;
                            if(Program.ttsProcess != null) Program.ttsProcess.Kill();
                        }
                        new Thread(() =>
                        {
                            UserPlayback userPlayback = Spotify.GetUserPlayback();

                            if (userPlayback != null && userPlayback.IsPlaying && userPlayback.device.Id == Spotify.SpeakersSpotifyConnectId)
                            {
                                Program.MusicWasPlaying = true;
                                Spotify.PausePlayerPlayback();
                            }
                        }).Start();
                    }
				}

                resp.ContentType = "text/html";
                resp.ContentEncoding = Encoding.UTF8;
                resp.ContentLength64 = data.LongLength;

                await resp.OutputStream.WriteAsync(data, 0, data.Length);
                resp.Close();
            }
        }
    }
}