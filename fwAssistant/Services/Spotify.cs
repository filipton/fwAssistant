using Newtonsoft.Json;
using Swan;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Diagnostics.Contracts;
using System.Threading;
using fwAssistant.SpotifyApi;

namespace fwAssistant
{
	class Spotify
	{
		public static string ClientID;
		public static string ClientSECRET;
		public static string RefreshToken;
		public static string AcessToken;

		public static string SpeakersSpotifyConnectId = "32b1144cf54c71990ad802f030874995db7bf45d";

		public static void RunSpotifyService()
		{
			Console.WriteLine("RUNNING SPOTIFY SERVICE!");

			if (File.Exists("spotify_config.txt"))
			{
				string[] sconfig = File.ReadAllText("spotify_config.txt").Split(':');
				if(sconfig.Length == 4)
				{
					ClientID = sconfig[0];
					ClientSECRET = sconfig[1];
					RefreshToken = sconfig[2];
					AcessToken = sconfig[3];

					UserPlayback r = GetUserPlayback();
					//Console.WriteLine(r.IsPlaying);
					//SetVolume(100);
				}
				else
				{
					Console.WriteLine("SPOTIFY SERVICE CANNOT BE INITALIZED BECAUSE YOUR CONFIG FILE IS CORRUPTED!");
				}
			}
			else
			{
				Console.WriteLine("SPOTIFY SERVICE CANNOT BE INTALIZED BECAUSE YOU DOESNT SAVE CREDENTIALS TO FILE \"spotify_config.txt\" IN FORMAT: \"clientid:clientsecret:refreshtoken:authoken\"!");
			}
		}

		public static UserPlayback GetUserPlayback()
		{
			again:
			using (var httpClient = new HttpClient())
			{
				using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://api.spotify.com/v1/me/player"))
				{
					request.Headers.TryAddWithoutValidation("Accept", "application/json");
					request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {AcessToken}");

					var response = httpClient.SendAsync(request);
					string result = response.Result.Content.ReadAsStringAsync().Result;
					UserPlayback userPlayback = JsonConvert.DeserializeObject<UserPlayback>(result);
					Error error = JsonConvert.DeserializeObject<Error>(result);

					if(error != null && error.ErrorError != null)
					{
						if(error.ErrorError.Message == "The access token expired" || error.ErrorError.Message == "Invalid access token")
						{
							sRefreshToken();
							goto again;
						}
					}

					return userPlayback;
				}
			}
		}
		public static void TransferPlayback()
		{
			again:
			using (var httpClient = new HttpClient())
			{
				using (var request = new HttpRequestMessage(new HttpMethod("PUT"), "https://api.spotify.com/v1/me/player"))
				{
					request.Headers.TryAddWithoutValidation("Accept", "application/json");
					request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {AcessToken}");

					request.Content = new StringContent("{\"device_ids\":[\"" + SpeakersSpotifyConnectId + "\"]}");
					request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

					var response = httpClient.SendAsync(request);
					string result = response.Result.Content.ReadAsStringAsync().Result;
					Error error = JsonConvert.DeserializeObject<Error>(result);

					if (error != null && error.ErrorError.Message == "The access token expired")
					{
						sRefreshToken();
						goto again;
					}
				}
			}
		}

		public static void SetVolume(int percent)
		{
			again:
			using (var httpClient = new HttpClient())
			{
				using (var request = new HttpRequestMessage(new HttpMethod("PUT"), $"https://api.spotify.com/v1/me/player/volume?volume_percent={percent}"))
				{
					request.Headers.TryAddWithoutValidation("Accept", "application/json");
					request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {AcessToken}");

					var response = httpClient.SendAsync(request);
					string result = response.Result.Content.ReadAsStringAsync().Result;
					Error error = JsonConvert.DeserializeObject<Error>(result);

					if (error != null) Console.WriteLine(error.ErrorError.Message);

					if (error != null && error.ErrorError.Message == "The access token expired")
					{
						sRefreshToken();
						goto again;
					}
				}
			}
		}

		public static void ResumePlayerPlayback(string deviceId = "", string ctxUri = "", int offset = 0, int position_ms = 0)
		{
			again:
			using (var httpClient = new HttpClient())
			{
				using (var request = new HttpRequestMessage(new HttpMethod("PUT"), $"https://api.spotify.com/v1/me/player/play{(deviceId == string.Empty ? "" : $"?device_id={deviceId}")}"))
				{
					request.Headers.TryAddWithoutValidation("Accept", "application/json");
					request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {AcessToken}");

					if(ctxUri != string.Empty)
					{
						request.Content = new StringContent("{\"context_uri\":\"" + ctxUri + "\",\"offset\":{\"position\":" + offset + "},\"position_ms\":" + position_ms + "}");
						request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
					}

					var response = httpClient.SendAsync(request);
					string result = response.Result.Content.ReadAsStringAsync().Result;
					Error error = JsonConvert.DeserializeObject<Error>(result);

					if (error != null && error.ErrorError.Message == "The access token expired")
					{
						sRefreshToken();
						goto again;
					}
				}
			}
		}

		public static void PausePlayerPlayback()
		{
			again:
			using (var httpClient = new HttpClient())
			{
				using (var request = new HttpRequestMessage(new HttpMethod("PUT"), $"https://api.spotify.com/v1/me/player/pause"))
				{
					request.Headers.TryAddWithoutValidation("Accept", "application/json");
					request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {AcessToken}");

					var response = httpClient.SendAsync(request);
					string result = response.Result.Content.ReadAsStringAsync().Result;
					Error error = JsonConvert.DeserializeObject<Error>(result);

					if (error != null && error.ErrorError.Message == "The access token expired")
					{
						sRefreshToken();
						goto again;
					}
				}
			}
		}

		public static void NextSong()
		{
			again:
			using (var httpClient = new HttpClient())
			{
				using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.spotify.com/v1/me/player/next"))
				{
					request.Headers.TryAddWithoutValidation("Accept", "application/json");
					request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {AcessToken}");

					var response = httpClient.SendAsync(request);
					string result = response.Result.Content.ReadAsStringAsync().Result;
					Error error = JsonConvert.DeserializeObject<Error>(result);

					if (error != null && error.ErrorError.Message == "The access token expired")
					{
						sRefreshToken();
						goto again;
					}
				}
			}
		}

		public static void PreviousSong()
		{
			again:
			using (var httpClient = new HttpClient())
			{
				using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.spotify.com/v1/me/player/previous"))
				{
					request.Headers.TryAddWithoutValidation("Accept", "application/json");
					request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {AcessToken}");

					var response = httpClient.SendAsync(request);
					string result = response.Result.Content.ReadAsStringAsync().Result;
					Error error = JsonConvert.DeserializeObject<Error>(result);

					if (error != null && error.ErrorError.Message == "The access token expired")
					{
						sRefreshToken();
						goto again;
					}
				}
			}
		}

		public static Playlists GetPlaylists()
		{
			again:
			using (var httpClient = new HttpClient())
			{
				using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://api.spotify.com/v1/me/playlists"))
				{
					request.Headers.TryAddWithoutValidation("Accept", "application/json");
					request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {AcessToken}");

					var response = httpClient.SendAsync(request);
					string result = response.Result.Content.ReadAsStringAsync().Result;
					Playlists playlists = JsonConvert.DeserializeObject<Playlists>(result);
					Error error = JsonConvert.DeserializeObject<Error>(result);

					if (error != null && error.ErrorError != null)
					{
						if (error.ErrorError.Message == "The access token expired" || error.ErrorError.Message == "Invalid access token")
						{
							sRefreshToken();
							goto again;
						}
					}

					return playlists;
				}
			}
		}

		public static Playlist GetPlaylist(string plId)
		{
			again:
			using (var httpClient = new HttpClient())
			{
				using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://api.spotify.com/v1/playlists/{plId}"))
				{
					request.Headers.TryAddWithoutValidation("Accept", "application/json");
					request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {AcessToken}");

					var response = httpClient.SendAsync(request);
					string result = response.Result.Content.ReadAsStringAsync().Result;
					Playlist playlist = JsonConvert.DeserializeObject<Playlist>(result);
					Error error = JsonConvert.DeserializeObject<Error>(result);

					if (error != null && error.ErrorError != null)
					{
						if (error.ErrorError.Message == "The access token expired" || error.ErrorError.Message == "Invalid access token")
						{
							sRefreshToken();
							goto again;
						}
					}

					return playlist;
				}
			}
		}

		public static void sRefreshToken()
		{
			using (var httpClient = new HttpClient())
			{
				using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://accounts.spotify.com/api/token"))
				{
					request.Headers.TryAddWithoutValidation("Authorization", $"Basic {Convert.ToBase64String(Encoding.Default.GetBytes($"{ClientID}:{ClientSECRET}"))}");

					var contentList = new List<string>();
					contentList.Add($"grant_type={Uri.EscapeDataString("refresh_token")}");
					contentList.Add($"refresh_token={Uri.EscapeDataString(RefreshToken)}");
					request.Content = new StringContent(string.Join("&", contentList));
					request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

					var response = httpClient.SendAsync(request);
					dynamic json = JsonConvert.DeserializeObject(response.Result.Content.ReadAsStringAsync().Result);
					AcessToken = json["access_token"];
					Console.WriteLine(AcessToken);
					SaveConfig();
				}
			}
		}

		static void SaveConfig()
		{
			File.WriteAllText("spotify_config.txt", $"{ClientID}:{ClientSECRET}:{RefreshToken}:{AcessToken}");
		}
	}
}