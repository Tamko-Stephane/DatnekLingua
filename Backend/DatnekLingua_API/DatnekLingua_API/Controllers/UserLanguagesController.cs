/// AUTHOR
/// TAMKO STEPHANE,
/// @contact: https://twitter.com/FlywingsS
using DatnekLingua_API.Data;
using DatnekLingua_API.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DatnekLingua_API.Controllers
{
    [Route("api/[controller]")]
    public class UserLanguagesController : Controller
    {
        private readonly LanguagesDbContext context;

        public UserLanguagesController(LanguagesDbContext context)
        {
            this.context = context;
        }

        // GET: api/<controller>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            try
            {
                //get configured languages
                var user_config_languages = context.UserConfiguredLanguages.Select(ul => new UserConfigLanguage_Get()
                {
                    guid = ul.Guid,
                    isAppLanguage = ul.IsApplicationUserLanguage,
                    isCompulsoryLanguage = ul.LanguageConcerned.IsCompulsoryToTheApplication,
                    language_name = ul.LanguageConcerned != null ? ul.LanguageConcerned.Name : string.Empty,
                    level_speak_name = ul.NiveauParle != null ? ul.NiveauParle.Name : string.Empty,
                    level_write_name = ul.NiveauEcrit != null ? ul.NiveauEcrit.Name : string.Empty,
                    level_understand_name = ul.NiveauComprehension != null ? ul.NiveauComprehension.Name : string.Empty
                }
                    );

                return Ok(new
                {
                    total_count = user_config_languages.Count(),
                    data = user_config_languages
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(417, "Error: " + e.Message + " " + Environment.NewLine + e.Source);
            }
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<string>> Paginate(int page_size = 3, int page = 0)
        {
            try
            {
                //get the configured languages
                //paginate result
                var user_config_languages = context.UserConfiguredLanguages.Skip(page * page_size).Take(page_size).Select(ul => new UserConfigLanguage_Get()
                {
                    guid = ul.Guid,
                    isAppLanguage = ul.IsApplicationUserLanguage,
                    isCompulsoryLanguage = ul.LanguageConcerned.IsCompulsoryToTheApplication,
                    language_name = ul.LanguageConcerned != null ? ul.LanguageConcerned.Name : string.Empty,
                    level_speak_name = ul.NiveauParle != null ? ul.NiveauParle.Name : string.Empty,
                    level_write_name = ul.NiveauEcrit != null ? ul.NiveauEcrit.Name : string.Empty,
                    level_understand_name = ul.NiveauComprehension != null ? ul.NiveauComprehension.Name : string.Empty
                }
                    );
                return Ok(new
                {
                    total_count = context.UserConfiguredLanguages.Count(),
                    page,
                    data = user_config_languages
                });
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
                return StatusCode(417, "Error: " + e.Message + " " + Environment.NewLine + e.Source);
            }
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<string>> SetAppLanguage(Guid user_language_guid)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {

                    //check if there is with language guid as identifier of language
                    bool userLangIsAvailable = context.UserConfiguredLanguages.FirstOrDefault(ul => ul.Guid == user_language_guid) != null;
                    if (userLangIsAvailable)
                    {
                        //get the collection
                        var user_config_languages = context.UserConfiguredLanguages.ToList();
                        if (user_config_languages != null && user_config_languages.Count > 0)
                        {
                            //iterate the collection
                            foreach (var user_conf_lang in user_config_languages)
                            {
                                //set application language
                                user_conf_lang.IsApplicationUserLanguage = user_conf_lang.Guid == user_language_guid;
                                //save modification
                                context.Entry(user_conf_lang).State = EntityState.Modified;
                                context.SaveChanges();
                            }
                        }
                    }
                    //save changes from transaction
                    transaction.Commit();
                    return Ok(new { status = userLangIsAvailable ? HttpStatusCode.OK : HttpStatusCode.ExpectationFailed });

                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    Console.WriteLine(e);
                    return StatusCode(417, "Error: " + e.Message + " " + Environment.NewLine + e.Source);
                }
            }
        }

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<string>> getAppLanguageCode()
        {
            try
            {
                //check if there is with language guid as identifier of language
                var userLang = context.UserConfiguredLanguages.Include(ul => ul.LanguageConcerned).FirstOrDefault(ul => ul.IsApplicationUserLanguage);
                return Ok(new { code = userLang != null ? userLang.LanguageConcerned.Code : string.Empty });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(417, "Error: " + e.Message + " " + Environment.NewLine + e.Source);
            }
        }


        // GET api/<controller>/5
        [HttpGet("{guid}")]
        public ActionResult<string> Get(Guid guid)
        {
            if (guid != Guid.Empty)
            {
                try
                {
                    var user_language = context.UserConfiguredLanguages.
                                                Include(ul => ul.LanguageConcerned).
                                                Include(ul => ul.NiveauParle).
                                                Include(ul => ul.NiveauEcrit).
                                                Include(ul => ul.NiveauComprehension).
                                                FirstOrDefault(ul => ul.Guid == guid);
                    if (user_language != null) return Ok(new UserConfigLanguage_Get()
                    {
                        guid = user_language.Guid,
                        isAppLanguage = user_language.IsApplicationUserLanguage,
                        isCompulsoryLanguage = user_language.LanguageConcerned.IsCompulsoryToTheApplication,
                        language_name = user_language.LanguageConcerned.Name,
                        level_speak_name = user_language.NiveauParle.Name,
                        level_write_name = user_language.NiveauEcrit.Name,
                        level_understand_name = user_language.NiveauComprehension.Name
                    });
                    return NotFound("user configured language of guid: " + guid + " not found !");
                }
                catch (Exception e)
                {

                    Console.WriteLine(e);
                    return StatusCode(417, "Error: " + e.Message + " " + Environment.NewLine + e.Source);
                }
            }
            return BadRequest("guid: " + guid + " is incorrect");
        }

        // POST api/<controller>
        [HttpPost]
        public ActionResult<string> Post([FromBody]UserConfigLanguage_Post model)
        {
            if (!ModelState.IsValid) return BadRequest("model is incorrect due to:" + Environment.NewLine + GetModelStateErrors(ModelState));
            //create a userlanguage configuration instance
            //get language concerned                        
            try
            {
                var language = context.Languages.FirstOrDefault(l => l.Guid == model.language_guid);
                if (language == null) return BadRequest("language not recognized !");
                //level concerned
                var level_write = context.NiveauxEcrits.FirstOrDefault(ne => ne.Guid == model.write_level_guid);
                if (level_write == null) return BadRequest("write_level not recognized !");
                var level_speak = context.NiveauxParles.FirstOrDefault(np => np.Guid == model.speak_level_guid);
                if (level_speak == null) return BadRequest("speak_level not recognized !");
                var level_understand = context.NiveauxComprehensions.FirstOrDefault(nc => nc.Guid == model.understand_level_guid);
                if (level_understand == null) return BadRequest("understand_level not recognized !");

                //create user configured language
                var userLanguageConfig = new UserConfiguredLanguage()
                {
                    Guid = Guid.NewGuid(),
                    DateCreated = DateTime.Now,
                    LastModifiedDate = DateTime.Now,
                    LanguageGuid = language.Guid,
                    LanguageConcerned = language,
                    NiveauParleGuid = level_speak.Guid,
                    NiveauEcritGuid = level_write.Guid,
                    NiveauComprehensionGuid = level_understand.Guid,
                    NiveauParle = level_speak,
                    NiveauEcrit = level_write,
                    NiveauComprehension = level_understand
                };
                //save configured language
                context.UserConfiguredLanguages.Add(userLanguageConfig);
                context.SaveChanges();
                return Created($"{Url.Action("Get")}/{userLanguageConfig.Guid}", new UserConfigLanguage_Get()
                {
                    guid = userLanguageConfig.Guid,
                    isAppLanguage = userLanguageConfig.IsApplicationUserLanguage,
                    isCompulsoryLanguage = userLanguageConfig.LanguageConcerned.IsCompulsoryToTheApplication,
                    language_name = language.Name,
                    level_speak_name = level_speak.Name,
                    level_write_name = level_write.Name,
                    level_understand_name = level_understand.Name
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(417, "Error: " + e.Message + " " + Environment.NewLine + e.Source);
            }
        }




        private string GetModelStateErrors(ModelStateDictionary modelState)
        {
            if (modelState == null) return string.Empty;
            StringBuilder errorStringBuilder = new StringBuilder();
            foreach (var stateValue in modelState.Values)
            {
                if (stateValue != null)
                {
                    errorStringBuilder.Append(string.Join(" ", new string[] { stateValue.Errors[0]?.ErrorMessage, stateValue.Errors[0]?.Exception?.InnerException?.ToString() }));
                }
            }

            return errorStringBuilder.ToString();
        }

        // PUT api/<controller>/5
        [HttpPut("{guid}")]
        public ActionResult<string> Put(Guid guid, [FromBody]UserConfigLanguage_Post model)
        {
            if (guid == Guid.Empty) return BadRequest("guid can not be empty");
            if (!ModelState.IsValid)
                return BadRequest("model is incorrect due to:" + Environment.NewLine + GetModelStateErrors(ModelState));
            try
            {
                //find entity concerned with guid
                var user_language = context.UserConfiguredLanguages.FirstOrDefault(ul => ul.Guid == guid);
                if (user_language == null) return NotFound($"unknown user configured language of guid: {guid}");
                //modify the entity fields
                //get the levels
                var speak_level = context.NiveauxParles.FirstOrDefault(np => np.Guid == model.speak_level_guid);
                if (speak_level == null) return BadRequest("speak_level not recognized !");
                var write_level = context.NiveauxEcrits.FirstOrDefault(ne => ne.Guid == model.write_level_guid);
                if (write_level == null) return BadRequest("write_level not recognized !");
                var understand_level = context.NiveauxComprehensions.FirstOrDefault(nc => nc.Guid == model.understand_level_guid);
                if (understand_level == null) return BadRequest("understand_level not recognized !");
                user_language.NiveauParleGuid = model.speak_level_guid;
                user_language.NiveauEcritGuid = model.write_level_guid;
                user_language.NiveauComprehensionGuid = model.understand_level_guid;
                user_language.NiveauParle = speak_level;
                user_language.NiveauEcrit = write_level;
                user_language.NiveauComprehension = understand_level;

                //update entity
                context.Entry(user_language).State = EntityState.Modified;
                context.SaveChanges();

                return Ok(new UserConfigLanguage_Get()
                {
                    guid = user_language.Guid,
                    isAppLanguage = user_language.IsApplicationUserLanguage,
                    isCompulsoryLanguage = user_language.LanguageConcerned.IsCompulsoryToTheApplication,
                    language_name = user_language.LanguageConcerned.Name,
                    level_speak_name = user_language.NiveauParle.Name,
                    level_write_name = user_language.NiveauEcrit.Name,
                    level_understand_name = user_language.NiveauComprehension.Name
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(417, "Error: " + e.Message + " " + Environment.NewLine + e.Source);
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete("{guid}")]
        public ActionResult<string> Delete(Guid guid)
        {
            if (guid == Guid.Empty) return BadRequest("guid can not be empty");

            try
            {
                //get user configured language with guid
                var user_language = context.UserConfiguredLanguages
                    .Include(ul => ul.LanguageConcerned)
                    .Include(ul => ul.NiveauParle)
                    .Include(ul => ul.NiveauEcrit)
                    .Include(ul => ul.NiveauComprehension)
                    .FirstOrDefault(ul => ul.Guid == guid);
                if (user_language == null) return NotFound($"unknown user configured language of guid: {guid}");
                var deleted_UL_vm = new UserConfigLanguage_Get()
                {
                    guid = user_language.Guid,
                    isAppLanguage = user_language.IsApplicationUserLanguage,
                    isCompulsoryLanguage = user_language.LanguageConcerned.IsCompulsoryToTheApplication,
                    language_name = user_language.LanguageConcerned.Name,
                    level_speak_name = user_language.NiveauParle.Name,
                    level_write_name = user_language.NiveauEcrit.Name,
                    level_understand_name = user_language.NiveauComprehension.Name
                };
                //remove user configured language from DB
                context.UserConfiguredLanguages.Remove(user_language);
                context.SaveChanges();
                return Ok(deleted_UL_vm);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(417, "Error: " + e.Message + " " + Environment.NewLine + e.Source);
            }
        }
    }
}
