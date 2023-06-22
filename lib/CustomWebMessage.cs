using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SpineViewer.lib
{
    public enum MessageType { OpenFile, Record, Frame, ParameterChange };
    internal class CustomWebMessage
    {
        public MessageType type { get; set; }
        public dynamic data { get; set; }

        public CustomWebMessage(MessageType type, dynamic? data)
        {
            this.type = type;
            this.data = data;
        }

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

        static public CustomWebMessage FromJSON(string message)
        {
            return JsonSerializer.Deserialize<CustomWebMessage>(message);
        }
    }
}
