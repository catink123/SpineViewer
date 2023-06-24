#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SpineViewer.lib
{
    public enum MessageType { OpenFile, Record, ParameterChange, PageLoaded, FrameRendered, ReadyForFrame, BufferRequested };
    internal class CustomWebMessage
    {
        [JsonPropertyName("type")]
        public MessageType Type { get; set; }

        [JsonPropertyName("data")]
        public dynamic? Data { get; set; }

        public string ToJSON()
        {
            var options = new JsonSerializerOptions
            { 
                Converters =
                {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                }
            };
            return JsonSerializer.Serialize(this, options);
        }

        static public CustomWebMessage? FromJSON(string message)
        {
            var options = new JsonSerializerOptions
            {
                Converters =
                {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                }
            };
            return JsonSerializer.Deserialize<CustomWebMessage>(message, options);
        }
    }
}
