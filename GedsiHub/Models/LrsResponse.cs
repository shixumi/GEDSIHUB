// Models/LrsResponse.cs
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GedsiHub.Models
{
    public class LrsResponse
    {
        [JsonProperty("statements")]
        public List<Statement> Statements { get; set; }
    }

    public class Statement
    {
        [JsonProperty("actor")]
        public Actor Actor { get; set; }

        [JsonProperty("verb")]
        public Verb Verb { get; set; }

        [JsonProperty("object")]
        public Activity Object { get; set; }

        [JsonProperty("result")]
        public Result Result { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
    }

    public class Actor
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("mbox")]
        public string Mbox { get; set; }
    }

    public class Verb
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("display")]
        public Dictionary<string, string> Display { get; set; }
    }

    public class Activity
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("definition")]
        public ActivityDefinition Definition { get; set; }
    }

    public class ActivityDefinition
    {
        [JsonProperty("name")]
        public Dictionary<string, string> Name { get; set; }

        [JsonProperty("description")]
        public Dictionary<string, string> Description { get; set; }
    }

    public class Result
    {
        [JsonProperty("success")]
        public bool? Success { get; set; }

        [JsonProperty("score")]
        public Score Score { get; set; }

        [JsonProperty("duration")]
        public TimeSpan Duration { get; set; }
    }

    public class Score
    {
        [JsonProperty("raw")]
        public double? Raw { get; set; }

        [JsonProperty("min")]
        public double? Min { get; set; }

        [JsonProperty("max")]
        public double? Max { get; set; }
    }
}
