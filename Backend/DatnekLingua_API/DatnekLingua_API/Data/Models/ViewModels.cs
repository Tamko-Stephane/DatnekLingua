using System;
using System.ComponentModel.DataAnnotations;

namespace DatnekLingua_API.Data.Models
{
    public class Level_Get
    {
        public Guid guid { get; set; }
        public string name { get; set; }
    }

    public class Language_Get
    {
        public Guid guid { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string description { get; set; }
    }

    public class UserConfigLanguage_Post
    {
        [Required]
        public Guid language_guid { get; set; }
        [Required]
        public Guid speak_level_guid { get; set; }
        [Required]
        public Guid write_level_guid { get; set; }
        [Required]
        public Guid understand_level_guid { get; set; }
    }

    public class UserConfigLanguage_Get
    {
        public Guid guid { get; set; }
        public string language_name { get; set; }
        public string level_speak_name { get; set; }
        public string level_write_name { get; set; }
        public string level_understand_name { get; set; }
        public bool isAppLanguage { get; set; }
        public bool isCompulsoryLanguage { get; set; }
    }
}
