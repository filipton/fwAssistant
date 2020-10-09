using fwAssistant.SpotifyApi;
using Google.Apis.Util;
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
			UserPlayback userPlayback = Spotify.GetUserPlayback();

			string command = ReplaceCmdPrefix(cmd, kvCmd.Key).ToLower();
			string[] args = command.Split(" ");

			switch (command)
			{
				case "następna piosenka":
					Spotify.NextSong();
					break;
				case "poprzednia piosenka":
					var plss = Spotify.GetPlaylists().Items;
					for(int i = 0; i < plss.Length; i++)
					{
						if(plss[i].Uri == userPlayback.context.Uri.Replace($"user:{plss[i].Owner.Id}:", ""))
						{
							Playlist playlist = Spotify.GetPlaylist(plss[i].Id);
							for(int x = 0; x < playlist.tracks.Items.Length; x++)
							{
								if(userPlayback.item.Id == playlist.tracks.Items[x].Track.Id)
								{
									Spotify.ResumePlayerPlayback(ctxUri: userPlayback.context.Uri, offset: (x == 0 ? x : (x - 1)));
									break;
								}
							}
							break;
						}
					}
					//Spotify.PreviousSong();
					break;
				case "zrestartuj piosenkę":
					var plsss = Spotify.GetPlaylists().Items;
					for (int i = 0; i < plsss.Length; i++)
					{
						if (plsss[i].Uri == userPlayback.context.Uri.Replace($"user:{plsss[i].Owner.Id}:", ""))
						{
							Playlist playlist = Spotify.GetPlaylist(plsss[i].Id);
							for (int x = 0; x < playlist.tracks.Items.Length; x++)
							{
								if (userPlayback.item.Id == playlist.tracks.Items[x].Track.Id)
								{
									Spotify.ResumePlayerPlayback(ctxUri: userPlayback.context.Uri, offset: x);
									break;
								}
							}
							break;
						}
					}
					break;
				case "jaka to piosenka":
				case "co to za piosenka":
				case "co jest aktualnie grane":
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