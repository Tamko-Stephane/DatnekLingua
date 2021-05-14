/// AUTHOR
/// TAMKO STEPHANE,
/// @contact: https://twitter.com/FlywingsS
using DatnekLingua_API.Data;
using DatnekLingua_API.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DatnekLingua_API.Controllers
{
    [Route("api/[controller]/[action]")]
    public class niveauxController : Controller
    {
        private readonly LanguagesDbContext context;

        public niveauxController(LanguagesDbContext context)
        {
            this.context = context;
        }

        // GET: api/<controller>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Parle()
        {
            return Ok(context.NiveauxParles.Select(np => new Level_Get()
            {
                guid = np.Guid,
                name = np.Name
            }));
        }

        // GET api/<controller>/5
        [HttpGet("{guid}")]
        public ActionResult<string> Parle(Guid guid)
        {
            //check guid value
            if (guid == Guid.Empty) return BadRequest("guid can not be null");
            //get level concerned
            var level = context.NiveauxParles.FirstOrDefault(np => np.Guid == guid);
            if (level == null) return NotFound("level is not found !");
            //return level found
            return Ok(new Level_Get()
            {
                guid = level.Guid,
                name = level.Name
            });
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Ecrit()
        {
            return Ok(context.NiveauxEcrits.Select(ne => new Level_Get()
            {
                guid = ne.Guid,
                name = ne.Name
            }));
        }

        // GET api/<controller>/5
        [HttpGet("{guid}")]
        public ActionResult<string> Ecrit(Guid guid)
        {
            //check guid value
            if (guid == Guid.Empty) return BadRequest("guid can not be null");
            //get level concerned
            var level = context.NiveauxEcrits.FirstOrDefault(ne => ne.Guid == guid);
            if (level == null) return NotFound("level is not found !");
            //return level found
            return Ok(new Level_Get()
            {
                guid = level.Guid,
                name = level.Name
            });
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Comprit()
        {
            return Ok(context.NiveauxComprehensions.Select(nc => new Level_Get()
            {
                guid = nc.Guid,
                name = nc.Name
            }));
        }

        // GET api/<controller>/5
        [HttpGet("{guid}")]
        public ActionResult<string> Comprit(Guid guid)
        {
            //check guid value
            if (guid == Guid.Empty) return BadRequest("guid can not be null");
            //get level concerned
            var level = context.NiveauxEcrits.FirstOrDefault(ne => ne.Guid == guid);
            if (level == null) return NotFound("level is not found !");
            //return level found
            return Ok(new Level_Get()
            {
                guid = level.Guid,
                name = level.Name
            });
        }
    }
}
