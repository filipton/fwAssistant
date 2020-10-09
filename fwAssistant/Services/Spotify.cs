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
					Console.WriteLine(r.IsPlaying);
					SetVolume(100);
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

					if(error.ErrorError.Message == "The access token expired")
					{
						sRefreshToken();
						goto again;
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

					request.Content = new StringContent("{\"device_ids\":[\"" + "\"]}");
					request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

					var response = httpClient.SendAsync(request);
					string result = response.Result.Content.ReadAsStringAsync().Result;
					Error error = JsonConvert.DeserializeObject<Error>(result);

					if (error.ErrorError.Message == "The access token expired")
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
				using (var request = new HttpRequestMessage(new HttpMethod("PUT"), "https://api.spotify.com/v1/me/player/volume"))
				{
					request.Headers.TryAddWithoutValidation("Accept", "application/json");
					request.Headers.TryAddWithoutValidation("Authorization", $"Bearer {AcessToken}");

					var response = httpClient.SendAsync(request);
					string result = response.Result.Content.ReadAsStringAsync().Result;
					Error error = JsonConvert.DeserializeObject<Error>(result);

					if (error.ErrorError.Message == "The access token expired")
					{
						sRefreshToken();
						goto again;
					}
				}
			}
		}

		/*public static void SetVolume(int percent)
		{
			again:
			try 
			{
				spotify.Player.SetVolume(new PlayerVolumeRequest(percent));
			}
            catch(Exception e)
			{
				if (e.Message.Contains("The access token expired"))
				{
					sRefreshToken();
					spotify = new SpotifyClient(AcessToken);
					goto again;
				}
				else
				{
					Console.WriteLine(e.GetType() + " : " + e.Message);
				}
			}
		}

		public static void SkipSong()
		{
			again:
			try
			{
				spotify.Player.SkipNext();
			}
			catch (Exception e)
			{
				if (e.Message.Contains("The access token expired"))
				{
					sRefreshToken();
					spotify = new SpotifyClient(AcessToken);
					goto again;
				}
				else
				{
					Console.WriteLine(e.GetType() + " : " + e.Message);
				}
			}
		}

		public static void PrevoiusSong()
		{
			again:
			try
			{
				spotify.Player.SkipPrevious();
			}
			catch (Exception e)
			{
				if (e.Message.Contains("The access token expired"))
				{
					sRefreshToken();
					spotify = new SpotifyClient(AcessToken);
					goto again;
				}
				else
				{
					Console.WriteLine(e.GetType() + " : " + e.Message);
				}
			}
		}

		public static void PausePlayback(bool pause)
		{
			again:
			try
			{
				var x = spotify.Player.GetCurrentPlayback();
				if (x != null)
				{
					if (pause) spotify.Player.PausePlayback();
					else spotify.Player.ResumePlayback();
				}
			}
			catch (Exception e)
			{
				if (e.Message.Contains("The access token expired"))
				{
					sRefreshToken();
					spotify = new SpotifyClient(AcessToken);
					goto again;
				}
				else
				{
					Console.WriteLine(e.GetType() + " : " + e.Message);
				}
			}
		}

		public static dynamic GetActualPlayingSong()
		{
			again:
			try
			{
				var x = spotify.Player.GetCurrentlyPlaying(new PlayerCurrentlyPlayingRequest()).Result;
				if (x != null)
				{
					dynamic song = JsonConvert.DeserializeObject(x.Item.ToJson());
					return song;
				}
			}
			catch (Exception e)
			{
				if (e.Message.Contains("The access token expired"))
				{
					sRefreshToken();
					spotify = new SpotifyClient(AcessToken);
					goto again;
				}
				else
				{
					Console.WriteLine(e.GetType() + " : " + e.Message);
				}
			}

			return null;
		}

		public static void PlaySong(string ctxuri)
		{
			again:
			try
			{
				PlayerResumePlaybackRequest prpr = new PlayerResumePlaybackRequest();
				prpr.ContextUri = ctxuri;

				spotify.Player.ResumePlayback(prpr);
			}
			catch (Exception e)
			{
				if (e.Message.Contains("The access token expired"))
				{
					sRefreshToken();
					spotify = new SpotifyClient(AcessToken);
					goto again;
				}
				else
				{
					Console.WriteLine(e.GetType() + " : " + e.Message);
				}
			}
		}*/


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
