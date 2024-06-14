using Acef.MVC.Models.DTO;
using Acef.Raisons.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Acef.Raisons.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RaisonController : ControllerBase
    {
        private readonly IRaisonService _raisonService;

        public RaisonController(IRaisonService raisonService)
        {
            _raisonService = raisonService;
        }

        /// <summary>
        /// Returns a list of consultation reasons
        /// </summary>
        /// <response code="200">list of consultation reasons found and returned</response>
        /// <response code="404">list of consultation reasons not found</response>
        /// <response code="500">service currently unavailable</response>
        // GET: api/<RaisonController>
        [HttpGet]
        public async Task<IEnumerable<RaisonDTO>> Get()
        {
            return await _raisonService.ObtenirTout();
        }

        /// <summary>
        /// Returns a specific consultation reason from its id
        /// </summary>
        /// <param name="id">id of the consultation reason to return</param>
        /// <response code="200">consultation reason found and returned</response>
        /// <response code="404">consultation reason not found for specified id</response>
        /// <response code="500">service currently unavailable</response>
        // GET api/<RaisonController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            RaisonDTO raison = await _raisonService.ObtenirSelonId(id);

            if (raison == null)
            {
                return NotFound();
            }

            return Ok(raison);
        }

        /// <summary>
        /// Adds a consultation reason to the database
        /// </summary>
        /// <param name="raison">the consultation reason to add</param>
        /// <response code="201">consultation reason added successfully</response>
        /// <response code="400">invalid Model, bad request</response>
        /// <response code="500">service currently unavailable</response>
        // POST api/<RaisonController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] RaisonDTO raison)
        {
            if (ModelState.IsValid)
            {
                await _raisonService.Ajouter(raison);

                return CreatedAtAction("Get", new { id = raison.ID }, raison);
            }

            return BadRequest();
        }

        /// <summary>
        /// Modifying a consultation reason
        /// </summary>
        /// <param name="id">id of the consultation reason to modify</param>
        /// <param name="raison">the consultation reason with change</param>
        /// <response code="200">consultation reason successfully modified</response>
        /// <response code="204">consultation reason successfully modified (no response body data)</response>
        /// <response code="400">invalid Model, bad request</response>
        /// <response code="404">consultation reason not found for specified id</response>
        /// <response code="500">service currently unavailable</response>
        // PUT api/<RaisonController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] RaisonDTO raison)
        {
            if (id != raison.ID)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                await _raisonService.Modifier(raison);

                return NoContent();
            }
            return BadRequest();
        }

        /// <summary>
        /// Delete a consultation reason
        /// </summary>
        /// <param name="id">id of the consultation reason to delete</param>
        /// <response code="200">consultation reason successfully deleted</response>
        /// <response code="204">consultation reason successfully deleted (no response body data)</response>
        /// <response code="404">consultation reason not found for specified id</response>
        /// <response code="500">service currently unavailable</response>
        // DELETE api/<RaisonController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            RaisonDTO raison = await _raisonService.ObtenirSelonId(id);

            if (raison == null)
            {
                return NotFound();
            }

            await _raisonService.Supprimer(raison);

            return NoContent();
        }
    }
}
