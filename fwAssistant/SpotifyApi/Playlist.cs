using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace fwAssistant.SpotifyApi
{
    public class Playlist
    {
        [JsonProperty("collaborative")]
        public bool Collaborative { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("external_urls")]
        public ExternalUrls externalUrls { get; set; }

        [JsonProperty("followers")]
        public Followers followers { get; set; }

        [JsonProperty("href")]
        public Uri Href { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("images")]
        public Image[] Images { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("owner")]
        public Owner owner { get; set; }

        [JsonProperty("primary_color")]
        public object PrimaryColor { get; set; }

        [JsonProperty("public")]
        public bool Public { get; set; }

        [JsonProperty("snapshot_id")]
        public string SnapshotId { get; set; }

        [JsonProperty("tracks")]
        public Tracks tracks { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }


        public class ExternalUrls
        {
            [JsonProperty("spotify")]
            public Uri Spotify { get; set; }
        }

        public class Followers
        {
            [JsonProperty("href")]
            public object Href { get; set; }

            [JsonProperty("total")]
            public long Total { get; set; }
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

        public class Owner
        {
            [JsonProperty("display_name", NullValueHandling = NullValueHandling.Ignore)]
            public string DisplayName { get; set; }

            [JsonProperty("external_urls")]
            public ExternalUrls ExternalUrls { get; set; }

            [JsonProperty("href")]
            public Uri Href { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("type")]
            public OwnerType Type { get; set; }

            [JsonProperty("uri")]
            public string Uri { get; set; }

            [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
            public string Name { get; set; }
        }

        public class Tracks
        {
            [JsonProperty("href")]
            public Uri Href { get; set; }

            [JsonProperty("items")]
            public Item[] Items { get; set; }

            [JsonProperty("limit")]
            public long Limit { get; set; }

            [JsonProperty("next")]
            public object Next { get; set; }

            [JsonProperty("offset")]
            public long Offset { get; set; }

            [JsonProperty("previous")]
            public object Previous { get; set; }

            [JsonProperty("total")]
            public long Total { get; set; }
        }

        public class Item
        {
            [JsonProperty("added_at")]
            public DateTimeOffset AddedAt { get; set; }

            [JsonProperty("added_by")]
            public Owner AddedBy { get; set; }

            [JsonProperty("is_local")]
            public bool IsLocal { get; set; }

            [JsonProperty("primary_color")]
            public object PrimaryColor { get; set; }

            [JsonProperty("track")]
            public Track Track { get; set; }

            [JsonProperty("video_thumbnail")]
            public VideoThumbnail VideoThumbnail { get; set; }
        }

        public class Track
        {
            [JsonProperty("album")]
            public Album Album { get; set; }

            [JsonProperty("artists")]
            public Owner[] Artists { get; set; }

            [JsonProperty("available_markets")]
            public string[] AvailableMarkets { get; set; }

            [JsonProperty("disc_number")]
            public long DiscNumber { get; set; }

            [JsonProperty("duration_ms")]
            public long DurationMs { get; set; }

            [JsonProperty("episode")]
            public bool Episode { get; set; }

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

            [JsonProperty("track")]
            public bool TrackTrack { get; set; }

            [JsonProperty("track_number")]
            public long TrackNumber { get; set; }

            [JsonProperty("type")]
            public TrackType Type { get; set; }

            [JsonProperty("uri")]
            public string Uri { get; set; }
        }

        public class Album
        {
            [JsonProperty("album_type")]
            public AlbumTypeEnum AlbumType { get; set; }

            [JsonProperty("artists")]
            public Owner[] Artists { get; set; }

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
            public ReleaseDatePrecision ReleaseDatePrecision { get; set; }

            [JsonProperty("total_tracks")]
            public long TotalTracks { get; set; }

            [JsonProperty("type")]
            public AlbumTypeEnum Type { get; set; }

            [JsonProperty("uri")]
            public string Uri { get; set; }
        }

        public class ExternalIds
        {
            [JsonProperty("isrc")]
            public string Isrc { get; set; }
        }

        public class VideoThumbnail
        {
            [JsonProperty("url")]
            public object Url { get; set; }
        }
    }

    public enum OwnerType { Artist, User };

    public enum AlbumTypeEnum { Album, Compilation, Single };

    public enum ReleaseDatePrecision { Day };

    public enum TrackType { Track };

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                OwnerTypeConverter.Singleton,
                AlbumTypeEnumConverter.Singleton,
                ReleaseDatePrecisionConverter.Singleton,
                TrackTypeConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class OwnerTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(OwnerType) || t == typeof(OwnerType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "artist":
                    return OwnerType.Artist;
                case "user":
                    return OwnerType.User;
            }
            throw new Exception("Cannot unmarshal type OwnerType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (OwnerType)untypedValue;
            switch (value)
            {
                case OwnerType.Artist:
                    serializer.Serialize(writer, "artist");
                    return;
                case OwnerType.User:
                    serializer.Serialize(writer, "user");
                    return;
            }
            throw new Exception("Cannot marshal type OwnerType");
        }

        public static readonly OwnerTypeConverter Singleton = new OwnerTypeConverter();
    }

    internal class AlbumTypeEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(AlbumTypeEnum) || t == typeof(AlbumTypeEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "album":
                    return AlbumTypeEnum.Album;
                case "compilation":
                    return AlbumTypeEnum.Compilation;
                case "single":
                    return AlbumTypeEnum.Single;
            }
            throw new Exception("Cannot unmarshal type AlbumTypeEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (AlbumTypeEnum)untypedValue;
            switch (value)
            {
                case AlbumTypeEnum.Album:
                    serializer.Serialize(writer, "album");
                    return;
                case AlbumTypeEnum.Compilation:
                    serializer.Serialize(writer, "compilation");
                    return;
                case AlbumTypeEnum.Single:
                    serializer.Serialize(writer, "single");
                    return;
            }
            throw new Exception("Cannot marshal type AlbumTypeEnum");
        }

        public static readonly AlbumTypeEnumConverter Singleton = new AlbumTypeEnumConverter();
    }

    internal class ReleaseDatePrecisionConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ReleaseDatePrecision) || t == typeof(ReleaseDatePrecision?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "day")
            {
                return ReleaseDatePrecision.Day;
            }
            throw new Exception("Cannot unmarshal type ReleaseDatePrecision");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (ReleaseDatePrecision)untypedValue;
            if (value == ReleaseDatePrecision.Day)
            {
                serializer.Serialize(writer, "day");
                return;
            }
            throw new Exception("Cannot marshal type ReleaseDatePrecision");
        }

        public static readonly ReleaseDatePrecisionConverter Singleton = new ReleaseDatePrecisionConverter();
    }

    internal class TrackTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TrackType) || t == typeof(TrackType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "track")
            {
                return TrackType.Track;
            }
            throw new Exception("Cannot unmarshal type TrackType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (TrackType)untypedValue;
            if (value == TrackType.Track)
            {
                serializer.Serialize(writer, "track");
                return;
            }
            throw new Exception("Cannot marshal type TrackType");
        }

        public static readonly TrackTypeConverter Singleton = new TrackTypeConverter();
    }
}