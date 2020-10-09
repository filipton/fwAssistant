using fwAssistant.SpotifyApi;
using Swan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace fwAssistant.Commands
{
	class SpotifySettings : Command
	{
		public override void Run(string cmd, KeyValuePair<List<string>, Command> kvCmd)
		{
			string command = ReplaceCmdPrefix(cmd, kvCmd.Key).ToLower();
			string[] args = command.Split(" ");

			switch (command)
			{
				case "następna piosenka":
					Spotify.NextSong();
					break;
				case "poprzednia piosenka":
					Spotify.PreviousSong();
					break;
				case "jaka to piosenka":
				case "co to za piosenka":
				case "co jest aktualnie grane":
					UserPlayback userPlayback = Spotify.GetUserPlayback();
					string retMessage = $"Aktualnie jest grane {userPlayback.item.Name} od {userPlayback.item.Artists[0].Name}!";
					TTS(retMessage);
					break;
				case "pokaż wszystkie playlisty":
				case "wszystkie playlisty":
					string playlists = string.Empty;
					var pl = Spotify.GetPlaylists().Items;
					for(int i = 0; i < pl.Length; i++)
					{
						playlists += $"{i+1}: {pl[i].Name}... ";
					}

					TTS($"Twoje playlisty to: {playlists}");
					break;
				case "zapauzuj odtwarzanie":
					Program.MusicWasPlaying = false;
					Spotify.PausePlayerPlayback();
					break;
				case "przenieś odtwarzanie na głośniki":
				case "przynieś odtwarzanie na":
				case "odtwarzanie na głośniki":
					Spotify.ResumePlayerPlayback();
					Spotify.TransferPlayback();
					Program.MusicWasPlaying = false;
					break;
				default:
					if(command.StartsWith("ustaw głośność na "))
					{
						int volume = int.Parse(args[args.Length - 1].Replace("%", ""));
						Spotify.SetVolume(volume);
					}
					else if(command.StartsWith("włącz playlistę ") || command.StartsWith("odpal playlistę ") || command.StartsWith("puść playlistę "))
					{
						Spotify.TransferPlayback();

						var pls = Spotify.GetPlaylists().Items;

						if (int.TryParse(args[args.Length - 1], out int pn))
						{
							if (pn >= 1 && pn <= pls.Length)
							{
								Console.WriteLine(pls[pn - 1].Uri);
								Spotify.ResumePlayerPlayback(ctxUri: pls[pn - 1].Uri);
							}
						}
						else
						{
							string pname = command.ToLower().Replace("włącz playlistę ", "").Replace("odpal playlistę ", "").Replace("puść playlistę ", "");

							foreach (var playlist in pls)
							{
								if (playlist.Name.ToLower() == pname)
								{
									Console.WriteLine(playlist.Uri);
									Spotify.ResumePlayerPlayback(ctxUri: playlist.Uri);
								}
							}
						}
					}
					break;
			}
		}
	}
}