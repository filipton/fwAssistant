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

			/*switch (command)
			{
				case "następna piosenka":
					Spotify.SkipSong();
					break;
				case "poprzednia piosenka":
					Spotify.PrevoiusSong();
					break;
				case "jaka to piosenka":
				case "co to za piosenka":
				case "co jest aktualnie grane":
					var song = Spotify.GetActualPlayingSong();
					string retMessage = $"Aktualnie jest grane {song["Name"]} od {song["Artists"][0]["Name"]}!";
					TTS(retMessage);
					break;
				case "pokaż wszystkie playlisty":
				case "wszystkie playlisty":
					string playlists = string.Empty;
					var pl = Spotify.spotify.Playlists.CurrentUsers().Result.Items;
					for(int i = 0; i < pl.Count; i++)
					{
						playlists += $"{i+1}: {pl[i].Name}...";
					}

					TTS($"Twoje playlisty to: {playlists}");
					break;
				case "wznów odtwarzanie":
					Spotify.PausePlayback(false);
					break;
				case "przenieś odtwarzanie na głośniki":
					Spotify.BetterTransferPlayback();
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

						var pls = Spotify.spotify.Playlists.CurrentUsers().Result.Items;

						if (int.TryParse(args[args.Length - 1], out int pn))
						{
							if (pn >= 1 && pn <= pls.Count)
							{
								Console.WriteLine(pls[pn - 1].Uri);
								Spotify.PlaySong(pls[pn - 1].Uri);
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
									Spotify.PlaySong(playlist.Uri);
								}
							}
						}
					}
					break;
			}*/
		}
	}
}
