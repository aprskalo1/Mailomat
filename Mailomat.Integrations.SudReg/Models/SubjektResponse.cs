using System.Text.Json.Serialization;
using Mailomat.Integrations.SudReg.Utils;

namespace Mailomat.Integrations.SudReg.Models
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SubjektResponse
    {
        public SubjektResponse()
        {
        }

        [JsonPropertyName("oib")]
        [JsonConverter(typeof(NumberToStringConverter))]
        public required string Oib { get; set; }

        [JsonPropertyName("mbs")]
        [JsonConverter(typeof(NumberToStringConverter))]
        public required string Mbs { get; set; }

        [JsonPropertyName("datum_osnivanja")] public DateTime DatumOsnivanja { get; set; }
    }
}