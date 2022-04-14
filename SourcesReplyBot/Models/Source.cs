using System.ComponentModel.DataAnnotations;

namespace SourcesReplyBot.Models
{
    public class Source
    {
        [Key] public string key { get; set; }
        [Required] public string link { get; set; }
        public string? description { get; set; }
    }
}