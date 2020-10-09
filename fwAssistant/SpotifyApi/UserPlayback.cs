using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace fwAssistant.SpotifyApi
{
    public class UserPlayback
    {
        [JsonProperty("device")]
        public Device device { get; set; }

        [JsonProperty("shuffle_state")]
        public bool ShuffleState { get; set; }

        [JsonProperty("repeat_state")]
        public string RepeatState { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("context")]
        public Context context { get; set; }

        [JsonProperty("progress_ms")]
        public long ProgressMs { get; set; }

        [JsonProperty("item")]
        public Item item { get; set; }

        [JsonProperty("currently_playing_type")]
        public string CurrentlyPlayingType { get; set; }

        [JsonProperty("actions")]
        public Actions actions { get; set; }

        [JsonProperty("is_playing")]
        public bool IsPlaying { get; set; }

        public class Actions
        {
            [JsonProperty("disallows")]
            public Disallows Disallows { get; set; }
        }

        public class Disallows
        {
            [JsonProperty("pausing")]
            public bool Pausing { get; set; }
        }

        public class Context
        {
            [JsonProperty("external_urls")]
            public ExternalUrls ExternalUrls { get; set; }

            [JsonProperty("href")]
            public Uri Href { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("uri")]
            public string Uri { get; set; }
        }

        public class ExternalUrls
        {
            [JsonProperty("spotify")]
            public Uri Spotify { get; set; }
        }

        public class Device
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("is_active")]
            public bool IsActive { get; set; }

            [JsonProperty("is_private_session")]
            public bool IsPrivateSession { get; set; }

            [JsonProperty("is_restricted")]
            public bool IsRestricted { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("volume_percent")]
            public long VolumePercent { get; set; }
        }

        public class Item
        {
            [JsonProperty("album")]
            public Album Album { get; set; }

            [JsonProperty("artists")]
            public Artist[] Artists { get; set; }

            [JsonProperty("available_markets")]
            public string[] AvailableMarkets { get; set; }

            [JsonProperty("disc_number")]
            public long DiscNumber { get; set; }

            [JsonProperty("duration_ms")]
            public long DurationMs { get; set; }

            [JsonProperty("explicit")]
            public bool Explicit { get; set; }

            [JsonProperty("external_ids")]
            public ExternalIds ExternalIds { get; set; }

            [JsonProperty("external_urls")]
            public ExternalUrls ExternalUrls { get; set; }

            [JsonProperty("href")]
            public Uri Href { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("is_local")]
            public bool IsLocal { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("popularity")]
            public long Popularity { get; set; }

            [JsonProperty("preview_url")]
            public Uri PreviewUrl { get; set; }

            [JsonProperty("track_number")]
            public long TrackNumber { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("uri")]
            public string Uri { get; set; }
        }

        public class Album
        {
            [JsonProperty("album_type")]
            public string AlbumType { get; set; }

            [JsonProperty("artists")]
            public Artist[] Artists { get; set; }

            [JsonProperty("available_markets")]
            public string[] AvailableMarkets { get; set; }

            [JsonProperty("external_urls")]
            public ExternalUrls ExternalUrls { get; set; }

            [JsonProperty("href")]
            public Uri Href { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("images")]
            public Image[] Images { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("release_date")]
            public DateTimeOffset ReleaseDate { get; set; }

            [JsonProperty("release_date_precision")]
            public string ReleaseDatePrecision { get; set; }

            [JsonProperty("total_tracks")]
            public long TotalTracks { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("uri")]
            public string Uri { get; set; }
        }

        public class Artist
        {
            [JsonProperty("external_urls")]
            public ExternalUrls ExternalUrls { get; set; }

            [JsonProperty("href")]
            public Uri Href { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("uri")]
            public string Uri { get; set; }
        }

        public class Image
        {
            [JsonProperty("height")]
            public long Height { get; set; }

            [JsonProperty("url")]
            public Uri Url { get; set; }

            [JsonProperty("width")]
            public long Width { get; set; }
        }

        public class ExternalIds
        {
            [JsonProperty("isrc")]
            public string Isrc { get; set; }
        }
    }
}