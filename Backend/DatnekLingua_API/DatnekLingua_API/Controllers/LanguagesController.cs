/// AUTHOR
/// TAMKO STEPHANE,
/// @contact: https://twitter.com/FlywingsS
using DatnekLingua_API.Data;
using DatnekLingua_API.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatnekLingua_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguagesController : ControllerBase
    {
        private readonly LanguagesDbContext context;

        public LanguagesController(LanguagesDbContext context)
        {
            this.context = context;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return Ok(context.Languages.Select(l => new Language_Get()
            {
                guid = l.Guid,
                name = l.Name,
                code = l.Code,
                description = l.Description
            }).ToArray());
        }

        // GET api/values/5
        [HttpGet("{guid}")]
        public ActionResult<string> Get(Guid guid)
        {
            if (Guid.Empty != guid)
            {
                //get language referenced by guid
                var language = context.Languages.FirstOrDefault(l => l.Guid == guid);
                if (language != null) return Ok(new Language_Get()
                {
                    guid = language.Guid,
                    name = language.Name,
                    code = language.Code,
                    description = language.Description
                });
                return NotFound("object with guid: " + guid + " not found !");
            }

            return BadRequest("guid is incorrect");
        }

        //get the languages not yet configured
        [HttpGet("[action]")]
        public ActionResult<IEnumerable<string>> configurable()
        {
            //get languages configured
            var languages_configured = context.UserConfiguredLanguages.Select(ul => ul.LanguageConcerned).ToList();
            List<Language> languages = context.Languages.ToList();
            if (languages != null)
            {
                if (languages_configured == null || languages_configured.Count == 0) return Ok(languages.Select(l => new Language_Get()
                {
                    guid = l.Guid,
                    name = l.Name,
                    code = l.Code,
                    description = l.Description
                }).ToArray());

                //remove configured languages from 
                languages.RemoveAll(l => languages_configured.Contains(l));

                return Ok(languages.Select(l => new Language_Get()
                {
                    guid = l.Guid,
                    name = l.Name,
                    code = l.Code,
                    description = l.Description
                }).ToArray());
            }

            return StatusCode(417, "languages can not be null!");

        }
    }
}
