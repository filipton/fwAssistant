using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace fwAssistant.SpotifyApi
{
    public partial class Error
    {
        [JsonProperty("error")]
        public ErrorClass ErrorError { get; set; }
    }

    public partial class ErrorClass
    {
        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }
    }
}