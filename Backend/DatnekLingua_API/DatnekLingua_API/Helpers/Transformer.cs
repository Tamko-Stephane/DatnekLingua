/// AUTHOR
/// TAMKO STEPHANE,
/// @contact: https://twitter.com/FlywingsS
using DatnekLingua_API.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DatnekLingua_API.Helpers
{
    public static class Transformer
    {
        public static List<T> GetLanguagesFromFile<T>(string languagesListFilePath) where T : Language, new()       //NAME, CODE, DESCRIPTION, IsCompulsoryToTheApplication
        {
            if (!string.IsNullOrEmpty(languagesListFilePath) && File.Exists(languagesListFilePath))
            {
                string[] lines = File.ReadAllLines(languagesListFilePath);
                if (lines != null && lines.Length > 1)      //contains header
                {
                    List<Language> list_t = new List<Language>();
                    string[] valid_booleans = new string[] { "true", "false" };
                    for (int index = 1; index < lines.Length; index++)
                    {
                        var line_info = (lines[index]).Split(';', StringSplitOptions.RemoveEmptyEntries);
                        list_t.Add(new Language()
                        {
                            Guid = Guid.NewGuid(),
                            Name = (line_info[0]).Trim(),
                            Code = (line_info[1]).Trim(),
                            Description = (line_info[2]).Trim(),
                            IsCompulsoryToTheApplication = valid_booleans.Contains((line_info[3]).Trim().ToLower()) ? bool.Parse((line_info[3]).Trim().ToLower()) : false
                        });
                    }

                    return list_t as List<T>;
                }

            }

            return null;
        }
    }
}
