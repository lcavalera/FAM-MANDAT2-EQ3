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
        [ProducesResponseType(typeof(IEnumerable<RaisonDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IEnumerable<RaisonDTO>> Get()
        {
            return await _raisonService.Get();
        }

        /// <summary>
        /// Returns a specific consultation reason from its id
        /// </summary>
        /// <param name="id">id of the consultation reason to return</param>
        /// <response code="200">consultation reason found and returned</response>
        /// <response code="400">Bad Request : Element has been soft deleted</response>
        /// <response code="404">consultation reason not found for specified id</response>
        /// <response code="500">service currently unavailable</response>
        // GET api/<RaisonController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RaisonDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RaisonDTO>> Get(int id)
        {
            RaisonDTO raison = await _raisonService.GetById(id);

            return raison == null ?
                NotFound(new { Erreur = $"Consultation reason not found (id = {id})" }) : Ok(raison);
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
        [Consumes("application/json")]
        [ProducesResponseType(typeof(RaisonDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(RaisonDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody] RaisonDTO raison)
        {
            if (ModelState.IsValid)
            {
                await _raisonService.Add(raison);

                return CreatedAtAction("Get", new { id = raison.ID }, raison);
            }

            return BadRequest(ModelState);
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
        [ProducesResponseType(typeof(RaisonDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RaisonDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put(int id, [FromBody] RaisonDTO raison)
        {
            if (id != raison.ID)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                await _raisonService.Edit(raison);

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
        /// <response code="400">Bad Request (client side): consultation reason has been already deleted</response>
        /// <response code="404">consultation reason not found for specified id</response>
        /// <response code="500">service currently unavailable</response>
        // DELETE api/<RaisonController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(RaisonDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RaisonDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(int id)
        {
            RaisonDTO raison = await _raisonService.GetById(id);

            if (raison == null)
            {
                return NotFound();
            }

            await _raisonService.Delete(raison);

            return NoContent();
        }
    }
}
